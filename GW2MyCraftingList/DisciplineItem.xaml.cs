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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GW2ExplorerCraftTool
{
    /// <summary>
    /// Logique d'interaction pour DisciplineItem.xaml
    /// </summary>
    public partial class DisciplineItem : UserControl
    {
        private Data.Discipline _discipline;

        public Data.Discipline Discipline
        {
            get { return _discipline; }
        }

        public DisciplineItem(Data.Discipline discipline)
        {
            InitializeComponent();
            this._discipline = discipline;

            this.tbDisciplineName.Text = Config.GetLocalizedName(discipline);

            if (!String.IsNullOrEmpty(discipline.Path))
            {
                BitmapImage bi = new BitmapImage(new Uri(discipline.Path));
                this.iDisciplineIcon.Source = bi;
            }
        }
    }
}
