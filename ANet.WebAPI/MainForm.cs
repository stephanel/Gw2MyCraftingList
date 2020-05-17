using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ANet.WebAPI
{
    public partial class MainForm : Form
    {

        public MainForm()
        {
            InitializeComponent();

            this.tbFileSignature.Text = Config.FileSignature;
            this.tbFileID.Text = Config.FileID;
            this.cbFileFormat.SelectedItem = Config.FileFormat;

        }

        #region "Background worker"
        private void Process(Func<string> onWork, TextBox textBox)
        {
            this.Cursor = Cursors.WaitCursor;

            BackgroundWorker _searchWorker = new BackgroundWorker();

            _searchWorker.DoWork +=
                (object sender, DoWorkEventArgs e) =>
                {
                    try
                    {
                        e.Result = onWork.Invoke();

                    }
                    catch (Exception ex)
                    {
                        // Ignore exceptions at this point - failed to take from queue
                        e.Result = ex;
                    }
                };
            _searchWorker.RunWorkerCompleted +=
                (object sender, RunWorkerCompletedEventArgs e) =>
                {
                    object r = e.Result;
                    if (r != null)
                    {
                        if (r.GetType() == typeof(string))
                        {
                            textBox.Text = (string)r;
                        }
                        else
                        {
                            textBox.Text = ((Exception)r).Message;
                        }
                    }
                    this.Cursor = Cursors.Default;

                };
            _searchWorker.RunWorkerAsync();
        }
        private void Process(Func<object> onWork, Action<object> onFinish)
        {
            this.Cursor = Cursors.WaitCursor;

            BackgroundWorker _searchWorker = new BackgroundWorker();

            _searchWorker.DoWork +=
                (object sender, DoWorkEventArgs e) =>
                {
                    try
                    {
                        e.Result = onWork.Invoke();

                    }
                    catch (Exception ex)
                    {
                        // Ignore exceptions at this point - failed to take from queue
                        e.Result = ex;
                    }
                };
            _searchWorker.RunWorkerCompleted +=
                (object sender, RunWorkerCompletedEventArgs e) =>
                {
                    object r = e.Result;
                    onFinish.Invoke(r);
                    this.Cursor = Cursors.Default;

                };
            _searchWorker.RunWorkerAsync();
        }
        #endregion

        #region "Events"
        private void bGetEventNames_Click(object sender, EventArgs e)
        {
            this.Process(
                () =>
                {
                    return ANet.WebAPI.Data.DataProvider.GetEventNames(Config.Language);
                },
                this.tbGetEventNamesResult);
        }
        private void bGetEvents_Click(object sender, EventArgs e)
        {
            try
            {
                string txt = this.tbWorldId.Text;
                if (String.IsNullOrEmpty(txt))
                    throw new NullReferenceException("World ID cannot be a null value !");

                int world_id;
                int.TryParse(txt, out world_id);
                if (world_id == 0)
                    throw new NullReferenceException("World ID need to be an integer value !");

                this.Process(
                    () =>
                    {
                        return ANet.WebAPI.Data.DataProvider.GetEvents(Config.Language, world_id);
                    },
                    this.tbGetEventsResult);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region "Maps"
        private void bGetMapNames_Click(object sender, EventArgs e)
        {
            this.Process(
                () =>
                {
                    return ANet.WebAPI.Data.DataProvider.GetMapNames(Config.Language);
                },
                this.tbGetMapNamesResult);
        }
        #endregion

        #region "Worlds"
        private void bGetWorldNames_Click(object sender, EventArgs e)
        {
            this.Process(
                () =>
                {
                    return ANet.WebAPI.Data.DataProvider.GetWorldNames(Config.Language);
                },
                this.tbGetWorldNamesResult);
        }
        #endregion

        #region "WvW"
        private void bGetWvwMatches_Click(object sender, EventArgs e)
        {
            this.Process(
                () =>
                {
                    return ANet.WebAPI.Data.DataProvider.GetWvwMatches();
                },
                this.tbGetWvwMatchesResult);
        }
        private void bGetWvwMatchDetails_Click(object sender, EventArgs e)
        {
            try
            {
                string match_id = this.tbMatchId.Text;
                if (String.IsNullOrEmpty(match_id))
                    throw new NullReferenceException("Match ID cannot be a null value !");

                this.Process(
                    () =>
                    {
                        return ANet.WebAPI.Data.DataProvider.GetWvwMatcheDetails(Config.Language, match_id);
                    },
                    this.tbGetWvwMatchDetailsResult);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void bGetWvwObjectiveNames_Click(object sender, EventArgs e)
        {
            this.Process(
                () =>
                {
                    return ANet.WebAPI.Data.DataProvider.GetWvwObjectiveNames(Config.Language);
                },
                this.tbGetWvwObjectiveNamesResult);
        }
        #endregion

        #region "Colors"
        private void bGetColors_Click(object sender, EventArgs e)
        {
            this.Process(
                () =>
                {
                    return ANet.WebAPI.Data.DataProvider.GetColors(Config.Language);
                },
                this.tbGetColorsResult);
        }
        #endregion

        #region "Items"
        private void bGetItemDetails_Click(object sender, EventArgs e)
        {
            string txt = this.tbItemId.Text;
            if (String.IsNullOrEmpty(txt))
                throw new NullReferenceException("Item ID cannot be a null value !");

            int item_id;
            int.TryParse(txt, out item_id);
            if (item_id == 0)
                throw new NullReferenceException("Item ID need to be an integer value !");

            this.Process(
                () =>
                {
                    return ANet.WebAPI.Data.DataProvider.GetItemDetails(Config.Language, item_id);
                },
                this.tbGetItemDetailsResult);
        }
        private void bGetItems_Click(object sender, EventArgs e)
        {
            this.Process(
                () =>
                {
                    return ANet.WebAPI.Data.DataProvider.GetItems();
                },
                this.tbGetItemsResult);
        }
        #endregion

        #region "Recipes"
        private void bGetRecipes_Click(object sender, EventArgs e)
        {
            this.Process(
                () =>
                {
                    return ANet.WebAPI.Data.DataProvider.GetRecipes();
                },
                this.tbGetRecipesResult);
        }
        private void bGetRecipeDetails_Click(object sender, EventArgs e)
        {
            string txt = this.tbRecipeID.Text;
            if (String.IsNullOrEmpty(txt))
                throw new NullReferenceException("Item ID cannot be a null value !");

            int recipe_id;
            int.TryParse(txt, out recipe_id);
            if (recipe_id == 0)
                throw new NullReferenceException("Recipe ID need to be an integer value !");

            this.Process(
                () =>
                {
                    return ANet.WebAPI.Data.DataProvider.GetRecipeDetails(Config.Language, recipe_id);
                },
                this.tbGetRecipeDetailsResult);
        }
        #endregion

        #region "Render Service"
        private void bGetFile_Click(object sender, EventArgs e)
        {
            string fileSignature = this.tbFileSignature.Text;
            string fileID = this.tbFileID.Text;
            string fileFormat = this.cbFileFormat.SelectedItem.ToString();

            if (String.IsNullOrEmpty(fileSignature))
                throw new NullReferenceException("File signature cannot be a null value !");

            if (String.IsNullOrEmpty(fileID))
                throw new NullReferenceException("File ID cannot be a null value !");

            if (String.IsNullOrEmpty(fileFormat))
                throw new NullReferenceException("File format cannot be a null value !");

            int file_id;
            int.TryParse(fileID, out file_id);
            if (file_id == 0)
                throw new NullReferenceException("File ID need to be an integer value !");

            this.Process(
                () =>
                {
                    return ANet.WebAPI.Data.DataProvider.GetFile(fileSignature, file_id, fileFormat.ToLower());
                },
                (p) => 
                {
                    if (p.GetType() == typeof(System.Net.WebException))
                    {
                        this.tbAssetFileLog.Text = ((Exception)p).Message;
                    }
                    else
                    {
                        using (System.IO.MemoryStream ms = new System.IO.MemoryStream((byte[])p))
                        {
                            Image img = Image.FromStream(ms);
                            this.pbAssetFile.Image = img;
                        }
                    }
                });
        }
        #endregion

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Config.FileSignature=this.tbFileSignature.Text;
            Config.FileID=this.tbFileID.Text;
            object format =this.cbFileFormat.SelectedItem;
            if(format != null)
                Config.FileFormat=String.Format("{0}", format);
            Config.Save();
        }

    }
}
