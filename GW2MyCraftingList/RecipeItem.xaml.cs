using System;
using System.Collections;
using System.Collections.Generic;
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
using System.ComponentModel;

namespace GW2ExplorerCraftTool
{
    /// <summary>
    /// Logique d'interaction pour RecipeItem.xaml
    /// </summary>
    public partial class RecipeItem : UserControl
    {

        private MainWindow _parentWindow;
        private bool _isExpanded = false;
        private List<IngredientItem> ingredientItems;
        private ListCollectionView ingredientsView;

        private List<TradingPostDataItem> priceItems;
        private ListCollectionView pricesView;

        private Data.Recipe _recipe;

        public Data.Recipe Recipe
        {
            get
            {
                return this._recipe;
            }
        }

        public RecipeItem(Data.Recipe recipe, MainWindow parentWindow)
        {
            InitializeComponent();
            this._recipe = recipe;
            this._parentWindow = parentWindow;

            // hide others components
            this.cIconContainer.Visibility = System.Windows.Visibility.Collapsed;
            this.tbRecipeDescription.Visibility = System.Windows.Visibility.Collapsed;
            this.tbGwSpidyHyperLinkContainer.Visibility = System.Windows.Visibility.Collapsed;
            this.tbWikiHyperLinkContainer.Visibility = System.Windows.Visibility.Collapsed;

            if (this._recipe.SomethingIsWrong)
            {
                this.tbRecipeName.Foreground = new System.Windows.Media.SolidColorBrush(Colors.Red);
                this.tbRecipeName.Text = String.Format("[{0}] {1}", this._recipe.Id, Properties.Resources.SomethingIsWrongWithThisRecipe);
                // hide button bExpandRecipe
                this.bExpandRecipe.Visibility = System.Windows.Visibility.Collapsed;
                // destruct tooltip over the recipe name
                this.tbRecipeName.ToolTip = null;
                return;
            }

            if (this._recipe.Is_checked)
                this.cbCheckRecipe.IsChecked = true;
            this.tbRecipeName.Text = String.Format("[{0}] {1}", this._recipe.Id, this._recipe.Name);
            this.tbRecipeName.Foreground = this._recipe.RarityColor;

            if (!String.IsNullOrEmpty(recipe.Image))
            {
                this.iRecipeIcon.ImageSource = new BitmapImage(new Uri(recipe.Image));
            }
            this.cIconContainer.Visibility = System.Windows.Visibility.Visible;

            this.iTradingPost.Source = new BitmapImage(new Uri(Config.TRADING_POST_ICON_URL));
            this.tbOutputItemsCount.Text = recipe.OutputItemsCount.ToString();

            hlGw2SpidyLink.NavigateUri = new Uri(string.Format(Properties.Resources.Gw2SpidyItemHyperLinkUrlBase, recipe.OutputItemId));
            hlWikiLink.NavigateUri = new Uri(string.Format(Properties.Resources.WikiHyperLinkUrlBase, recipe.Name));
            this.tbWikiHyperLinkContainer.Visibility = System.Windows.Visibility.Visible;
            this.tbGwSpidyHyperLinkContainer.Visibility = System.Windows.Visibility.Visible;

        }

        private void cbCheckRecipe_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Recipe.Is_checked = true;
                this.SaveChange(this.Recipe);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._parentWindow, ex.Message);
            }
        }

        private void cbCheckRecipe_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Recipe.Is_checked = false;
                this.SaveChange(this.Recipe);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._parentWindow, ex.Message);
            }
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

        private void SaveChange(Data.Recipe recipe)
        {
                // save modifications
            BackgroundWorker recipeCheckerWorker = new BackgroundWorker();
            recipeCheckerWorker.DoWork +=
                (object sender, DoWorkEventArgs e) =>
                {
                    Data.Cache.GetCache().SaveRecipe(recipe);
                };
            recipeCheckerWorker.RunWorkerCompleted +=
                (object sender, RunWorkerCompletedEventArgs e) =>
                {
                    if (e.Error != null)
                    {
                        MessageBox.Show(((Exception)e.Error).Message, "Erreur");
                    }
                };
            recipeCheckerWorker.RunWorkerAsync();
        }

        private void tbRecipeName_ToolTipOpening(object sender, ToolTipEventArgs e)
        {
            this.tbRecipeToolTipName.Text = String.Format("[{0}] {1}", this._recipe.OutputItemId, this._recipe.Name);
            this.tbRecipeToolTipName.Foreground = this._recipe.RarityColor;
            this.tbRecipeToolTipDescription.Text = String.Format("{0}", this._recipe.Item.ToString());
            if (!String.IsNullOrEmpty(this._recipe.Image))
            {
                this.iRecipeToolTipIcon.Source = new BitmapImage(new Uri(this._recipe.Image));
            }

        }

        TradingPostDataItem tpdItem = null;
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
                        _worker.ReportProgress(0, this._recipe.GetTradingPostData());
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

        private void bExpandRecipe_Click(object sender, RoutedEventArgs e)
        {
            if (this._isExpanded)
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

                this.tbButtonExpandIngredients.Text = "+";
                this.ingredientsContainer.Visibility = System.Windows.Visibility.Collapsed;
                this.tbRecipeDescription.Visibility = System.Windows.Visibility.Collapsed;

            }
            else
            {
                this.ingredientItems = new List<IngredientItem>();
                this.ingredientsView = new ListCollectionView(this.ingredientItems);
                this.ingredientsContainer.ItemsSource = this.ingredientsView;

                this.tbRecipeDescription.Text = String.Format("{0}", this._recipe);
                this.tbRecipeDescription.Visibility = System.Windows.Visibility.Visible;

                Array.ForEach<Data.Ingredient>(this._recipe.Ingredients.ToArray(),
                    p =>
                    {
                        IngredientItem ii = new IngredientItem(p, _parentWindow, 0);
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

    public class RecipeItemComparer : IComparer
    {
        public int Compare(object a, object b)
        {
            string s1 = ((RecipeItem)a).tbRecipeName.Text;
            string s2 = ((RecipeItem)b).tbRecipeName.Text;
            return s1.CompareTo(s2);
        }
    }
}
