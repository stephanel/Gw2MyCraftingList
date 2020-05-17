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
    /// Logique d'interaction pour ChangeLanguageMessage.xaml
    /// </summary>
    public partial class ChangeLanguageMessage : Window
    {
        public ChangeLanguageMessage(MainWindow owner)
        {
            InitializeComponent();
            this.Owner = owner;
        }
        private void bYes_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }
        private void bNo_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
