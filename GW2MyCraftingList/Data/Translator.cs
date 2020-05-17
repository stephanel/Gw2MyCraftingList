using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GW2ExplorerCraftTool.Data
{
    interface ITanslated
    {
        string Name_En;
        string Name_Fr;
    }
    class Translator
    {
        private Dictionary<String, ITanslated> _values;

        public Dictionary<String, ITanslated> Values
        {
            get
            {
                return _values;
            }
        }

        public void Add(string key, ITanslated translated)
        {
            _values.Add(key, translated);
        }

        public Translator() { }
    }
}
