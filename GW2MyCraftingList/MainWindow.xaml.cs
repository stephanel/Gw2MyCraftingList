using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Threading;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace GW2ExplorerCraftTool
{
    enum SearchMode
    {
        Normal = 1,
        RecipesInError = 2,
        LatestDownload = 3
    }

    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private List<RecipeItem> recipeItems;
        private ListCollectionView recipeView;
        //private ViewModel _viewModel;

        public MainWindow()
        {
            InitializeComponent();

            this.Title = App.Name;

            // generate welcome msg
            System.IO.MemoryStream stream = 
                new System.IO.MemoryStream(
                    ASCIIEncoding.UTF8.GetBytes(Properties.Resources.RichTextBoxWelcomeText));
            // Convert to object
            FlowDocument doc = (FlowDocument)System.Windows.Markup.XamlReader.Load(stream);
            doc.FontFamily = new FontFamily("Segoe UI");
            doc.FontSize = 12;
            this.rtbWelcomeMsg.Document = doc;
            Hyperlink hl = LogicalTreeHelper.FindLogicalNode(doc, "hlGw2Spidy") as Hyperlink;
            Hyperlink hl2 = LogicalTreeHelper.FindLogicalNode(doc, "hlGw2Spidy2") as Hyperlink;
            Hyperlink hl3 = LogicalTreeHelper.FindLogicalNode(doc, "hlGw2Db") as Hyperlink;
            hl.RequestNavigate += new RequestNavigateEventHandler(Hyperlink_RequestNavigate);
            hl2.RequestNavigate += new RequestNavigateEventHandler(Hyperlink_RequestNavigate);
            hl3.RequestNavigate += new RequestNavigateEventHandler(Hyperlink_RequestNavigate);

            // initialize comboboxes
            this.LoadDisciplines();
            this.LoadLanguages();
            this.LoadRarities();
            this.LoadRecipeTypes();
            this.LoadTextSearchModes();

            //this.DataContext = new MainWindowViewModel(this);

            recipeItems = new List<RecipeItem>();
            recipeView = new ListCollectionView(recipeItems);
            recipeView.CustomSort = (IComparer)(new RecipeItemComparer());
            recipesContainer.ItemsSource = recipeView;

            this.cbDisciplines.IsEnabled = false;
            this.cbRarities.IsEnabled = false;
            this.cbRecipeTypes.IsEnabled = false;
            this.cbSearchAlsoIngredients.IsEnabled = false;
            this.cbShowCheckedRecipes.IsEnabled = false;
            this.cbTextSearchMode.IsEnabled = false;
            this.tbFilter.IsEnabled = false;
            this.tbMaxRate.IsEnabled = false;
            this.tbMinRate.IsEnabled = false;
            this.bSearch.IsEnabled = false;

        }

        #region "Initialize view"
        private void cbDisciplines_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Data.RecipeType _selectedType = null;
            if(this.cbRecipeTypes.Items.Count>0)
                _selectedType = this.GetSelectedvalue(this.cbRecipeTypes, Data.RecipeType.RecipeTypes);
            Data.Discipline discipline = this.GetSelectedvalue(this.cbDisciplines, Data.Discipline.Disciplines);
            LoadRecipeTypes(discipline);
            if (_selectedType != null)
                if (discipline==null || discipline.RecipeTypes.Values.Contains(_selectedType))
                {
                    foreach(object cbi in this.cbRecipeTypes.Items)
                        if (cbi.GetType() == typeof(ComboBoxItem))
                            if(((ComboBoxItem)cbi).Content.ToString() == Config.GetLocalizedName(_selectedType))
                            {
                                this.cbRecipeTypes.SelectedItem = cbi;
                                break;
                            }
                }
        }
        private void LoadDisciplines()
        {
            cbDisciplines.Items.Add(Properties.Resources.ComboBoxDisciplineFirstEntry);
            foreach (KeyValuePair<string, Data.Discipline> pair in
                Data.Discipline.Disciplines.OrderBy<KeyValuePair<string, Data.Discipline>, string>(p => Config.GetLocalizedName(p.Value)))
            {
                Data.Discipline r = pair.Value;
                //ComboBoxItem cbi = new ComboBoxItem();
                //cbi.Content = Config.GetLocalizedName(r);
                DisciplineItem cbi = new DisciplineItem(r);
                this.cbDisciplines.Items.Add(cbi);
            }
            this.cbDisciplines.SelectedIndex = 0;
        }
        private void LoadLanguages()
        {
            foreach (Data.Language l in
                Data.Language.Languages.OrderBy(p => p.Label))
            {
                ComboBoxItem cbi = new ComboBoxItem();
                cbi.Content = l.Label;
                cbi.Tag = l;
                this.cblanguages.Items.Add(cbi);
            }
            this.cblanguages.SelectedIndex = Config.Language.Id;
        }
        private void LoadRarities()
        {
            cbRarities.Items.Add(Properties.Resources.ComboBoxRarityFirstEntry);
            foreach (KeyValuePair<string, Data.Rarity> pair in
                Data.Rarity.Rarities)
            {
                ComboBoxItem cbi = new ComboBoxItem();
                Data.Rarity r = pair.Value;
                System.Windows.Media.SolidColorBrush brush = new System.Windows.Media.SolidColorBrush();
                brush.Color = System.Windows.Media.Color.FromRgb(r.Color.R, r.Color.G, r.Color.B);
                cbi.BorderBrush = brush;
                cbi.Background = brush;
                cbi.Content = Config.GetLocalizedName(r);
                this.cbRarities.Items.Add(cbi);
            }
            this.cbRarities.SelectedIndex = 0;
        }
        private void LoadRecipeTypes(Data.Discipline discipline = null)
        {
            Action<KeyValuePair<string, Data.RecipeType>> addRecipeType = delegate(KeyValuePair<string, Data.RecipeType> pair)
            {
                System.Windows.Media.SolidColorBrush fontBrush = new System.Windows.Media.SolidColorBrush(Colors.Black);
                ComboBoxItem cbi = new ComboBoxItem();
                cbi.Foreground = fontBrush;
                Data.RecipeType ot = pair.Value;
                cbi.Content = Config.GetLocalizedName(ot);
                this.cbRecipeTypes.Items.Add(cbi);
            };

            cbRecipeTypes.Items.Clear();
            cbRecipeTypes.Items.Add(Properties.Resources.ComboBoxRecipeTypeFirstEntry);
            if (discipline != null)
            {
                foreach (KeyValuePair<string, Data.RecipeType> pair in
                    discipline.RecipeTypes.OrderBy<KeyValuePair<string, Data.RecipeType>, string>(p => Config.GetLocalizedName(p.Value)))
                {
                    addRecipeType(pair);
                }
            }
            else
            {
                foreach (KeyValuePair<string, Data.RecipeType> pair in
                    Data.RecipeType.RecipeTypes.OrderBy<KeyValuePair<string, Data.RecipeType>, string>(p => Config.GetLocalizedName(p.Value)))
                {
                    addRecipeType(pair);
                }
            }
            this.cbRecipeTypes.SelectedIndex = 0;
        }
        private void LoadTextSearchModes()
        {
            foreach (KeyValuePair<string, Data.TextSearchOperator> pair in
                Data.TextSearchOperator.TextSearchModes.OrderBy<KeyValuePair<string, Data.TextSearchOperator>, string>(p => Config.GetLocalizedName(p.Value)))
            {
                Data.TextSearchOperator r = pair.Value;
                ComboBoxItem cbi = new ComboBoxItem();
                cbi.Content = Config.GetLocalizedName(r);
                this.cbTextSearchMode.Items.Add(cbi);
            }
            this.cbTextSearchMode.SelectedIndex = 0;
        }
        #endregion

        private void Reload()
        {
            try
            {
                Window oldForm = App.Current.Windows[0];
                Type mainWinType = App.Current.Windows[0].GetType();
                Window mainForm = 
                    System.Reflection.Assembly.GetExecutingAssembly().CreateInstance(mainWinType.FullName) as Window;
                mainForm.Top = oldForm.Top;
                mainForm.Left = oldForm.Left;
                mainForm.Show();

                // close all windows and reload the startup form
                foreach (Window win in App.Current.Windows)
                {
                    if (mainForm == win) // skip our newly loaded form
                        continue;

                    win.Close();
                }
            }
            catch
            {
                throw;
            }
        }
        private Data.Language GetSelectedLanguage()
        {
            string value;
            object selectedValue = this.cblanguages.SelectedValue;
            if (selectedValue.GetType() == typeof(ComboBoxItem))
            {
                value = (string)((ComboBoxItem)selectedValue).Content;
                return Data.Language.Languages.Where((p) => { return p.Label == value; }).Single();
            }

            return null;
        }
        private void cblanguages_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Data.Language language = this.GetSelectedLanguage();
            string origCulture = System.Globalization.CultureInfo.CurrentUICulture.IetfLanguageTag;

            if (this.IsInitialized && language.Label.ToLower() != origCulture.ToLower())
            {
                ChangeLanguageMessage win = new ChangeLanguageMessage(this);
                win.ShowDialog();
                if (win.DialogResult.Equals(true))
                {
                    Config.Lang = language.Label.ToLower();
                    Config.Save();
                    App.SetCulture();
                    App.Restart();
                    return;
                }
                ComboBox combo = (ComboBox)sender;
                combo.SelectedItem = e.RemovedItems[0];
            }
        }
        private void Window_ContentRendered(object sender, EventArgs e)
        {
            try
            {
                this.tbCurrentTask.Text = Properties.Resources.TextBoxCurrentDownloadLoadingCachedRecipes;
                this.bSearch.IsEnabled = false;
                Mouse.OverrideCursor = Cursors.Wait;

                BackgroundWorker _worker = new BackgroundWorker();
                _worker.DoWork +=
                    (object worker, DoWorkEventArgs evt) =>
                    {
                        // init cache
                        Data.Cache.Create(Config.Lang);

                        Dispatcher.BeginInvoke(new Action(() =>
                        {
                            this.tbCurrentTask.Text = Properties.Resources.TextBoxCheckingRecipesInError;

                            // refresh UI
                            this.RefreshDownloadResultsCount();

                            Mouse.OverrideCursor = null;

                            List<Data.Recipe> _inError = Data.Cache.GetCache().GetRecipesInError();
                            if (_inError.Count > 0)
                            {
                                // ask to user if he wants to delete and redownload
                                DeleteRecipeInErrorMessage win = new DeleteRecipeInErrorMessage(this, _inError.Count);
                                win.ShowDialog();
                                if (win.DialogResult.Equals(true))
                                {
                                    // Delete
                                    Data.Cache.GetCache().DeleteRecipesInError();
                                    Data.Cache.GetCache().LoadRecipes();

                                    // re-get recipes in errors and export to a file errors.json
                                    _inError = Data.Cache.GetCache().GetRecipesInError();
                                }
                            }
                            if(_inError.Count > 0)
                                ErrorExportHelper.ExportErrorToJson(_inError,
                                    System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "errors.json"));

                            // check network availability
                            if (!IsInternetAvailable())
                            {
                                this.tbCurrentTask.Foreground = new System.Windows.Media.SolidColorBrush(Colors.Red);
                                this.tbCurrentTask.Text = Properties.Resources.TextBoxCurrentDownloadNetworkNotAvailableNotification;
                            }
                            else
                            {
                                // download new recipes
                                this.tbCurrentTask.Foreground = new System.Windows.Media.SolidColorBrush(Colors.White);
                                this.DownloadNewRecipes();
                            }

                            this.cbDisciplines.IsEnabled = true;
                            this.cbRarities.IsEnabled = true;
                            this.cbRecipeTypes.IsEnabled = true;
                            this.cbSearchAlsoIngredients.IsEnabled = true;
                            this.cbShowCheckedRecipes.IsEnabled = true;
                            this.cbTextSearchMode.IsEnabled = true;
                            this.tbFilter.IsEnabled = true;
                            this.tbMaxRate.IsEnabled = true;
                            this.tbMinRate.IsEnabled = true;
                            this.bSearch.IsEnabled = true;

                        }));

                    };

                _worker.RunWorkerAsync();
            }
            catch
            {
                throw;
            }
        }

        private void AddRecipeToView(Data.Recipe recipe)
        {
            RecipeItem ri = new RecipeItem(recipe, this);
            recipeItems.Add(ri);
        }
        
        private void RefreshView()
        {
            RefreshView(this);
        }
        private static void RefreshView(MainWindow win)
        {
            win.recipeView.Refresh();
            win.recipesContainer.UpdateLayout();
        }

        #region "Search for recipes"
        //private BackgroundWorker _searchWorker;
        private int? GetMaxRate()
        {
            try
            {
                if (String.IsNullOrEmpty(this.tbMaxRate.Text))
                    return null;
                else
                {
                    int maxTemp;
                    if (!int.TryParse(this.tbMaxRate.Text, out maxTemp))
                        throw new Exception(Properties.Resources.ErrorMessageMaxLevelMustBeAnIntValue);

                    return maxTemp;
                }
            }
            catch
            {
                throw;
            }
        }
        private int? GetMinRate()
        {
            try
            {
                if (String.IsNullOrEmpty(this.tbMinRate.Text))
                    return null;
                else
                {
                    int minTemp;
                    if (!int.TryParse(this.tbMinRate.Text, out minTemp))
                        throw new Exception(Properties.Resources.ErrorMessageMinLevelMustBeAnIntValue);

                    return minTemp;
                }
            }
            catch
            {
                throw;
            }
        }
        private T GetSelectedvalue<T>(ComboBox cb, Dictionary<string, T> datas) where T : Data.ILocalizable
        {
            string value;
            object selectedValue = cb.SelectedValue;
            if (selectedValue.GetType() == typeof(ComboBoxItem))
            {
                value = (string)((ComboBoxItem)selectedValue).Content;
                foreach (KeyValuePair<string, T> pair in datas)
                {
                    T r = pair.Value;
                    if (value == Config.GetLocalizedName(r))
                    {
                        return r;
                    }
                }
            }
            else if (selectedValue.GetType() == typeof(DisciplineItem))
            {
                Data.Discipline selection = ((DisciplineItem)selectedValue).Discipline;
                foreach (KeyValuePair<string, T> pair in datas)
                {
                    T r = pair.Value;
                    if (Config.GetLocalizedName(selection) == Config.GetLocalizedName(r))
                    {
                        return r;
                    }
                }
            }
            return default(T);
        }
        private bool CheckSearchParameters(Data.Discipline selectedDiscipline, Data.Rarity selectedRarity, Data.RecipeType selectedRecipeType, int? minRate, int?MaxRate, string _txtOnFilter)
        {
            if (selectedDiscipline == null
                && selectedRarity == null
                && selectedRecipeType == null
                && minRate == null
                && MaxRate == null
                && (String.IsNullOrEmpty(_txtOnFilter) || _txtOnFilter.Length < 2 ))
            {
                return false;
            }
            return true;
        }
        private void searchRecipes(SearchMode searchMode)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                string txtOnFilter = this.tbFilter.Text;
                txtOnFilter = StringHelper.RemoveDiacritics(txtOnFilter);

                Data.Discipline selectedDiscipline = this.GetSelectedvalue(this.cbDisciplines, Data.Discipline.Disciplines);
                Data.RecipeType selectedRecipeType = this.GetSelectedvalue(this.cbRecipeTypes, Data.RecipeType.RecipeTypes);
                Data.Rarity selectedRarity = this.GetSelectedvalue(this.cbRarities, Data.Rarity.Rarities);
                Data.TextSearchOperator searchOperator = this.GetSelectedvalue(this.cbTextSearchMode, Data.TextSearchOperator.TextSearchModes);
                bool searchAlsoIntoIngredients = (bool)this.cbSearchAlsoIngredients.IsChecked;
                bool showCheckedRecipes = (bool)this.cbShowCheckedRecipes.IsChecked;

                long currentSessionId = Data.Cache.SessionID;
                int takeNRows = 300;

                int? minRate = this.GetMinRate();
                int? maxRate = this.GetMaxRate();

                if (searchMode == SearchMode.Normal
                    && !CheckSearchParameters(selectedDiscipline, selectedRarity, selectedRecipeType, minRate, maxRate, txtOnFilter))
                {
                    this.tbResults.Foreground = new System.Windows.Media.SolidColorBrush(Colors.Red);
                    this.tbResults.Text = Properties.Resources.TextBoxSearchResultAlterIfNoParameter;
                    return;
                }
                else
                {
                    this.tbResults.Foreground = new System.Windows.Media.SolidColorBrush(Colors.White);
                }

                // hide welcome msg
                this.gWelcomeMsg.Visibility = System.Windows.Visibility.Collapsed;

                this.recipeItems.Clear();
                this.RefreshView();
                this.RefreshDownloadResultsCount();

                // show search indicator
                this.tbResults.Text = Properties.Resources.TextBoxSearchResultInProgress;

                BackgroundWorker worker = new BackgroundWorker();
                worker.WorkerReportsProgress = true;

                worker.DoWork +=
                    (object sender, DoWorkEventArgs e) =>
                    {
                        try
                        {
                            Func<string, string> convert = delegate(string s)
                            { return s.ToUpper(); };
                            Func<Data.Recipe, Data.Discipline, bool> hasDiscipline = delegate(Data.Recipe recipe, Data.Discipline discipline)
                            {
                                if (recipe.Disciplines.Count == 0)
                                    return false;
                                return recipe.Disciplines.Contains(discipline);
                            };

                            List<Data.Recipe> _temp = Data.Cache.GetCache().Recipes.ToList();


                            // if not params used to search, all recipes'll be shown with the checked one
                            if (!showCheckedRecipes)
                            {
                                _temp = _temp.Where((r) => {
                                    return !r.Is_checked; 
                                }).ToList();
                            }

                            if (searchMode == SearchMode.LatestDownload)
                            {
                                _temp = _temp.Where((r) => {
                                    return r.SessionID == currentSessionId; 
                                }).ToList();
                            }
                            else if(searchMode == SearchMode.RecipesInError)
                            {
                                _temp = Data.Cache.GetCache().GetRecipesInError();
                            }
                            else
                            {
                                // filter on recipe type
                                if (selectedRecipeType != null)
                                    _temp = _temp.Where((r) => {
                                        return r.RecipeType != null && r.RecipeType.Key == selectedRecipeType.Key;
                                    }).ToList();

                                // filter on rarity, search deeply through rarities of ingredients too
                                if (selectedRarity != null)
                                {
                                        // if mode is "text contains"
                                    if (searchAlsoIntoIngredients)
                                    {
                                        _temp = _temp.Where((r) => {
                                            return (r.Rarity != null && r.Rarity.Key == selectedRarity.Key)
                                                || (r.Ingredients != null && r.Ingredients.Where((p) => {
                                                    return p.Rarity != null && p.Rarity.Key == selectedRarity.Key; 
                                                }).Count() > 0);
                                        }).ToList();
                                    }
                                    else
                                    {
                                        _temp = _temp.Where((r) => {
                                            return r.Rarity != null && r.Rarity.Key == selectedRarity.Key;
                                        }).ToList();
                                    }
                                }

                                // filter on discipline
                                if (selectedDiscipline != null)
                                    _temp = _temp.Where((r) => {
                                            return r.Disciplines.Contains(selectedDiscipline);
                                        }).ToList();

                                // filter on minRate
                                if (minRate != null)
                                    _temp = _temp.Where((r) => {
                                        return r.MinRate != null && int.Parse(r.MinRate) >= minRate;
                                    }).ToList();

                                // filter on maxRate
                                if (maxRate != null)
                                    _temp = _temp.Where((r) => {
                                        return r.MinRate !=null && int.Parse(r.MinRate) <= maxRate;
                                    }).ToList();

                                // filter on keyword
                                if (!String.IsNullOrEmpty(txtOnFilter))
                                {
                                    if (searchOperator.Mode == Data.TextSearchOperator.Modes.Contains)
                                    {
                                        // if mode is "text contains"
                                        if (searchAlsoIntoIngredients)
                                        {
                                            _temp = _temp.Where((r) => {
                                                return (r.ContainsText(txtOnFilter) 
                                                    || r.IngredientContainsText(txtOnFilter));
                                            }).ToList();
                                        }
                                        else
                                        {
                                            _temp = _temp.Where((r) => {
                                                return r.ContainsText(txtOnFilter);
                                            }).ToList();
                                        }
                                    }
                                    else
                                    {
                                        // if mode is "text equals"
                                        if (searchAlsoIntoIngredients)
                                        {
                                            _temp = _temp.Where((r) => {
                                                return (r.EqualsText(txtOnFilter)
                                                    || r.IngredientEqualsText(txtOnFilter));
                                            }).ToList();
                                        }
                                        else
                                        {
                                            _temp = _temp.Where((r) => {
                                                return r.EqualsText(txtOnFilter);
                                            }).ToList();
                                        }
                                    }
                                }

                            }

                            if (_temp.Count() > takeNRows)
                                _temp = _temp.Take(takeNRows).ToList();

                            for (int i = 0; i <= _temp.Count() - 1; i++)
                                worker.ReportProgress(i, new SearchResultSate() { Current = i + 1, Total = _temp.Count(), Recipe = _temp[i] });

                        }
#if DEBUG
                        catch (Exception ex)
                        {
                            
                            MessageBox.Show(ex.ToString());
                        }
#else
                        catch
                        {
                            // Ignore exceptions at this point
                        }
#endif
                    };
                worker.ProgressChanged +=
                    (object sender, ProgressChangedEventArgs e) =>
                    {
                        SearchResultSate result = (SearchResultSate)e.UserState;
                        AddRecipeToView(result.Recipe);
                    };
                worker.RunWorkerCompleted +=
                    (object sender, RunWorkerCompletedEventArgs e) =>
                    {
                        this.RefreshView();
                        this.RefreshSearchResultsCount();
                        this.svScroller.ScrollToTop();
                    };
                worker.RunWorkerAsync();
            }
            catch(Exception ex)
            {
                this.tbResults.Foreground = new SolidColorBrush(Colors.Red);
                this.tbResults.Text = ex.Message;
            }
            finally
            {
                Mouse.OverrideCursor = null;
            }
        }
        //private void bReinitialize_Click(object sender, RoutedEventArgs e)
        //{
        //    this.cbDisciplines.SelectedIndex = 0;
        //    this.cbRarities.SelectedIndex = 0;
        //    this.cbRecipeTypes.SelectedIndex = 0;
        //    this.cbSearchAlsoIngredients.IsChecked = false;
        //    this.cbShowCheckedRecipes.IsChecked = false;
        //    this.tbFilter.Clear();
        //}
        private void bSearch_Click(object sender, EventArgs e)
        {
            string txtOnFilter = this.tbFilter.Text;
            txtOnFilter = StringHelper.RemoveDiacritics(txtOnFilter);

            this.searchRecipes(SearchMode.Normal);
        }
        private void onEnterKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                this.bSearch_Click(null, null);
            }
        }
        private void RefreshSearchResultsCount()
        {
            RefreshSearchResultsCount(this);
        }
        private static void RefreshSearchResultsCount(MainWindow win)
        {
            int foundCount = win.recipeItems.Where(p => p.Visibility == System.Windows.Visibility.Visible).Count();
            int checkedOnesCount = Data.Cache.GetCache().Recipes.Where((p) => { return p.Is_checked; }).Count();
            win.tbResults.Text = String.Format(GW2ExplorerCraftTool.Properties.Resources.TextBoxSearchResult, foundCount, checkedOnesCount);
            win.tbResults.Foreground = new System.Windows.Media.SolidColorBrush(Colors.White);
        }
        public void SearchByRecipeName(string recipeName)
        {
            this.cbSearchAlsoIngredients.IsChecked = true;
            this.tbFilter.Text = recipeName;
            this.searchRecipes(SearchMode.Normal);
        }
        public void SearchByIngredientName(string recipeName)
        {
            this.cbSearchAlsoIngredients.IsChecked = true;
            this.tbFilter.Text = recipeName;
            this.searchRecipes(SearchMode.Normal);
        }
        private void ShowMeLatestDownload(object sender, RequestNavigateEventArgs e)
        {
            this.searchRecipes(SearchMode.LatestDownload);
        }
        private void ShowMeRecipesInError(object sender, RequestNavigateEventArgs e)
        {
            this.searchRecipes(SearchMode.RecipesInError);
        }
        #endregion

        #region "Download new recipes"
        private void DownloadNewRecipes()
        {
            BackgroundWorker downloadWorker = new BackgroundWorker();
            downloadWorker.WorkerReportsProgress = true;

            List<int> recipe_ids = null;
            MainWindow win = this;
            downloadWorker.DoWork +=
                    (object sender, DoWorkEventArgs e) =>
                    {
                        try
                        {
                            Dispatcher.BeginInvoke(new Action(() =>
                            {
                                this.tbCurrentTask.Text = Properties.Resources.TextBoxCurrentDownloadStarting;
                            }));

                            recipe_ids = Data.DataProvider.GetRecipeIDs();

                            // get only not cached recipes
                            List<int> cached_recipe_ids = Data.Cache.GetCache().Recipes.Select<Data.Recipe, int>(p => (int)p.Id).ToList();
                            recipe_ids = recipe_ids.Where(id => !cached_recipe_ids.Contains(id)).ToList();

                            Dispatcher.BeginInvoke(new Action(() => {
                                this.tbCurrentTask.Text = 
                                    String.Format(Properties.Resources.TextBoxCurrentDownloadNewRecipesFoundNotification, recipe_ids.Count);
                                this.pbDownloadIndicator.Minimum = 0;
                                this.pbDownloadIndicator.Maximum = recipe_ids.Count;
                            }));

                            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
                            for (int i = 0; i <= recipe_ids.Count - 1; i++)
                            {
                                if (e.Cancel)
                                    break;

                                int recipe_id = recipe_ids[i];

                                try
                                {
                                    sw.Start();
                                    Data.API.ANet.Recipe recipe = Data.DataProvider.GetANetRecipe(recipe_id, Config.Lang);
                                    if (recipe == null)
                                        throw new System.Net.WebException(String.Format("Something goes wrong with recipe {0}!", recipe_id));

                                    sw.Stop();
                                    DownloadState.AddNewTimeElapsed(sw.ElapsedMilliseconds);
                                    downloadWorker.ReportProgress(i + 1, new DownloadState() { Current = i + 1, Total = recipe_ids.Count, Recipe = recipe });
                                }
                                finally
                                {
                                    sw.Reset();
                                }
                            }
                        }
                        catch(Exception)
                        {
                            // Ignore exceptions at this point - failed to take from queue
                        }
                    };
            downloadWorker.ProgressChanged +=
                    (object sender, ProgressChangedEventArgs e) =>
                    {
                        DownloadState state = (DownloadState)e.UserState;
                        TimeSpan t = TimeSpan.FromMilliseconds(DownloadState.AverageMilliseconds * (state.Total - state.Current));
                        this.pbDownloadIndicator.Value = state.Current;
                        this.tbCurrentTask.Text =
                            String.Format(
                            Properties.Resources.TextBoxCurrentDownloadInProgress, 
                            state.Current, 
                            state.Total, 
                            state.Percent,
                            t.Minutes,
                            t.Seconds);
                        // cache new recipe
                        Data.Recipe recipe = Data.Cache.GetCache().AddRecipe(state.Recipe);
                        // add to view
                        this.RefreshDownloadResultsCount();
                    };
            downloadWorker.RunWorkerCompleted +=
                   (object sender, RunWorkerCompletedEventArgs e) =>
                   {
                       if (e.Error != null)
                       {
                           this.tbCurrentTask.Text =
                               String.Format(Properties.Resources.TextBoxCurrentDownloadSomethingGoesWrong);
                       }
                       else
                       {
                           if (recipe_ids == null)
                           {
                               this.tbCurrentTask.Text = Properties.Resources.SomethingIsWrongNoDownloadPossible;
                           }
                           else
                           {
                               if (recipe_ids != null && recipe_ids.Count == 0)
                               {
                                   this.tbCurrentTask.Text = Properties.Resources.TextBoxCurrentDownloadNoNewRecipesFoundNotification;
                               }
                               else
                               {
                                   this.tbCurrentTask.Text = Properties.Resources.TextBoxCurrentDownloadFinished;
                               }
                           }
                       }
                       this.pbDownloadIndicator.Visibility = System.Windows.Visibility.Collapsed;
                   };
            downloadWorker.RunWorkerAsync();

        }
        private void RefreshDownloadResultsCount()
        {
            RefreshDownloadResultsCount(this);
        }
        private static void RefreshDownloadResultsCount(MainWindow win)
        {
            int downloadedCount = Data.Cache.GetCache().Recipes.Count();
            win.tbCachedRecipes.Text = String.Format(Properties.Resources.TextBoxCachedRecipes, downloadedCount);
            win.tbCachedRecipes.Visibility = Visibility.Visible;
        }
        #endregion

        #region "Welcome message"
        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }
        #endregion

        #region "Checking Internet connection"
        [DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int description, int reservedValue);

        public static bool IsInternetAvailable()
        {
            int description;
            return InternetGetConnectedState(out description, 0);
        }
        #endregion

    }

    class DownloadState
    {
        public double Percent
        {
            get
            {
                return (double)Current / (double)Total;
            }
        }
        public int Current { get; set; }
        public int Total { get; set; }

        internal static List<long> _milliseconds = new List<long>();
        public static long AverageMilliseconds
        { 
            get 
            {
                return _milliseconds.Sum() / _milliseconds.Count;
            }
        }
        public GW2ExplorerCraftTool.Data.API.ANet.Recipe Recipe { get; set; }

        public static void AddNewTimeElapsed(long ms)
        {
            _milliseconds.Add(ms);
        }

    }
    class SearchResultSate
    {
        public int Current { get; set; }
        public int Total { get; set; }
        public GW2ExplorerCraftTool.Data.Recipe Recipe { get; set; }
    }
    class ErrorBot<T>
    {
        MainWindow _parentWindow;
        private List<T> _items;

        public List<T> FoundItems
        {
            get
            {
                return this._items;
            }
        }

        public ErrorBot(MainWindow parentWindow, List<T> source, Func<T, bool> predicate, bool async)
        {
            this._parentWindow = parentWindow;

            if (async)
            {
                // start async worker
                BackgroundWorker worker = new BackgroundWorker();

                worker.DoWork +=
                    (object sender, DoWorkEventArgs e) =>
                    {
                        try
                        {
                            _items = source.Where(predicate).ToList();

                        }
#if DEBUG
                        catch (Exception ex)
                        {
                            
                            MessageBox.Show(ex.Message);
                        }
#else
                        catch
                        {
                            // Ignore exceptions at this point
                        }
#endif
                    };
                worker.RunWorkerCompleted +=
                    (object sender, RunWorkerCompletedEventArgs e) =>
                    {
                        if (_items.Count > 0)
                            MessageBox.Show(
                                _parentWindow,
                                "Warning",
                                String.Format("Some items in error has been detected.\n{0} items found.", _items.Count));
                    };
                worker.RunWorkerAsync();
            }
            else
            {
                _items = source.Where(predicate).ToList();
            }
        }

    }
    class SearchParameters
    {
        public string  FilterField{ get; set; }
        public Data.Discipline DisciplineField{ get; set; }
        public Data.Rarity RarityField { get; set; }
        public Data.RecipeType RecipeTypeField { get; set; }
        public Data.TextSearchOperator SearchOperatorField{ get; set; }
        public SearchMode SearchMode { get; set; }
        public bool SearchAlsoIntoIngredientsField { get; set; }
        public bool ShowCheckedRecipesField{ get; set; }
    }
    class SearchParametersManager
    {
        private TextBox _FilterField;
        private ComboBox _DisciplineField;
        private ComboBox _RecipeTypeField;
        private ComboBox _RarityField;
        private ComboBox _SearchOperatorField;
        private CheckBox _SearchAlsoIntoIngredientsField;
        private CheckBox _ShowCheckedRecipesField;

        private List<SearchParameters> _parametersHistoric;
        private int _currentIndex;
        public SearchParameters Next
        {
            get
            {
                _currentIndex = Math.Min(_parametersHistoric.Count(), _currentIndex + 1);
                return _parametersHistoric[_currentIndex];
            }
        }
        public SearchParameters Previous
        {
            get
            {
                _currentIndex = Math.Max(0, _currentIndex - 1);
                return _parametersHistoric[_currentIndex];
            }
        }
        public SearchParameters Current
        {
            get
            {
                return _parametersHistoric[_currentIndex];
            }
        }
        public SearchParametersManager(
            TextBox FilterField,
            ComboBox DisciplineField,
            ComboBox RecipeTypeField,
            ComboBox RarityField,
            ComboBox SearchOperatorField,
            CheckBox SearchAlsoIntoIngredientsField,
            CheckBox ShowCheckedRecipesField)
        {
            this._DisciplineField = DisciplineField;
            this._FilterField = FilterField;
            this._RarityField = RarityField;
            this._RecipeTypeField = RecipeTypeField;
            this._SearchOperatorField = SearchOperatorField;
            this._SearchAlsoIntoIngredientsField = SearchAlsoIntoIngredientsField;
            this._ShowCheckedRecipesField = ShowCheckedRecipesField;
            
            this._currentIndex = -1;
            this._parametersHistoric = new List<SearchParameters>();
        }
    }
}