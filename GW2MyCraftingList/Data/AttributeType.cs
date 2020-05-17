using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GW2ExplorerCraftTool.Data
{
    public class AttributeType : ILocalizable
    {
        public const string CONDITION_DAMAGE="ConditionDamage";
        public const string CRITICAL_DAMAGE = "CritDamage";
        public const string DEFENSE = "Defense";
        public const string HEALING = "Healing";
        public const string POWER = "Power";
        public const string PRECISION = "Precision";
        public const string TOUGHNESS = "Toughness";
        public const string VITALITY = "Vitality";

        private string _key;
        public string Key
        {
            get { return _key; }
            set { _key = value; }
        }

        private string _scheme;
        public string Scheme
        {
            get { return _scheme; }
            set { _scheme = value; }
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

        public AttributeType(string key, string name_de, string name_en, string name_es, string name_fr, string scheme = "{0}: {1}")
        {
            this._key = key;
            this._name_de = name_de;
            this._name_en = name_en;
            this._name_es = name_es;
            this._name_fr = name_fr;
            this._scheme = scheme;
        }

        public override string ToString()
        {
            return Config.GetLocalizedName(this);
        }

        private static Dictionary<String, AttributeType> _attributeTypes;

        public static Dictionary<String, AttributeType> AttributeTypes
        {
            get {
                return _attributeTypes;
            }
        }
        static AttributeType()
        {
            try
            {
                _attributeTypes = new Dictionary<String, AttributeType>();
                _attributeTypes.Add(CONDITION_DAMAGE, new AttributeType(CONDITION_DAMAGE, "Zustandsschaden", "Condition Damage", "Daño de Condición", "Dégats d'altération"));
                _attributeTypes.Add(CRITICAL_DAMAGE, new AttributeType(CRITICAL_DAMAGE, "Kritischer Schaden", "Critical Damage", "Daño Crítico", "Dégats critique", "{0}: {1}%"));
                _attributeTypes.Add(DEFENSE, new AttributeType(DEFENSE, "Verteidigung", "Defense", "Armadura", "Defense"));
                _attributeTypes.Add(HEALING, new AttributeType(HEALING, "Heilkraft", "Healing Power", "Poder de Curación", "Puissance aux soins"));
                _attributeTypes.Add(POWER, new AttributeType(POWER, "Kraft", "Puissance", "Potencia", "Puissance"));
                _attributeTypes.Add(PRECISION, new AttributeType(PRECISION, "Präzision", "Precision", "Precisión", "Précision"));
                _attributeTypes.Add(TOUGHNESS, new AttributeType(TOUGHNESS, "Zähigkeit", "Toughness", "Fortaleza", "Robustesse"));
                _attributeTypes.Add(VITALITY, new AttributeType(VITALITY, "Vitalität", "Vitality", "Vitalidad", "Vitalité"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static Data.AttributeType GetAttributeType(string key)
        {
            Data.AttributeType r = null;
            Data.AttributeType.AttributeTypes.TryGetValue(key, out r);
            return r;
        }

    }
}
