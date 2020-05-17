using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GW2ExplorerCraftTool.Data
{
    class TextSearchOperator : ILocalizable
    {
        public const string CONTAINS = "contains";
        public const string EQUALS = "equals";

        public enum Modes { Equals = 0, Contains = 1 }

        private Modes _mode;

        public Modes Mode
        {
            get { return _mode; }
            set { _mode = value; }
        }

        private string _name_de;
        public string Name_De
        {
            get { return _name_de; }
            set { _name_en = value; }
        }

        private string _name_en;
        public string Name_En
        {
            get { return _name_en; }
            set { _name_en = value; }
        }

        private string _name_es;
        public string Name_Es
        {
            get { return _name_es; }
            set { _name_en = value; }
        }

        private string _name_fr;
        public string Name_Fr
        {
            get { return _name_fr; }
            set { _name_fr = value; }
        }

        private static Dictionary<String, TextSearchOperator> _modes;

        public static Dictionary<String, TextSearchOperator> TextSearchModes
        {
            get
            {
                return _modes;
            }
        }

        public TextSearchOperator(Modes mode, string name_de, string name_en, string name_es, string name_fr)
        {
            this._mode = mode;
            this._name_de = name_de;
            this._name_en = name_en;
            this._name_es = name_es;
            this._name_fr = name_fr;
        }

        public override string ToString()
        {
            return Config.GetLocalizedName(this);
        }

        static TextSearchOperator()
        {
            _modes = new Dictionary<String, TextSearchOperator>();;
            _modes.Add(CONTAINS, new TextSearchOperator(Modes.Contains, "Enthält", "Contains", "Contiene", "Contient"));
            _modes.Add(EQUALS, new TextSearchOperator(Modes.Equals, "Ist gleich", "Is Equals", "Es igual", "Est égal"));
        }

        public static Data.TextSearchOperator GetTextSearchMode(string key)
        {
            Data.TextSearchOperator r = null;
            Data.TextSearchOperator.TextSearchModes.TryGetValue(key, out r);
            return r;
        }

    }

}
