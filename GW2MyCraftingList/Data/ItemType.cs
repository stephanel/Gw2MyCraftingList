using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GW2ExplorerCraftTool.Data
{
    public class ItemType : ILocalizable
    {
        #region "constants"
        public const string ARMOR = "Armor";
        public const string BACK = "Back";
        public const string BAG = "Bag";
        public const string CONSUMABLE = "Consumable";
        public const string CONTAINER = "Container";
        public const string CRAFTING_MATERIAL = "CraftingMaterial";
        public const string GATHERING = "Gathering";
        public const string GIZMO = "Gizmo";
        public const string MINIPET = "MiniPet";
        public const string TOOL = "Tool";
        public const string TRINKET = "Trinket";
        public const string TROPHY = "Trophy";
        public const string UPGRADE_COMPONENT = "UpgradeComponent";
        public const string WEAPON = "Weapon";
        #endregion

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

        public ItemType(string key, string name_de, string name_en, string name_es, string name_fr)
        {
            this._key = key;
            this._name_de = name_de;
            this._name_en = name_en;
            this._name_es = name_es;
            this._name_fr = name_fr;
        }

        public override string ToString()
        {
            return Config.GetLocalizedName(this);
        }

        private static Dictionary<String, ItemType> _objectTypes;

        public static Dictionary<String, ItemType> ObjectTypes
        {
            get {
                return _objectTypes;
            }
        }
        static ItemType()
        {
            _objectTypes = new Dictionary<String, ItemType>();
            _objectTypes.Add(ARMOR, new ItemType(ARMOR, "Rüstung", "Armor", "Armadura", "Armure"));
            _objectTypes.Add(BACK, new ItemType(BACK, "Rücken", "Back", "Diseño de espalda", "Objet de dos"));
            _objectTypes.Add(BAG, new ItemType(BAG, "Taschen", "Bag", "Bolsa", "Sac"));
            _objectTypes.Add(CONSUMABLE, new ItemType(CONSUMABLE, "Verbrauchsgegenstand", "Consumable", "Consumible", "Consommable"));
            _objectTypes.Add(CONTAINER, new ItemType(CONTAINER, "Behälter", "Container", "Container", "Conteneur"));
            _objectTypes.Add(CRAFTING_MATERIAL, new ItemType(CRAFTING_MATERIAL, "Handwerksmaterial", "Crafting Material", "Componente de artesanía", "Matériaux d'artisanat"));
            _objectTypes.Add(GATHERING, new ItemType(GATHERING, "Sammelwerkzeug", "Gathering Tool", "Gathering Tool", "Outils de récolte"));
            _objectTypes.Add(GIZMO, new ItemType(GIZMO, "Geist-Verwandlung", "Gizmo", "Gizmo", "Tonique"));
            _objectTypes.Add(MINIPET, new ItemType(MINIPET, "Miniatur", "Miniature", "Miniatura", "Mini-familier"));
            _objectTypes.Add(TOOL, new ItemType(TOOL, "Tool", "Tool", "Tool", "Outils"));
            _objectTypes.Add(TRINKET, new ItemType(TRINKET, "Trinket", "Trinket", "Trinket", "Trinket")); // es=Equipamiento? ; fr=Colifichet?
            _objectTypes.Add(TROPHY, new ItemType(TROPHY, "Trophäe", "Trophy", "Tropeo", "Trophée"));
            _objectTypes.Add(UPGRADE_COMPONENT, new ItemType(UPGRADE_COMPONENT, "Aufwertungen", "Upgrade Component", "Componente de Mejora", "Composant d'amélioration"));
            _objectTypes.Add(WEAPON, new ItemType(WEAPON, "Waffe", "Weapon", "Arma", "Arme"));
        }
        public static Data.ItemType GetItemType(string key)
        {
            Data.ItemType ot = null;
            Data.ItemType.ObjectTypes.TryGetValue(key, out ot);
            return ot;
        }
    }
}
