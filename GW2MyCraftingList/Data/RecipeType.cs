using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GW2ExplorerCraftTool.Data
{
    public class RecipeType : ILocalizable
    {
        #region "constants"
        // not affected 
        public const string AQUATIC_HELM = "HelmAquatic";
        // armorsmith, leatherworker, tailor
        public const string BAG = "Bag";
        public const string BOOTS = "Boots";
        public const string BULK = "Bulk";
        public const string COAT = "Coat";
        public const string COMPONENT = "Component";
        public const string GLOVES= "Gloves";
        public const string HELM= "Helm";
        public const string INSIGNIA= "Insignia";
        public const string LEGGINGS= "Leggings";
        public const string REFINEMENT = "Refinement";
        public const string SHOULDERS= "Shoulders";
        public const string UPGRADE_COMPONENT = "UpgradeComponent";
        // artificer
        public const string CONSUMABLE = "Consumable";
        public const string INSCRIPTION = "Inscription";
        public const string FOCUS= "Focus";
        public const string POTION= "Potion";
        public const string SCEPTER= "Scepter";
        public const string STAFF= "Staff";
        public const string TRIDENT= "Trident";
        // chef
        public const string DESSERT= "Dessert";
        public const string DYE= "Dye";
        public const string FEAST= "Feast";
        public const string INGREDIENT_COOKING= "IngredientCooking";
        public const string MEAL= "Meal";
        public const string SEASONING= "Seasoning";
        public const string SNACK= "Snack";
        public const string SOUP= "Soup";
        // huntsman
        public const string LONGBOW= "LongBow";
        public const string PISTOL= "Pistol";
        public const string RIFLE= "Rifle";
        public const string SHORTBOW= "ShortBow";
        public const string SPEARGUN= "Speargun";
        public const string TORCH= "Torch";
        public const string WARHORN= "Warhorn";
        // jeweler
        public const string AMULET= "Amulet";
        public const string EARRING= "Earring";
        public const string RING= "Ring";
        // scribe
        public const string GUILD_CONSUMABLE = "Guild Consumable";
        public const string SCHEMATICS = "Schematics";
        public const string WVW_CLAIMING = "WvW Claiming";
        // weaponsmith
        public const string AXE= "Axe";
        public const string DAGGER= "Dagger";
        public const string GREATSWORD= "Greatsword";
        public const string HAMMER= "Hammer";
        public const string HARPOON= "Harpoon";
        public const string MACE= "Mace";
        public const string SHIELD= "Shield";
        public const string SWORD= "Sword";
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

        private List<Data.Discipline> _disciplines;
        public List<Data.Discipline> Disciplines
        {
            get
            {
                return _disciplines;
            }
        }

        public RecipeType(string key, string name_de, string name_en, string name_es, string name_fr)
        {
            this._key = key;
            this._name_de = name_de;
            this._name_en = name_en;
            this._name_es = name_es;
            this._name_fr = name_fr;
            this._disciplines = new List<Data.Discipline>();
        }

        public override string ToString()
        {
            return Config.GetLocalizedName(this);
        }

        private static Dictionary<String, RecipeType> _recipeTypes;

        public static Dictionary<String, RecipeType> RecipeTypes
        {
            get
            {
                return _recipeTypes;
            }
        }
        static RecipeType()
        {
            _recipeTypes = new Dictionary<String, RecipeType>();

            // all except chef
            _recipeTypes.Add(COMPONENT, new RecipeType(COMPONENT, "Komponente", "Component", "Componente", "Composant"));
            _recipeTypes.Add(CONSUMABLE, new RecipeType(CONSUMABLE, "Consumable", "Consumable", "Consumible", "Consommable"));
            _recipeTypes.Add(REFINEMENT, new RecipeType(REFINEMENT, "Veredelung", "Refinement", "Refinamiento", "Perfectionnement"));
            _recipeTypes.Add(UPGRADE_COMPONENT, new RecipeType(UPGRADE_COMPONENT, "Aufwertungen", "Upgrade Component", "Componente de Mejora", "Composant d'amélioration"));

            // artificer, huntsman, scribe, weaponsmith
            //-- inscriptions are used to craft weapons
            _recipeTypes.Add(INSCRIPTION, new RecipeType(INSCRIPTION, "Inschrift", "Inscription", "Inscripcion", "Inscription"));

            // armorsmith, leatherworker, tailor
            _recipeTypes.Add(BAG, new RecipeType(BAG, "Taschen", "Bag", "Bolsa", "Sac"));
            _recipeTypes.Add(BOOTS, new RecipeType(BOOTS, "Stiefel", "Boots", "Botas", "Bottes"));
            _recipeTypes.Add(BULK, new RecipeType(BULK, "Bulk", "Bulk", "Bulk", "Kit d'armure"));
            _recipeTypes.Add(COAT, new RecipeType(COAT, "Wams", "Coat", "Abrigo", "Manteau"));
            _recipeTypes.Add(GLOVES, new RecipeType(GLOVES, "Stulpenhandschuhe", "Gloves", "Guantes", "Gants"));
            _recipeTypes.Add(HELM, new RecipeType(HELM, "Helms", "Helm", "Yelmo", "Heaume"));
            _recipeTypes.Add(AQUATIC_HELM, new RecipeType(AQUATIC_HELM, "Atemgerät", "Breathing apparatus", "Armadura de cabeza", "Masque aquatique"));
            //-- insignias are used to craft armors
            _recipeTypes.Add(INSIGNIA, new RecipeType(INSIGNIA, "Insignie", "Insignia", "Insignia", "Insigne"));
            _recipeTypes.Add(LEGGINGS, new RecipeType(LEGGINGS, "Beinschutz", "Leggings", "Calzas", "Jambières"));
            _recipeTypes.Add(SHOULDERS, new RecipeType(SHOULDERS, "Schulters", "Shoulders", "Hombreras", "Épaulières"));

            // artificer
            _recipeTypes.Add(FOCUS, new RecipeType(FOCUS, "Fokus", "Focus", "Foco", "Focus"));
            _recipeTypes.Add(POTION, new RecipeType(POTION, "Trank", "Potion", "Poción", "Potion"));
            _recipeTypes.Add(SCEPTER, new RecipeType(SCEPTER, "Zepter", "Scepter", "Cetro", "Sceptre"));
            _recipeTypes.Add(STAFF, new RecipeType(STAFF, "Stab", "Staff", "Báculo", "Bâton"));
            _recipeTypes.Add(TRIDENT, new RecipeType(TRIDENT, "Dreizack", "Trident", "Tridente", "Trident"));

            // chef
            _recipeTypes.Add(DESSERT, new RecipeType(DESSERT, "Dessert", "Dessert", "Postre", "Déssert"));
            _recipeTypes.Add(DYE, new RecipeType(DYE, "Farbstoff", "Dye", "Tinte", "Teinture"));
            _recipeTypes.Add(FEAST, new RecipeType(FEAST, "Fest/Tablett", "Feast/Tray", "Feast/Tray", "Festin/Plateau"));
            _recipeTypes.Add(INGREDIENT_COOKING, new RecipeType(INGREDIENT_COOKING, "Kochzutat", "Cooking Ingredient", "Ingredient de cocina", "Ingrédient culinaire"));
            _recipeTypes.Add(MEAL, new RecipeType(MEAL, "Mahlzeiten", "Meal", "Comida", "Plat"));
            _recipeTypes.Add(SEASONING, new RecipeType(SEASONING, "Gewürz", "Seasoning", "Condimento", "Assaisonnement"));
            _recipeTypes.Add(SNACK, new RecipeType(SNACK, "Imbisse", "Snack", "Tentempié", "En-cas"));
            _recipeTypes.Add(SOUP, new RecipeType(SOUP, "Suppen", "Soup", "Sopa", "Soupe"));

            // huntsman
            _recipeTypes.Add(LONGBOW, new RecipeType(LONGBOW, "Langbogen", "LongBow", "Arco largo", "Arc long"));
            _recipeTypes.Add(PISTOL, new RecipeType(PISTOL, "Pistole", "Pistol", "Pistola", "Pistolet"));
            _recipeTypes.Add(RIFLE, new RecipeType(RIFLE, "Gewehr", "Rifle", "Rifle", "Fusil"));
            _recipeTypes.Add(SHORTBOW, new RecipeType(SHORTBOW, "Kurzbogen", "ShortBow", "Arco corto", "Arc court"));
            _recipeTypes.Add(SPEARGUN, new RecipeType(SPEARGUN, "Harpunenschleuder", "Harpoon Gun", "Cañon de arpón", "Fusil-harpon"));
            _recipeTypes.Add(TORCH, new RecipeType(TORCH, "Fackel", "Torch", "Antorcha", "Torche"));
            _recipeTypes.Add(WARHORN, new RecipeType(WARHORN, "Kriegshorn", "Warhorn", "Cuerno de guerra", "Cor de guerre"));

            // jeweler
            _recipeTypes.Add(AMULET, new RecipeType(AMULET, "Amulett", "Amulet", "Amuleto", "Amulette"));
            _recipeTypes.Add(EARRING, new RecipeType(EARRING, "Ohrring", "Earring", "Pendiente", "Boucle d'oreille"));
            _recipeTypes.Add(RING, new RecipeType(RING, "Ring", "Ring", "Anillo", "Bague"));

            // scribe
            _recipeTypes.Add(GUILD_CONSUMABLE, new RecipeType(GUILD_CONSUMABLE, "GUILD_CONSUMABLE", GUILD_CONSUMABLE, "GUILD_CONSUMABLE", "GUILD_CONSUMABLE"));
            _recipeTypes.Add(SCHEMATICS, new RecipeType(SCHEMATICS, "Schematics_De", "Schematics_En", "Schematics_Es", "Schémas"));
            _recipeTypes.Add(WVW_CLAIMING, new RecipeType(WVW_CLAIMING, "WVW_CLAIMING", WVW_CLAIMING, "WVW_CLAIMING", "WVW_CLAIMING"));

            // weaponsmith
            _recipeTypes.Add(AXE, new RecipeType(AXE, "Axt", "Axe", "hacha", "Hache"));
            _recipeTypes.Add(DAGGER, new RecipeType(DAGGER, "Dolch", "Dagger", "Daga", "Dague"));
            _recipeTypes.Add(GREATSWORD, new RecipeType(GREATSWORD, "Großschwert", "Greatsword", "Mandoble", "Epée à deux mains"));
            _recipeTypes.Add(HAMMER, new RecipeType(HAMMER, "Hammer", "Hammer", "Martillo", "Marteau"));
            _recipeTypes.Add(HARPOON, new RecipeType(HARPOON, "Speer", "Harpoon", "Lanza", "Lance"));
            _recipeTypes.Add(MACE, new RecipeType(MACE, "Streitkolben", "Mace", "Maza", "Masse"));
            _recipeTypes.Add(SHIELD, new RecipeType(SHIELD, "Schild", "Shield", "Escudo", "Bouclier"));
            _recipeTypes.Add(SWORD, new RecipeType(SWORD, "Schwert", "Sword", "Espada", "Epée"));

        }

        public static Data.RecipeType GetRecipeType(string key)
        {
            Data.RecipeType r = null;
            Data.RecipeType.RecipeTypes.TryGetValue(key, out r);
            return r;
        }
    }
}
