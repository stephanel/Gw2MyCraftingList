using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GW2ExplorerCraftTool
{
    /// <summary>
    /// Logique d'interaction pour IngredientItem.xaml
    /// </summary>
    public partial class IngredientItem : UserControl
    {
        private MainWindow _parentWindow;
        private Data.Ingredient _ingredient;
        private int _level;
        private bool _isExpanded = false;

        private List<TradingPostDataItem> priceItems;
        private ListCollectionView pricesView;

        private List<IngredientItem> ingredientItems;
        private ListCollectionView ingredientsView;

        public Data.Ingredient Item
        {
            get
            {
                return this._ingredient;
            }
        }

        public IngredientItem(Data.Ingredient ingredient, MainWindow parentWindow, int level)
        {
            InitializeComponent();

            this.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;

            this._parentWindow = parentWindow;
            this._ingredient = ingredient;
            this._level = level;

            if (this._ingredient.SomethingIsWrong)
            {
                this.tbIngredientName.Foreground = new System.Windows.Media.SolidColorBrush(Colors.Red);
                this.tbIngredientName.Text = String.Format("[{0}] {1}", this._ingredient.Id, Properties.Resources.SomethingIsWrongWithThisIngredient);

                // hide others components
                this.cIconContainer.Visibility = System.Windows.Visibility.Collapsed;
                this.tbIngredientsCount.Visibility = System.Windows.Visibility.Collapsed;

                // destruct tooltip
                this.tbIngredientName.ToolTip = null;

                return;

            }

            if (this._level > 0)
            {
                BitmapImage bi = new BitmapImage(new Uri(Config.HIERARCHIC_INGREDIENT_URL));
                this.iHierarchic.Source = bi;
                this.iHierarchic.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                this.iHierarchic.Visibility = System.Windows.Visibility.Collapsed;
            }

            this.tbIngredientName.Text = ingredient.Name;
            this.tbIngredientName.Foreground = ingredient.RarityColor;

            if (!String.IsNullOrEmpty(ingredient.Image))
                this.iIngredientIcon.ImageSource = new BitmapImage(new Uri(ingredient.Image));
            this.tbIngredientsCount.Text = ingredient.Count.ToString();

            this.iTradingPost.Source = new BitmapImage(new Uri(Config.TRADING_POST_ICON_URL));

            this.ingredientsContainer.Visibility = System.Windows.Visibility.Collapsed;
            if (ingredient.IsCraftable)
            {
                this.bExpandIngredients.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                this.bExpandIngredients.Visibility = System.Windows.Visibility.Hidden;
            }
        }

        private void tbIngredientName_ToolTipOpening(object sender, ToolTipEventArgs e)
        {
            this.tbItemToolTipName.Text = String.Format("[{0}] {1}", this._ingredient.Id, this._ingredient.Name);
            this.tbItemToolTipName.Foreground = this._ingredient.RarityColor;
            this.tbItemToolTipDescription.Text = String.Format("{0}", this._ingredient);
            if (!String.IsNullOrEmpty(this._ingredient.Image))
            {
                this.iItemToolTipIcon.Source = new BitmapImage(new Uri(this._ingredient.Image));
            }
        }

        TradingPostDataItem tpdItem = null;
        /*private void GetGw2SpidyData(int item_id)
        {
            try
            {
                // first, clear existing component
                if (this.priceItems != null && this.priceItems.Count > 0)
                {
                    this.priceItems.Remove(this.tpdItem);

                }
                // re-init data list
                this.priceItems = new List<TradingPostDataItem>();

                this.pricesView = new ListCollectionView(this.priceItems);
                this.pricesContainer.ItemsSource = this.pricesView;

                this.tpdItem = new TradingPostDataItem(this._ingredient.GetTradingPostData(), _parentWindow);
                if (!this.tpdItem.WrongState)
                {
                    this.priceItems.Add(tpdItem);
                    this.pricesView.Refresh();
                    this.pricesContainer.UpdateLayout();
                }
            }
            catch (Exception)
            {
                // do nohting
            }
        }*/

        private void bRequestTradingPostData_Click(object sender, RoutedEventArgs e)
        {
            // first, clear existing component
            if (this.priceItems != null && this.priceItems.Count > 0)
            {
                this.priceItems.Remove(this.tpdItem);

            }
            this.tpdItem = null;
            // re-init data list
            this.priceItems = new List<TradingPostDataItem>();

            BackgroundWorker _worker = new BackgroundWorker();
            _worker.WorkerReportsProgress = true;
            _worker.DoWork +=
                (object _sender, DoWorkEventArgs _e) =>
                {
                    try
                    {
                        _worker.ReportProgress(0, this._ingredient.GetTradingPostData());
                    }
                    catch (Exception)
                    {
                        // Ignore exceptions at this point - failed to take from queue
                    }
                };
            _worker.ProgressChanged +=
                (object _sender, ProgressChangedEventArgs _e) =>
                {
                    Data.TradingPostData _data = (Data.TradingPostData)_e.UserState;
                    this.pricesView = new ListCollectionView(this.priceItems);
                    this.pricesContainer.ItemsSource = this.pricesView;

                    this.tpdItem = new TradingPostDataItem(_data, _parentWindow);
                    if (!this.tpdItem.WrongState)
                    {
                        this.priceItems.Add(this.tpdItem);
                        this.pricesView.Refresh();
                        this.pricesContainer.UpdateLayout();
                    }
                };
            _worker.RunWorkerAsync();
        }

        public void ClearIngredients()
        {
            if (this.ingredientsView != null)
            {
                for (int i = 0; i < this.ingredientItems.Count; i++)
                {
                    IngredientItem ii = (IngredientItem)this.ingredientsView.GetItemAt(i);
                    ii.ClearIngredients();
                    ii = null;
                }
                this.ingredientItems.Clear();
                this.ingredientItems = null;
                this.ingredientsView = null;
                this.ingredientsContainer.UpdateLayout();
            }
        }

        private void bExpandIngredients_Click(object sender, RoutedEventArgs e)
        {
            if(this._isExpanded)
            {
                /*-- remove crafting hierarchy for each ingredients dynamically --*/
                this.ClearIngredients();
                this.tbButtonExpandIngredients.Text = "+";
                this.ingredientsContainer.Visibility = System.Windows.Visibility.Collapsed;
            }
            else
            {
                /*-- Add crafting hierarchy for each ingredients dynamically --*/
                this.ingredientItems = new List<IngredientItem>();
                this.ingredientsView = new ListCollectionView(this.ingredientItems);
                this.ingredientsContainer.ItemsSource = this.ingredientsView;

                Data.Recipe _recipe = this._ingredient.CreatedFrom;
                Array.ForEach<Data.Ingredient>(_recipe.Ingredients.ToArray(),
                    p =>
                    {
                        IngredientItem ii = new IngredientItem(p, _parentWindow, this._level + 1);
                        this.ingredientItems.Add(ii);
                    });

                this.ingredientsView.Refresh();
                this.ingredientsContainer.UpdateLayout();

                this.tbButtonExpandIngredients.Text = "-";
                this.ingredientsContainer.Visibility = System.Windows.Visibility.Visible;
            }
            this._isExpanded = !this._isExpanded;
        }

    }
}
