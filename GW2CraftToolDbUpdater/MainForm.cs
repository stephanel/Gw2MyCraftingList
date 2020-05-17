using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GW2CraftToolDbUpdater
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }
        private void Form1_Shown(object sender, EventArgs e)
        {
            TextBoxStreamWriter writer = new TextBoxStreamWriter(this.tbLog);
            Console.SetOut(writer);
    
            Console.WriteLine("Ready to update your database!");
            Console.Write("1) Pick your old database" + Environment.NewLine);
            Console.Write("2) Pick your new database" + Environment.NewLine);
            Console.Write("3) Click on \"Update\" button" + Environment.NewLine);
        }

        private void bUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                this.bUpdate.Enabled = false;

                string db1 = this.tbOldDatabaseFilePath.Text;
                string db2 = this.tbNewDatabaseFilePath.Text;

                if (string.IsNullOrEmpty(db1))
                    throw new NullReferenceException("Please, pick a path for the old database!");
                if (string.IsNullOrEmpty(db2))
                    throw new NullReferenceException("Please, pick a path for the new database!");

                BackgroundWorker updateWorker = new BackgroundWorker();
                updateWorker.DoWork +=
                     (object _sender, DoWorkEventArgs _e) =>
                     {
                         try
                         {
                             Console.WriteLine("Starting to update the new database from the old one...");
                             Console.WriteLine(String.Format("From (old one): {0}", db1));
                             Console.WriteLine(String.Format("To (new one): {0}", db2));
                             Console.WriteLine("Update in progress. Please wait....");

                             List<DbLine> lines = Data.SQLite.GetAllCheckedLines(db1);
                             foreach (DbLine line in lines)
                             {
                                 Data.SQLite.UpdateLine(db2, line.Id);
                             }


                         }
                         catch (Exception ex)
                         {
                             // Ignore exceptions at this point - failed to take from queue
                             _e.Result = ex;
                         }
                     };
                updateWorker.RunWorkerCompleted +=
                    (object _sender, RunWorkerCompletedEventArgs _e) =>
                    {
                        object r = _e.Result;
                        
                        if (r!=null && r.GetType() == typeof(Exception))
                        {
                            MessageBox.Show(this, ((Exception)r).ToString());
                        }
                        else
                        {
                            Console.WriteLine("Updating the new database finished with success!");
                        }

                        this.bUpdate.Enabled = true;
                        this.Cursor = Cursors.Default;

                    };
                updateWorker.RunWorkerAsync();
            }
            catch (NullReferenceException ex)
            {
                MessageBox.Show(this, ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString());
            }
            finally
            {
                this.bUpdate.Enabled = true;
            }
        }

        private string GetFilePath()
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.InitialDirectory = Application.StartupPath;
                if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    return ofd.FileName;
                }
                return null;
            }
        }
        private void bPickOldDataBase_Click(object sender, EventArgs e)
        {
            string filePath = this.GetFilePath();
            this.tbOldDatabaseFilePath.Text = (!String.IsNullOrEmpty(filePath) ? filePath : this.tbOldDatabaseFilePath.Text);
        }

        private void bPickNewDatabase_Click(object sender, EventArgs e)
        {
            string filePath = this.GetFilePath();
            this.tbNewDatabaseFilePath.Text = (!String.IsNullOrEmpty(filePath) ? filePath : this.tbNewDatabaseFilePath.Text);
        }

    }
}
