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
    /// Logique d'interaction pour ItemPrice.xaml
    /// </summary>
    public partial class TradingPostDataItem : UserControl
    {
        private const string THOUSAND_SEPARATOR_FORMAT = "0,0";
        private MainWindow _parentWindow;
        private bool _wrongState = false;

        public bool WrongState
        {
            get { return _wrongState; }
            set { _wrongState = value; }
        }

        public TradingPostDataItem(Data.TradingPostData tradingPostData, MainWindow parentWindow)
        {
            InitializeComponent();

            this._parentWindow = parentWindow;

            if (tradingPostData.Exception != null)
            {
                this.gTradingPostData.Visibility = System.Windows.Visibility.Collapsed;
                this.tbNoDataFound.Visibility = System.Windows.Visibility.Visible;

            }
            else
            {
                if (tradingPostData == null)
                {
                    this._wrongState = true;
                    return;
                }

                if (tradingPostData.HavingMaxBuyCopperPart)
                {
                    this.tbMaxBuyCopperPart.Text = tradingPostData.MaxBuyCopperPart.ToString();
                    this.iMaxBuyCopperIcon.Source = new BitmapImage(new Uri(Config.COPPER_ICON_URL));
                    this.iMaxBuyCopperIcon.Visibility = System.Windows.Visibility.Visible;
                }
                else
                {
                    this.tbMaxBuyCopperPart.Visibility = System.Windows.Visibility.Collapsed;
                    this.iMaxBuyCopperIcon.Visibility = System.Windows.Visibility.Collapsed;
                }
                if (tradingPostData.HavingMaxBuySilverPart)
                {
                    this.tbMaxBuySilverPart.Text = tradingPostData.MaxBuySilverPart.ToString();
                    this.iMaxBuySilverIcon.Source = new BitmapImage(new Uri(Config.SILVER_ICON_URL));
                    this.iMaxBuySilverIcon.Visibility = System.Windows.Visibility.Visible;
                }
                else
                {
                    this.tbMaxBuySilverPart.Visibility = System.Windows.Visibility.Collapsed;
                    this.iMaxBuySilverIcon.Visibility = System.Windows.Visibility.Collapsed;
                }
                if (tradingPostData.HavingMaxBuyGoldPart)
                {
                    this.tbMaxBuyGoldPart.Text = tradingPostData.MaxBuyGoldPart.ToString();
                    this.iMaxBuyGoldIcon.Source = new BitmapImage(new Uri(Config.GOLD_ICON_URL));
                    this.iMaxBuyGoldIcon.Visibility = System.Windows.Visibility.Visible;
                }
                else
                {
                    this.tbMaxBuyGoldPart.Visibility = System.Windows.Visibility.Collapsed;
                    this.iMaxBuyGoldIcon.Visibility = System.Windows.Visibility.Collapsed;
                }

                if (tradingPostData.HavingMinSellCopperPart)
                {
                    this.tbMinSellCopperPart.Text = tradingPostData.MinSellCopperPart.ToString();
                    this.iMinSellCopperIcon.Source = new BitmapImage(new Uri(Config.COPPER_ICON_URL));
                    this.iMinSellCopperIcon.Visibility = System.Windows.Visibility.Visible;
                }
                else
                {
                    this.tbMinSellCopperPart.Visibility = System.Windows.Visibility.Collapsed;
                    this.iMinSellCopperIcon.Visibility = System.Windows.Visibility.Collapsed;
                }
                if (tradingPostData.HavingMinSellSilverPart)
                {
                    this.tbMinSellSilverPart.Text = tradingPostData.MinSellSilverPart.ToString();
                    this.iMinSellSilverIcon.Source = new BitmapImage(new Uri(Config.SILVER_ICON_URL));
                    this.iMinSellSilverIcon.Visibility = System.Windows.Visibility.Visible;
                }
                else
                {
                    this.tbMinSellSilverPart.Visibility = System.Windows.Visibility.Collapsed;
                    this.iMinSellSilverIcon.Visibility = System.Windows.Visibility.Collapsed;
                }
                if (tradingPostData.HavingMinSellGoldPart)
                {
                    this.tbMinSellGoldPart.Text = tradingPostData.MinSellGoldPart.ToString();
                    this.iMinSellGoldIcon.Source = new BitmapImage(new Uri(Config.GOLD_ICON_URL));
                    this.iMinSellGoldIcon.Visibility = System.Windows.Visibility.Visible;
                }
                else
                {
                    this.tbMinSellGoldPart.Visibility = System.Windows.Visibility.Collapsed;
                    this.iMinSellGoldIcon.Visibility = System.Windows.Visibility.Collapsed;
                }

                this.tbOffer.Text = tradingPostData.BuyCount.ToString(THOUSAND_SEPARATOR_FORMAT);
                this.tbSale.Text = tradingPostData.SellCount.ToString(THOUSAND_SEPARATOR_FORMAT);

                this.gTradingPostData.Visibility = System.Windows.Visibility.Visible;
                this.tbNoDataFound.Visibility = System.Windows.Visibility.Collapsed;

            }

        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }
    }
}
