using System;
using System.Net;
using System.Runtime.Serialization.Json;

namespace ANet.WebAPI.Data
{
    class DataProvider
    {
        
        public static string GetBuild()
        {
            try
            {
                return DataProvider.GetString("https://api.guildwars2.com/v1/build.json");
            }
            catch
            {
                throw;
            }
        }
        public static String GetColors(string lang)
        {
            try
            {
                return DataProvider.GetString(
                    String.Format("https://api.guildwars2.com/v1/colors.json?lang={0}", lang));
            }
            catch
            {
                throw;
            }
        }
        public static String GetEventNames(string lang)
        {
            try
            {
                return DataProvider.GetString(
                    String.Format("https://api.guildwars2.com/v1/event_names.json?lang={0}", lang));
            }
            catch
            {
                throw;
            }
        }
        public static String GetEvents(string lang, int world_id)
        {
            try
            {
                return DataProvider.GetString(
                    String.Format("https://api.guildwars2.com/v1/events.json?lang={0}&world_id={1}", lang, world_id));
            }
            catch
            {
                throw;
            }
        }
        public static byte[] GetFile(string signature, int id, string format)
        {
            try
            {
                // https://render.guildwars2.com/file/{signature}/{id}.{format}
                return DataProvider.GetBytes(
                    String.Format("https://render.guildwars2.com/file/{0}/{1}.{2}", signature, id, format));
            }
            catch
            {
                throw;
            }
        }

        public static string GetGuildDetails(string lang, string guild_name)
        {
            try
            {
                return DataProvider.GetString(
                    String.Format("https://api.guildwars2.com/v1/guild_details.json?guild_name={0}&lang={1}", guild_name, lang));
            }
            catch
            {
                throw;
            }
        }
        public static string GetItemDetails(string lang, int item_id)
        {
            try
            {
                return DataProvider.GetString(
                    String.Format("https://api.guildwars2.com/v1/item_details.json?item_id={0}&lang={1}", item_id, lang));
            }
            catch
            {
                throw;
            }
        }
        public static string GetItems()
        {
            try
            {
                return DataProvider.GetString("https://api.guildwars2.com/v1/items.json");
            }
            catch
            {
                throw;
            }
        }
        public static String GetMapNames(string lang)
        {
            try
            {
                return DataProvider.GetString(
                    String.Format("https://api.guildwars2.com/v1/map_names.json?lang={0}", lang));
            }
            catch
            {
                throw;
            }
        }
        public static String GetRecipeDetails(string lang, int recipe_id)
        {
            try
            {
                // https://api.guildwars2.com/v1/recipes.json
                return DataProvider.GetString(
                    String.Format("https://api.guildwars2.com/v1/recipe_details.json?recipe_id={0}&lang={1}", recipe_id, lang));
            }
            catch
            {
                throw;
            }
        }
        public static string GetRecipes()
        {
            try
            {
                return DataProvider.GetString("https://api.guildwars2.com/v1/recipes.json");
            }
            catch
            {
                throw;
            }
        }
        public static String GetWorldNames(string lang)
        {
            try
            {
                return DataProvider.GetString(
                    String.Format("https://api.guildwars2.com/v1/world_names.json?lang={0}", lang));
            }
            catch
            {
                throw;
            }
        }
        public static String GetWvwMatcheDetails(string lang, string match_id)
        {
            try
            {
                return DataProvider.GetString(
                    String.Format("https://api.guildwars2.com/v1/wvw/match_details.json?lang={0}&match_id={1}", lang, match_id));
            }
            catch
            {
                throw;
            }
        }
        public static String GetWvwMatches()
        {
            try
            {
                return DataProvider.GetString("https://api.guildwars2.com/v1/wvw/matches.json");
            }
            catch
            {
                throw;
            }
        }
        public static String GetWvwObjectiveNames(string lang)
        {
            try
            {
                return DataProvider.GetString(
                    String.Format("https://api.guildwars2.com/v1/wvw/objective_names.json?lang={0}", lang));
            }
            catch
            {
                throw;
            }
        }

        private static string GetString(string url)
        {
            HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(url);
			httpRequest.Method = "GET";
			httpRequest.Timeout = 10000;
			httpRequest.Proxy = null;
            using (HttpWebResponse response = (HttpWebResponse)httpRequest.GetResponse())
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    using (System.IO.StreamReader sr = new System.IO.StreamReader(response.GetResponseStream()))
                    {
                        return sr.ReadToEnd();
                    }
                }
                else
                {
                    return null;
                }
            }
        }
        private static byte[] GetBytes(string url)
        {
            try
            {
                HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(url);
                httpRequest.Method = "GET";
                httpRequest.Timeout = 10000;
                httpRequest.Proxy = null;
                using (HttpWebResponse response = (HttpWebResponse)httpRequest.GetResponse())
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        byte[] buffer = new byte[16 * 1024];
                        System.IO.Stream s = response.GetResponseStream();
                        using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                        {
                            int read;
                            while ((read = s.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                ms.Write(buffer, 0, read);
                            }
                            return ms.ToArray();
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch
            {
                throw;
            }
        }

    }
}
