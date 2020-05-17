using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;

namespace GW2ExplorerCraftTool.Data
{
    public class Discipline : ILocalizable
    {
        public const string ARMORSMITH = "Armorsmith";
        public const string ARTIFICER = "Artificer";
        public const string CHEF = "Chef";
        public const string HUNTSMAN = "Huntsman";
        public const string JEWELER = "Jeweler";
        public const string LEATHERWORKER = "Leatherworker";
        public const string SCRIBE = "Scribe";
        public const string TAILOR = "Tailor";
        public const string WEAPONSMITH = "Weaponsmith";

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

        private string _path;

        public string Path
        {
            get { return _path; }
            set { _path = value; }
        }

        private Dictionary<String, RecipeType> _recipeTypes;

        public Dictionary<String, RecipeType> RecipeTypes
        {
            get
            {
                return _recipeTypes;
            }
        }

        public Discipline() { }

        public Discipline(string key, string name_de, string name_en, string name_es, string name_fr, string path, Dictionary<String, RecipeType> recipeTypes)
        {
            this._key = key;
            this._name_de = name_de;
            this._name_en = name_en;
            this._name_es = name_es;
            this._name_fr = name_fr;
            this._path = path;
            this._recipeTypes = recipeTypes;
        }

        public override string ToString()
        {
            return Config.GetLocalizedName(this);
        }

        private static Dictionary<String, Discipline> _disciplines;

        public static Dictionary<String, Discipline> Disciplines
        {
            get {
                return _disciplines;
            }
        }
      
