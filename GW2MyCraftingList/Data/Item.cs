using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GW2ExplorerCraftTool.Data
{
   public class Item
    {
        private API.ANet.Item _item;

        public int Id
        {
            get
            {
                return int.Parse(_item.item_id);
            }
        }

        public Item(API.ANet.Item item)
        {
            this._item = item;
        }
    }
}
