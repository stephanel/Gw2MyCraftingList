using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GW2ExplorerCraftTool.Data
{
    public class Recipe
    {
        private API.ANet.Recipe _recipe;

        public API.ANet.Recipe AnetRecipe
        {
            get {
                return _recipe;
            }
        }

        public int OutputItemsCount
        {
            get
            {
                if (String.IsNullOrEmpty(_recipe.output_item_count))
                    return 0;
                return int.Parse(_recipe.output_item_count); 
            }
        }

        public String Description
        {
            get
            {
                return _recipe.Item.description;
            }
        }

        public List<Data.Discipline> Disciplines
        {
            get
            {
                if (_recipe.disciplines == null)
                    return null;
                List<Data.Discipline> temp = new List<Data.Discipline>();
                if (_recipe.disciplines.Length == 0)
                    return temp;
                foreach (string d in _recipe.disciplines)
                {
                    Data.Discipline discipline = null;
                    Data.Discipline.Disciplines.TryGetValue(d, out discipline);
                    if (discipline != null)
                        temp.Add(discipline);
                }
                return temp;
            }
        }

        public int? Id
        {
            get {
                if (_recipe == null || String.IsNullOrEmpty(_recipe.recipe_id))
                    return null;
                return int.Parse(_recipe.recipe_id);
            }
        }

        private bool _is_checked;
        public bool Is_checked
        {
            get
            {
                return _is_checked;
            }
            set
            {
                _is_checked = value;
            }
        }

        private bool _is_saved;
        public bool Is_saved
        {
            get { return _is_saved; }
            set { _is_saved = value; }
        }

        public string Image
        {
            get
            {
                //return _recipe.Item.img;    // old version using GW2Spidy API
                return _recipe.Item.IconPath;  //new one using Anet Api only
            }
        }

        public List<Ingredient> Ingredients
        {
            get
            {
                if (_recipe.ingredients == null)
                    return null;
                try
                {
                    return (from ingredient in _recipe.ingredients select new Ingredient(ingredient.Item, int.Parse(ingredient.count))).ToList();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public API.ANet.Item Item
        {
            get
            {
                return _recipe.Item;
            }
        }
        
        public int? OutputItemId
        {
            get
            {
                if (_recipe == null || String.IsNullOrEmpty(_recipe.output_item_id))
                    return null;
                return int.Parse(_recipe.output_item_id);
            }
        }

        public string Gw2Db_External_Id
        {
            get
            {
                if (_recipe.Item == null)
                    return null;
                if (_recipe.Item.gw2db_external_id == null)
                    return null;
                return _recipe.Item.gw2db_external_id;
            }
        }

        //public long LineNumber
        //{
        //    get
        //    {
        //        return long.Parse(_recipe.line_number);
        //    }
        //    set
        //    {
        //        _recipe.line_number = value.ToString();
        //    }
        //}

        public string MinRate
        {
            get
            {
                return this._recipe.min_rating;
            }
        }

        public String Name
        {
            get{
                return _recipe.Item.name;
            }
        }

        public Data.Rarity Rarity
        {
            get
            {
                if (_recipe.Item == null || String.IsNullOrEmpty(_recipe.Item.rarity))
                    return null;
                Data.Rarity temp = null;
                string rarity = _recipe.Item.rarity;
                Data.Rarity.Rarities.TryGetValue(rarity, out temp);
                return temp;
            }
        }

        public System.Windows.Media.SolidColorBrush RarityColor
        {
            get
            {
                if (this.Rarity == null)
                    return null;
                System.Windows.Media.SolidColorBrush brush = new System.Windows.Media.SolidColorBrush();
                brush.Color = System.Windows.Media.Color.FromRgb(
                    this.Rarity.Color.R,
                    this.Rarity.Color.G,
                    this.Rarity.Color.B);
                return brush;
            }
        }

        public Data.RecipeType RecipeType
        {
            get
            {
                if (String.IsNullOrEmpty(_recipe.type))
                    return null;
                Data.RecipeType temp = null;
                string type = _recipe.type;
                Data.RecipeType.RecipeTypes.TryGetValue(type, out temp);
                return temp;
            }
        }

        private long _session_id;
        public long SessionID
        {
            get { return _session_id; }
        }

        public bool SomethingIsWrong
        {
            get 
            {
                if (_recipe.Item == null)
                    return true;
                return !String.IsNullOrEmpty(_recipe.Item.error); 
            }
        }

        public string Type
        {
            get { return _recipe.Item.type; }
        }

        public Recipe(API.ANet.Recipe recipe, bool is_checked, bool is_saved, long session_id)
        {
            this._recipe = recipe;
            this._is_checked = is_checked;
            this._is_saved = is_saved;
            this._session_id = session_id;
        }

        public string GetItemDescription()
        {
            if (_recipe.Item== null)
                return null;
            return _recipe.Item.ToString();
        }

        /* function used to search into recipes */
        public bool ContainsText(string pattern)
        {
            try
            {
                // search into item caracteristics
                if (this._recipe != null)
                    if (this._recipe.ContainsText(pattern))
                        return true;

                return false;
            }
            catch {
                throw; 
            }
        }
        public bool EqualsText(string pattern)
        {
            try
            {
                // search into item caracteristics
                if (this._recipe != null)
                    if (this._recipe.EqualsText(pattern))
                        return true;

                return false;
            }
            catch
            {
                throw;
            }
        }

        public bool IngredientContainsText(string pattern)
        {
            try
            {
                if(this.Ingredients!=null)
                    foreach(Ingredient i in this.Ingredients)
                        if(i.ContainsText(pattern))
                            return true;

                return false;
            }
            catch
            {
                throw;
            }
        }
        public bool IngredientEqualsText(string pattern)
        {
            try
            {
                if (this.Ingredients != null)
                    foreach (Ingredient i in this.Ingredients)
                        if (i.EqualsText(pattern))
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
            if (this.OutputItemId==null)
                return null;

            Data.API.Gw2Spidy.Item item = Data.DataProvider.GetGw2SpidyItem((int)this.OutputItemId);
            //if (item != null)
            //{
            //    result = item.result;
            //}
            //return new TradingPostData((int)this.OutputItemId, result.max_offer_unit_price, result.min_sale_unit_price, result.offer_availability, result.sale_availability);
            return new TradingPostData((int)this.OutputItemId, item);
        }

        public override string ToString()
        {
            return _recipe.ToString();
        }
    }
}
