using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GW2ExplorerCraftTool.Data
{
    public class Rarity : ILocalizable
    {
        public const string JUNK = "Junk";
        public const string BASIC = "Basic";
        public const string FINE = "Fine";
        public const string MASTERWORK = "Masterwork";
        public const string RARE = "Rare";
        public const string EXOTIC = "Exotic";
        public const string ASCENDED = "Ascended";
        public const string LEGENDARY = "Legendary";

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

        private System.Drawing.Color _color;

        public System.Drawing.Color Color
        {
            get { return _color; }
            set { _color = value; }
        }

        public Rarity(string key, string name_de, string name_en, string name_es, string name_fr, System.Drawing.Color color)
        {
            this._key = key;
            this._name_de = name_de;
            this._name_en = name_en;
            this._name_es = name_es;
            this._name_fr = name_fr;
            this._color = color;
        }

        public override string ToString()
        {
            return Config.GetLocalizedName(this);
        }

        private static Dictionary<String, Rarity> _rarities;

        public static Dictionary<String, Rarity> Rarities
        {
            get {
                return _rarities;
            }
        }
        static Rarity()
        {
            _rarities = new Dictionary<String, Rarity>();
            _rarities.Add(JUNK, new Rarity(JUNK, "Schrott", "Junk", "Basura", "Inutile", System.Drawing.ColorTranslator.FromHtml("#AAAAAA")));
            _rarities.Add(BASIC, new Rarity(BASIC, "Einfach", "Basic", "Genérico", "Simple", System.Drawing.ColorTranslator.FromHtml("#FFFFFF")));
            _rarities.Add(FINE, new Rarity(FINE, "Edel", "Fine", "Bueno", "Raffiné", System.Drawing.ColorTranslator.FromHtml("#4F9DFE")));
            _rarities.Add(MASTERWORK, new Rarity(MASTERWORK, "Meisterwerk", "Masterwork", "Obra Maestra", "Chef-d'œuvre", System.Drawing.ColorTranslator.FromHtml("#33cc11")));
            _rarities.Add(RARE, new Rarity(RARE, "Selten", "Rare", "Excepcional", "Rare", System.Drawing.Color.Yellow));//System.Drawing.ColorTranslator.FromHtml("#FDA500"));
            _rarities.Add(EXOTIC, new Rarity(EXOTIC, "Exotisch", "Exotic", "Exótico", "Exotique", System.Drawing.ColorTranslator.FromHtml("#FCD00B")));
            _rarities.Add(ASCENDED, new Rarity(ASCENDED, "Aufgestiegen", "Ascended", "Ascendido", "Elevé", System.Drawing.ColorTranslator.FromHtml("#C73171")));
            _rarities.Add(LEGENDARY, new Rarity(LEGENDARY, "Legendär", "Legendary", "Legendario", "Légendaire", System.Drawing.Color.Purple));

        }

        public static Data.Rarity GetRatity(string key)
        {
            Data.Rarity r = null;
            Data.Rarity.Rarities.TryGetValue(key, out r);
            return r;
        }

    }
}
