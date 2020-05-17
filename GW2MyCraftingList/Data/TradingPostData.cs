using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GW2ExplorerCraftTool.Data
{
    public class TradingPostData
    {
        private Exception _Exception;

        public Exception Exception
        {
            get { return _Exception; }
        }

        private int _ItemId;
        public int ItemId
        {
            get { return _ItemId; }
        }

        private int _buyCount;
        public int BuyCount
        {
            get { return _buyCount; }
        }

        private int _sellCount;
        public int SellCount
        {
            get { return _sellCount; }
        }

        private int _maxBuyGoldPart;
        public int MaxBuyGoldPart
        {
            get { return _maxBuyGoldPart; }
        }

        private int _maxBuySilverPart;
        public int MaxBuySilverPart
        {
            get { return _maxBuySilverPart; }
        }

        private int _maxBuyCopperPart;
        public int MaxBuyCopperPart
        {
            get { return _maxBuyCopperPart; }
        }

        private int _minSellCopperPart;
        public int MinSellCopperPart
        {
            get { return _minSellCopperPart; }
        }

        private int _minSellSilverPart;
        public int MinSellSilverPart
        {
            get { return _minSellSilverPart; }
        }

        private int _minSellGoldPart;
        public int MinSellGoldPart
        {
            get { return _minSellGoldPart; }
        }


        private bool _havingMaxBuyCopperPart = false;
        public bool HavingMaxBuyCopperPart
        {
            get { return _havingMaxBuyCopperPart; }
        }

        private bool _havingMaxBuyGoldPart = false;
        public bool HavingMaxBuyGoldPart
        {
            get { return _havingMaxBuyGoldPart; }
        }
        
        private bool _havingMaxBuySilverPart = false;
        public bool HavingMaxBuySilverPart
        {
            get { return _havingMaxBuySilverPart; }
        }

        private bool _havingMinSellCopperPart = false;
        public bool HavingMinSellCopperPart
        {
            get { return _havingMinSellCopperPart; }
        }

        private bool _havingMinSellGoldPart = false;
        public bool HavingMinSellGoldPart
        {
            get { return _havingMinSellGoldPart; }
            set { _havingMinSellGoldPart = value; }
        }

        private bool _havingMinSellSilverPart = false;
        public bool HavingMinSellSilverPart
        {
            get { return _havingMinSellSilverPart; }
        }

        //public TradingPostData(int itemId, string buyPrice, string sellPrice, string buy, string sale)
        public TradingPostData(int itemId, Data.API.Gw2Spidy.Item item)
        {
            this._ItemId = itemId;

            if (item == null)
            {
                this._Exception = new NullReferenceException("Trading post data not found");
                return;
            }

            if (item.exception != null)
            {
                this._Exception = item.exception;
                return;
            }

            API.Gw2Spidy.ItemResult result = item.result;

            this._maxBuyCopperPart = int.Parse(result.max_offer_unit_price) % 100;
            this._maxBuySilverPart = (int.Parse(result.max_offer_unit_price) % 10000 - this._maxBuyCopperPart) / 100;
            this._maxBuyGoldPart = int.Parse(result.max_offer_unit_price) / 10000;
            //this._silverPart = int.Parse(price) / 100 - _goldPart * 100;

            this._minSellCopperPart = int.Parse(result.min_sale_unit_price) % 100;
            this._minSellSilverPart = (int.Parse(result.min_sale_unit_price) % 10000 - this._minSellCopperPart) / 100;
            this._minSellGoldPart = int.Parse(result.min_sale_unit_price) / 10000;
            //this._silverPart = int.Parse(price) / 100 - this._goldPart * 100;

            this._havingMaxBuyGoldPart = this._maxBuyGoldPart > 0;
            this._havingMaxBuySilverPart = this._maxBuySilverPart > 0 | this._havingMaxBuyGoldPart;
            this._havingMaxBuyCopperPart = this._maxBuyCopperPart > 0 | this._havingMaxBuySilverPart;

            this._havingMinSellGoldPart = _minSellGoldPart > 0;
            this._havingMinSellSilverPart = this._minSellSilverPart > 0 | this._havingMinSellGoldPart;
            this._havingMinSellCopperPart = this._minSellCopperPart > 0 | this._havingMinSellSilverPart;

            this._buyCount = int.Parse(result.offer_availability);
            this._sellCount = int.Parse(result.sale_availability);

        }


    }

}