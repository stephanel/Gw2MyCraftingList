using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GW2ExplorerCraftTool.Data
{
    public class Ingredient
    {
        private API.ANet.Item _item;

        public API.ANet.Item AnetItem
        {
            get
            {
                return _item;
            }
        }
        private int _count;
        public int Count
        {
            get { return _count; }
        }

        public Data.Recipe CreatedFrom
        {
            get
            {
                return Data.Cache.GetCache().Recipes.Where(p => p.OutputItemId!=null && (int)p.OutputItemId == this.Id).SingleOrDefault();
            }
        }

        public String Description
        {
            get
            {
                if (_item == null)
                    return null;
                return _item.description;
            }
        }

        public int? Id
        {
            get
            {
                if (_item == null || String.IsNullOrEmpty(_item.item_id))
                    return null;
                return int.Parse(_item.item_id);
            }
        }

        public string Image
        {
            get
            {
                if (_item == null)
                    return null;
                return _item.IconPath;  //new one using Anet Api only
            }
        }

        public bool InError
        {
            get
            {
                if (_item == null)
                    return true;
                return this._item.error == Config.DEFAULT_ERROR_CODE;
            }
        }

        public bool IsCraftable
        {
            get
            {
                return (CreatedFrom != null);
            }
        }

        public Data.ItemType ItemType
        {
            get
            {
                if (_item == null || String.IsNullOrEmpty(_item.type))
                    return null;
                Data.ItemType temp = null;
                string type = _item.type;
                Data.ItemType.ObjectTypes.TryGetValue(type, out temp);
                return temp;
            }
        }

        public String Name
        {
            get
            {
                if (_item == null)
                    return null;
                return _item.name;
            }
        }

        public Data.Rarity Rarity
        {
            get
            {
                if (_item == null || String.IsNullOrEmpty(_item.rarity))
                    return null;
                Data.Rarity temp = null;
                string rarity = _item.rarity;
                Data.Rarity.Rarities.TryGetValue(rarity, out temp);
                return temp;
            }
        }

        public System.Windows.Media.SolidColorBrush RarityColor
        {
            get
            {
                if (Rarity == null)
                    return null;
                System.Windows.Media.SolidColorBrush brush = new System.Windows.Media.SolidColorBrush();
                brush.Color = System.Windows.Media.Color.FromRgb(
                    this.Rarity.Color.R,
                    this.Rarity.Color.G,
                    this.Rarity.Color.B);
                return brush;
            }
        }

        public bool SomethingIsWrong
        {
            get 
            {
                if (_item == null)
                    return true;
                return !String.IsNullOrEmpty(_item.error);
            }
        }

        public Ingredient(API.ANet.Item item, int count)
        {
            this._item = item;
            this._count = count;
        }


        /* function used to search into recipes */
        public bool ContainsText(string pattern)
        {
            try
            {
                if (this._item != null)
                    if (_item.ContainsText(pattern))
                        return true;

                return false;
            }
            catch
            {
                throw;
            }
        }
        public bool EqualsText(string pattern)
        {
            try
            {
                if (this._item != null)
                    if (_item.EqualsText(pattern))
                        return true;

                return false;
            }
            catch
            {
                throw;
            }
        }

        //Data.API.Gw2Spidy.ItemResult result = null;
        public TradingPostData GetTradingPostData()
        {
            Data.API.Gw2Spidy.Item item = Data.DataProvider.GetGw2SpidyItem((int)this.Id);
            //if (item != null)
            //{
            //    result= item.result;
            //}
            //return new TradingPostData((int)id, result.max_offer_unit_price, result.min_sale_unit_price, result.offer_availability, result.sale_availability);
            return new TradingPostData((int)this.Id, item);
        }

        public override string ToString()
        {
            /*if (_item == null)
                return null;

            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            if (!String.IsNullOrEmpty(_item.description))
                sb.AppendLine(_item.description.Replace("<br>", Environment.NewLine));
            sb.AppendLine();

            if (_item.armor != null)
                sb.Append(_item.armor.ToString());
            if (_item.bag != null)
                sb.Append(_item.bag.ToString());
            if (_item.consumable != null)
                sb.Append(_item.consumable.ToString());
            if (_item.container != null)
                sb.Append(_item.container.ToString());
            if (_item.crafting_material != null)
                sb.Append(_item.crafting_material.ToString());
            if (_item.gizmo != null)
                sb.Append(_item.gizmo.ToString());
            if (_item.trinket != null)
                sb.Append(_item.trinket.ToString());
            if (_item.trophy != null)
                sb.Append(_item.trophy.ToString());
            if (_item.upgrade_component != null)
                sb.Append(_item.upgrade_component.ToString());
            if (_item.weapon != null)
                sb.Append(_item.weapon.ToString());

            if (!String.IsNullOrEmpty(_item.rarity))
                sb.AppendLine(Config.GetLocalizedName(Data.Rarity.GetRatity(_item.rarity)));
            if (!String.IsNullOrEmpty(_item.level))
                sb.AppendLine(String.Format("Niveau requis : {0}", _item.level));

            return sb.ToString();*/

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            if(this._item!=null)
                sb.AppendLine(this._item.ToString());
            if (this.IsCraftable)
            {
                sb.AppendLine(Properties.Resources.ItemDescriptionIsCraftableLabel);
            }

            return sb.ToString();
        }
    }
}
