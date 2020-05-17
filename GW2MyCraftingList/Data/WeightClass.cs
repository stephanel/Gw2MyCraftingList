using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GW2ExplorerCraftTool.Data
{
    class WeightClass : ILocalizable
    {
        public const string HEAVY = "Heavy";
        public const string LIGHT = "Light";
        public const string MEDIUM = "Medium";

        private string _key;
        public string Key
        {
            get { return _key; }
            set { _key = value; }
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

        public WeightClass(string key, string name_de, string name_en, string name_es, string name_fr)
        {
            this._key = key;
            this._name_de = name_de;
            this._name_en = name_en;
            this._name_es = name_es;
            this._name_fr = name_fr;
        }

        public override string ToString()
        {
            return this._name_fr;
        }

        private static Dictionary<String, WeightClass> _weightClasses;

        public static Dictionary<String, WeightClass> WeightClasses
        {
            get {
                return _weightClasses;
            }
        }
        static WeightClass()
        {
            _weightClasses = new Dictionary<String, WeightClass>();
            _weightClasses.Add(HEAVY, new WeightClass(HEAVY, "Schwere Rüstung", "Heavy Armor", "Armadura pesada", "Armure lourde"));
            _weightClasses.Add(LIGHT, new WeightClass(LIGHT, "Leichte Rüstung", "Light Armor", "Armadura ligera", "Armure légere"));
            _weightClasses.Add(MEDIUM, new WeightClass(MEDIUM, "Mittlere Rüstung", "Medium Armor", "Armadura media", "Armure Intermédiaire"));
        }
        public static Data.WeightClass GetWeightClass(string key)
        {
            Data.WeightClass ot = null;
            Data.WeightClass.WeightClasses.TryGetValue(key, out ot);
            return ot;
        }
    }
}
