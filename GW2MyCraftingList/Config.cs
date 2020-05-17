using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GW2ExplorerCraftTool
{
    class Config
    {
        public const string DEFAULT_ERROR_CODE = "999";

        // HIERARCHY BETWEEN INGREDIENTS
        public const string HIERARCHIC_INGREDIENT_URL = "http://www.gw2explorer.fr/images/hierarchic.png";
        // COINS ICONS
        public const string COPPER_ICON_URL = "https://wiki.guildwars2.com/images/e/eb/Copper_coin.png";
        public const string GOLD_ICON_URL = "https://wiki.guildwars2.com/images/d/d1/Gold_coin.png";
        public const string SILVER_ICON_URL = "https://wiki.guildwars2.com/images/3/3c/Silver_coin.png";
        public const string TRADING_POST_ICON_URL = "https://wiki.guildwars2.com/images/thumb/b/be/Black_Lion_Trading_Company_trading_post_icon.png/20px-Black_Lion_Trading_Company_trading_post_icon.png";
        // disciplines icons
        public const string ARMORSMITH_ICON_URL = "https://wiki.guildwars2.com/images/3/32/Armorsmith_tango_icon_20px.png";
        public const string ARTIFICER_ICON_URL = "https://wiki.guildwars2.com/images/b/b7/Artificer_tango_icon_20px.png";
        public const string CHEF_ICON_URL = "https://wiki.guildwars2.com/images/8/8f/Chef_tango_icon_20px.png";
        public const string HUNTSMAN_ICON_URL = "https://wiki.guildwars2.com/images/f/f3/Huntsman_tango_icon_20px.png";
        public const string JEWELER_ICON_URL = "https://wiki.guildwars2.com/images/f/f2/Jeweler_tango_icon_20px.png";
        public const string LEATHERWORKER_ICON_URL = "https://wiki.guildwars2.com/images/e/e5/Leatherworker_tango_icon_20px.png";
        public const string ScribeIconUrl = "https://wiki.guildwars2.com/images/0/0b/Scribe_tango_icon_20px.png";
        public const string TAILOR_ICON_URL = "https://wiki.guildwars2.com/images/4/4d/Tailor_tango_icon_20px.png";
        public const string WEAPONSMITH_ICON_URL = "https://wiki.guildwars2.com/images/4/46/Weaponsmith_tango_icon_20px.png";

        private static string _AssetIconFormat = "jpg";
        public static string AssetIconFormat
        {
            get { return _AssetIconFormat; }
        }

        public static Data.Language Language
        {
            get
            {
                return Data.Language.GetLanguage(Config.Lang);
            }
        }
        public static string Lang
        {
            get { return Properties.Settings.Default.lang; }
            set { Properties.Settings.Default.lang = value; }
        }

        public static System.Globalization.CultureInfo CultureInfo
        {
            get
            {
                //Data.Language l = Data.Language.Languages
                //    .Where((p) => { return p.Label.ToLower() == Config.Lang.ToLower(); }).Single();
                return new System.Globalization.CultureInfo(Config.Language.GlobalizationCode);
            }
        }

        public static string GetLocalizedName(Data.ILocalizable localizable)
        {
            switch (Config.CultureInfo.Name)
            {
                case Data.Language.DE: return localizable.Name_De;
                case Data.Language.EN: return localizable.Name_En;
                case Data.Language.ES: return localizable.Name_Es;
                case Data.Language.FR: return localizable.Name_Fr;
                default: return localizable.Name_En;
            }
        }

        public static void Save()
        {
            Properties.Settings.Default.Save();
        }
    }
}
