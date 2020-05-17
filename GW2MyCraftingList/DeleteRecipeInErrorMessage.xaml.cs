using System;
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
using System.Windows.Shapes;

namespace GW2ExplorerCraftTool
{
    /// <summary>
    /// Logique d'interaction pour DeleteRecipeInErrorWindow.xaml
    /// </summary>
    public partial class DeleteRecipeInErrorMessage : Window
    {
        public DeleteRecipeInErrorMessage(MainWindow owner, int recipesCount)
        {
            InitializeComponent();
            this.Owner = owner;
            this.tbRecipesCountInError.Text = String.Format("{0} {1}", recipesCount, Properties.Resources.DeleteRecipeInErrorMessageContentRecipesCount);
        }
        private void bDelete_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }
        private void bNotDelete_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