        static Discipline()
        {

            Dictionary<String, RecipeType> _armorsmithRecipeTypes = new Dictionary<String, RecipeType>();
            Dictionary<String, RecipeType> _artificerRecipeTypes = new Dictionary<String, RecipeType>();
            Dictionary<String, RecipeType> _chefRecipeTypes = new Dictionary<String, RecipeType>();
            Dictionary<String, RecipeType> _huntsmanRecipeTypes = new Dictionary<String, RecipeType>();
            Dictionary<String, RecipeType> _jewelerRecipeTypes = new Dictionary<String, RecipeType>();
            Dictionary<String, RecipeType> _leatherworkerRecipeTypes = new Dictionary<String, RecipeType>();
            Dictionary<String, RecipeType> _scribeRecipeTypes = new Dictionary<String, RecipeType>();
            Dictionary<String, RecipeType> _tailorRecipeTypes = new Dictionary<String, RecipeType>();
            Dictionary<String, RecipeType> _weaponsmithRecipeTypes = new Dictionary<String, RecipeType>();
  

            // armorsmith
            _armorsmithRecipeTypes.Add(RecipeType.COMPONENT, GetRecipeType(RecipeType.COMPONENT));
            _armorsmithRecipeTypes.Add(RecipeType.CONSUMABLE, GetRecipeType(RecipeType.CONSUMABLE));
            _armorsmithRecipeTypes.Add(RecipeType.REFINEMENT, GetRecipeType(RecipeType.REFINEMENT));
            _armorsmithRecipeTypes.Add(RecipeType.UPGRADE_COMPONENT, GetRecipeType(RecipeType.UPGRADE_COMPONENT));
            _armorsmithRecipeTypes.Add(RecipeType.BAG, GetRecipeType(RecipeType.BAG));
            _armorsmithRecipeTypes.Add(RecipeType.BOOTS, GetRecipeType(RecipeType.BOOTS));
            _armorsmithRecipeTypes.Add(RecipeType.BULK, GetRecipeType(RecipeType.BULK));
            _armorsmithRecipeTypes.Add(RecipeType.COAT, GetRecipeType(RecipeType.COAT));
            _armorsmithRecipeTypes.Add(RecipeType.GLOVES, GetRecipeType(RecipeType.GLOVES));
            _armorsmithRecipeTypes.Add(RecipeType.HELM, GetRecipeType(RecipeType.HELM));
            _armorsmithRecipeTypes.Add(RecipeType.INSIGNIA, GetRecipeType(RecipeType.INSIGNIA));
            _armorsmithRecipeTypes.Add(RecipeType.LEGGINGS, GetRecipeType(RecipeType.LEGGINGS));
            _armorsmithRecipeTypes.Add(RecipeType.SHOULDERS, GetRecipeType(RecipeType.SHOULDERS));
            // artificer
            _artificerRecipeTypes.Add(RecipeType.COMPONENT, GetRecipeType(RecipeType.COMPONENT));
            _artificerRecipeTypes.Add(RecipeType.CONSUMABLE, GetRecipeType(RecipeType.CONSUMABLE));
            _artificerRecipeTypes.Add(RecipeType.REFINEMENT, GetRecipeType(RecipeType.REFINEMENT));
            _artificerRecipeTypes.Add(RecipeType.UPGRADE_COMPONENT, GetRecipeType(RecipeType.UPGRADE_COMPONENT));
            _artificerRecipeTypes.Add(RecipeType.INSCRIPTION, GetRecipeType(RecipeType.INSCRIPTION));
            _artificerRecipeTypes.Add(RecipeType.FOCUS, GetRecipeType(RecipeType.FOCUS));
            _artificerRecipeTypes.Add(RecipeType.POTION, GetRecipeType(RecipeType.POTION));
            _artificerRecipeTypes.Add(RecipeType.SCEPTER, GetRecipeType(RecipeType.SCEPTER));
            _artificerRecipeTypes.Add(RecipeType.STAFF, GetRecipeType(RecipeType.STAFF));
            _artificerRecipeTypes.Add(RecipeType.TRIDENT, GetRecipeType(RecipeType.TRIDENT));
            // chef
            _chefRecipeTypes.Add(RecipeType.DESSERT, GetRecipeType(RecipeType.DESSERT));
            _chefRecipeTypes.Add(RecipeType.DYE, GetRecipeType(RecipeType.DYE));
            _chefRecipeTypes.Add(RecipeType.FEAST, GetRecipeType(RecipeType.FEAST));
            _chefRecipeTypes.Add(RecipeType.INGREDIENT_COOKING, GetRecipeType(RecipeType.INGREDIENT_COOKING));
            _chefRecipeTypes.Add(RecipeType.MEAL, GetRecipeType(RecipeType.MEAL));
            _chefRecipeTypes.Add(RecipeType.SEASONING, GetRecipeType(RecipeType.SEASONING));
            _chefRecipeTypes.Add(RecipeType.SNACK, GetRecipeType(RecipeType.SNACK));
            _chefRecipeTypes.Add(RecipeType.SOUP, GetRecipeType(RecipeType.SOUP));
            // huntsman
            _huntsmanRecipeTypes.Add(RecipeType.COMPONENT, GetRecipeType(RecipeType.COMPONENT));
            _huntsmanRecipeTypes.Add(RecipeType.CONSUMABLE, GetRecipeType(RecipeType.CONSUMABLE));
            _huntsmanRecipeTypes.Add(RecipeType.REFINEMENT, GetRecipeType(RecipeType.REFINEMENT));
            _huntsmanRecipeTypes.Add(RecipeType.UPGRADE_COMPONENT, GetRecipeType(RecipeType.UPGRADE_COMPONENT));
            _huntsmanRecipeTypes.Add(RecipeType.INSCRIPTION, GetRecipeType(RecipeType.INSCRIPTION));
            _huntsmanRecipeTypes.Add(RecipeType.LONGBOW, GetRecipeType(RecipeType.LONGBOW));
            _huntsmanRecipeTypes.Add(RecipeType.PISTOL, GetRecipeType(RecipeType.PISTOL));
            _huntsmanRecipeTypes.Add(RecipeType.RIFLE, GetRecipeType(RecipeType.RIFLE));
            _huntsmanRecipeTypes.Add(RecipeType.SHORTBOW, GetRecipeType(RecipeType.SHORTBOW));
            _huntsmanRecipeTypes.Add(RecipeType.SPEARGUN, GetRecipeType(RecipeType.SPEARGUN));
            _huntsmanRecipeTypes.Add(RecipeType.TORCH, GetRecipeType(RecipeType.TORCH));
            _huntsmanRecipeTypes.Add(RecipeType.WARHORN, GetRecipeType(RecipeType.WARHORN));
            // jeweler
            _jewelerRecipeTypes.Add(RecipeType.COMPONENT, GetRecipeType(RecipeType.COMPONENT));
            _jewelerRecipeTypes.Add(RecipeType.CONSUMABLE, GetRecipeType(RecipeType.CONSUMABLE));
            _jewelerRecipeTypes.Add(RecipeType.REFINEMENT, GetRecipeType(RecipeType.REFINEMENT));
            _jewelerRecipeTypes.Add(RecipeType.UPGRADE_COMPONENT, GetRecipeType(RecipeType.UPGRADE_COMPONENT));
            _jewelerRecipeTypes.Add(RecipeType.AMULET, GetRecipeType(RecipeType.AMULET));
            _jewelerRecipeTypes.Add(RecipeType.EARRING, GetRecipeType(RecipeType.EARRING));
            _jewelerRecipeTypes.Add(RecipeType.RING, GetRecipeType(RecipeType.RING));
            // leatherworker
            _leatherworkerRecipeTypes.Add(RecipeType.COMPONENT, GetRecipeType(RecipeType.COMPONENT));
            _leatherworkerRecipeTypes.Add(RecipeType.CONSUMABLE, GetRecipeType(RecipeType.CONSUMABLE));
            _leatherworkerRecipeTypes.Add(RecipeType.REFINEMENT, GetRecipeType(RecipeType.REFINEMENT));
            _leatherworkerRecipeTypes.Add(RecipeType.UPGRADE_COMPONENT, GetRecipeType(RecipeType.UPGRADE_COMPONENT));
            _leatherworkerRecipeTypes.Add(RecipeType.BAG, GetRecipeType(RecipeType.BAG));
            _leatherworkerRecipeTypes.Add(RecipeType.BOOTS, GetRecipeType(RecipeType.BOOTS));
            _leatherworkerRecipeTypes.Add(RecipeType.BULK, GetRecipeType(RecipeType.BULK));
            _leatherworkerRecipeTypes.Add(RecipeType.COAT, GetRecipeType(RecipeType.COAT));
            _leatherworkerRecipeTypes.Add(RecipeType.GLOVES, GetRecipeType(RecipeType.GLOVES));
            _leatherworkerRecipeTypes.Add(RecipeType.HELM, GetRecipeType(RecipeType.HELM));
            _leatherworkerRecipeTypes.Add(RecipeType.INSIGNIA, GetRecipeType(RecipeType.INSIGNIA));
            _leatherworkerRecipeTypes.Add(RecipeType.LEGGINGS, GetRecipeType(RecipeType.LEGGINGS));
            _leatherworkerRecipeTypes.Add(RecipeType.SHOULDERS, GetRecipeType(RecipeType.SHOULDERS));
            // scibe
            _scribeRecipeTypes.Add(RecipeType.BAG, GetRecipeType(RecipeType.BAG));
            _scribeRecipeTypes.Add(RecipeType.CONSUMABLE, GetRecipeType(RecipeType.CONSUMABLE));
            _scribeRecipeTypes.Add(RecipeType.COMPONENT, GetRecipeType(RecipeType.COMPONENT));
            _scribeRecipeTypes.Add(RecipeType.FEAST, GetRecipeType(RecipeType.FEAST));
            _scribeRecipeTypes.Add(RecipeType.GUILD_CONSUMABLE, GetRecipeType(RecipeType.GUILD_CONSUMABLE));
            _scribeRecipeTypes.Add(RecipeType.REFINEMENT, GetRecipeType(RecipeType.REFINEMENT));
            _scribeRecipeTypes.Add(RecipeType.SCHEMATICS, GetRecipeType(RecipeType.SCHEMATICS));
            _scribeRecipeTypes.Add(RecipeType.WVW_CLAIMING, GetRecipeType(RecipeType.WVW_CLAIMING));
            // taylor
            _tailorRecipeTypes.Add(RecipeType.COMPONENT, GetRecipeType(RecipeType.COMPONENT));
            _tailorRecipeTypes.Add(RecipeType.CONSUMABLE, GetRecipeType(RecipeType.CONSUMABLE));
            _tailorRecipeTypes.Add(RecipeType.REFINEMENT, GetRecipeType(RecipeType.REFINEMENT));
            _tailorRecipeTypes.Add(RecipeType.UPGRADE_COMPONENT, GetRecipeType(RecipeType.UPGRADE_COMPONENT));
            _tailorRecipeTypes.Add(RecipeType.BAG, GetRecipeType(RecipeType.BAG));
            _tailorRecipeTypes.Add(RecipeType.BOOTS, GetRecipeType(RecipeType.BOOTS));
            _tailorRecipeTypes.Add(RecipeType.BULK, GetRecipeType(RecipeType.BULK));
            _tailorRecipeTypes.Add(RecipeType.COAT, GetRecipeType(RecipeType.COAT));
            _tailorRecipeTypes.Add(RecipeType.GLOVES, GetRecipeType(RecipeType.GLOVES));
            _tailorRecipeTypes.Add(RecipeType.HELM, GetRecipeType(RecipeType.HELM));
            _tailorRecipeTypes.Add(RecipeType.INSIGNIA, GetRecipeType(RecipeType.INSIGNIA));
            _tailorRecipeTypes.Add(RecipeType.LEGGINGS, GetRecipeType(RecipeType.LEGGINGS));
            _tailorRecipeTypes.Add(RecipeType.SHOULDERS, GetRecipeType(RecipeType.SHOULDERS));
            // weaponsmith
            _weaponsmithRecipeTypes.Add(RecipeType.COMPONENT, GetRecipeType(RecipeType.COMPONENT));
            _weaponsmithRecipeTypes.Add(RecipeType.CONSUMABLE, GetRecipeType(RecipeType.CONSUMABLE));
            _weaponsmithRecipeTypes.Add(RecipeType.REFINEMENT, GetRecipeType(RecipeType.REFINEMENT));
            _weaponsmithRecipeTypes.Add(RecipeType.UPGRADE_COMPONENT, GetRecipeType(RecipeType.UPGRADE_COMPONENT));
            _weaponsmithRecipeTypes.Add(RecipeType.INSCRIPTION, GetRecipeType(RecipeType.INSCRIPTION));
            _weaponsmithRecipeTypes.Add(RecipeType.AXE, GetRecipeType(RecipeType.AXE));
            _weaponsmithRecipeTypes.Add(RecipeType.DAGGER, GetRecipeType(RecipeType.DAGGER));
            _weaponsmithRecipeTypes.Add(RecipeType.GREATSWORD, GetRecipeType(RecipeType.GREATSWORD));
            _weaponsmithRecipeTypes.Add(RecipeType.HAMMER, GetRecipeType(RecipeType.HAMMER));
            _weaponsmithRecipeTypes.Add(RecipeType.HARPOON, GetRecipeType(RecipeType.HARPOON));
            _weaponsmithRecipeTypes.Add(RecipeType.MACE, GetRecipeType(RecipeType.MACE));
            _weaponsmithRecipeTypes.Add(RecipeType.SHIELD, GetRecipeType(RecipeType.SHIELD));
            _weaponsmithRecipeTypes.Add(RecipeType.SWORD, GetRecipeType(RecipeType.SWORD));

            _disciplines = new Dictionary<String, Discipline>();
            _disciplines.Add(ARMORSMITH,
                new Discipline(
                    key: ARMORSMITH,
                    name_de: "Waffenschmied",
                    name_en: "Armorsmith",
                    name_es: "Forjador de armaduras",
                    name_fr:"Fabricant d'armures",
                    path: Config.ARMORSMITH_ICON_URL,
                    recipeTypes:_armorsmithRecipeTypes));
            _disciplines.Add(ARTIFICER,
                new Discipline(
                    key: ARTIFICER,
                    name_de: "Konstrukteur",
                    name_en: "Artificer",
                    name_es: "Artificiero",
                    name_fr:"Artificier",
                    path: Config.ARTIFICER_ICON_URL, 
                    recipeTypes:_artificerRecipeTypes));
            _disciplines.Add(CHEF,
                new Discipline(
                    key: CHEF,
                    name_de: "Küchenmeister",
                    name_en: "Chef",
                    name_es: "Cocinero",
                    name_fr:"Maître-queux",
                    path: Config.CHEF_ICON_URL,
                    recipeTypes:_chefRecipeTypes));
            _disciplines.Add(HUNTSMAN,
                new Discipline(
                    key: HUNTSMAN,
                    name_de: "Waidmann",
                    name_en: "Huntsman",
                    name_es: "Cazador",
                    name_fr:"Chasseur",
                    path: Config.HUNTSMAN_ICON_URL,
                    recipeTypes:_huntsmanRecipeTypes));
            _disciplines.Add(JEWELER, 
                new Discipline(
                    key:JEWELER,
                    name_de: "Juwelier",
                    name_en: "Jeweler",
                    name_es: "Joyero",
                    name_fr:"Bijoutier",
                    path: Config.JEWELER_ICON_URL,
                    recipeTypes: _jewelerRecipeTypes));
            _disciplines.Add(LEATHERWORKER,
                new Discipline(
                    key: LEATHERWORKER,
                    name_de: "Lederer",
                    name_en: "Leahterworker",
                    name_es: "Peletero",
                    name_fr:"Travailleur du cuir",
                    path: Config.LEATHERWORKER_ICON_URL,
                    recipeTypes: _leatherworkerRecipeTypes));
            _disciplines.Add(SCRIBE,
                new Discipline(
                    key: SCRIBE,
                    name_de: "Scribe_De",
                    name_en: "Scribe",
                    name_es: "Scribe_Es",
                    name_fr: "Illustrateur",
                    path: Config.ScribeIconUrl,
                    recipeTypes: _scribeRecipeTypes));
            _disciplines.Add(TAILOR,
                new Discipline(
                    key: TAILOR,
                    name_de: "Schneider",
                    name_en: "Tailor",
                    name_es: "Sastre",
                    name_fr:"Tailleur",
                    path: Config.TAILOR_ICON_URL,
                    recipeTypes: _tailorRecipeTypes));
            _disciplines.Add(WEAPONSMITH,
                new Discipline(
                    key: WEAPONSMITH,
                     name_de: "Rüstungsschmied",
                     name_en: "Weaponsmith",
                     name_es: "Armero",
                     name_fr:"Fabricant d'armes",
                     path: Config.WEAPONSMITH_ICON_URL,
                     recipeTypes: _weaponsmithRecipeTypes));
        }
        private static Data.RecipeType GetRecipeType(string key)
        {
            Data.RecipeType rt = null;
            Data.RecipeType.RecipeTypes.TryGetValue(key, out rt);
            return rt;
        }
        public static Data.Discipline GetDiscipline(string key)
        {
            Data.Discipline r = null;
            Data.Discipline.Disciplines.TryGetValue(key, out r);
            return r;
        }

    }
}