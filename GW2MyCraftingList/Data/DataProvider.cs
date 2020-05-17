using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Diagnostics;
using System.IO;

namespace GW2ExplorerCraftTool.Data
{
    class DataProvider
    {
        public static API.Gw2Spidy.Item GetGw2SpidyItem(int item_id)
        {
            API.Gw2Spidy.Item item = null;
            try
            {
                item = DataProvider<API.Gw2Spidy.Item>.GetData(
                    String.Format("http://www.gw2spidy.com/api/v0.9/json/item/{0}", item_id));
            }
            catch(Exception ex)
            {
                if(item==null)
                    item = new API.Gw2Spidy.Item();
                item.exception = ex; 
            }
            return item;
        }
        public static API.ANet.Item GetANetItem(int item_id, string lang)
        {
            try
            {
                return DataProvider<API.ANet.Item>.GetData(
                    String.Format("https://api.guildwars2.com/v1/item_details.json?item_id={0}&lang={1}", item_id, lang));
            }
            catch
            {
                throw;
            }
        }
        public static API.ANet.Recipe GetANetRecipe(int recipe_id, string lang)
        {
            API.ANet.Recipe recipe = null;
            try
            {
                      recipe = DataProvider<API.ANet.Recipe>.GetData(
                    String.Format("https://api.guildwars2.com/v1/recipe_details.json?recipe_id={0}&lang={1}", recipe_id, lang));
            }
            catch (Exception ex)
            {
                if (recipe == null)
                {
                    recipe = new API.ANet.Recipe();
                    recipe.recipe_id = recipe_id.ToString();
                    recipe.error = Config.DEFAULT_ERROR_CODE;
                    recipe.text = ex.Message;
                }
            }
            if (recipe.error == null)
            {
                API.ANet.Item item = null;
                string output_item_id = recipe.output_item_id;
                try
                {
                    // get correspondig item
                    item = GetANetItem(int.Parse(output_item_id), lang);
#if DEBUG
                    if (!API.ANet.ItemType.Values.Contains(item.type))
                    {
                        Console.WriteLine("New item type \"{0}\": {1}", item.type, output_item_id);
                    }
#endif
                    foreach (API.ANet.Ingredient ingredient in recipe.ingredients)
                    {
                        API.ANet.Item item2 = null;
                        try
                        {
                            item2 = GetANetItem(int.Parse(ingredient.item_id), lang);
                        }
                        catch (Exception ex)
                        {
                            if (item2 == null)
                            {
                                item2 = new API.ANet.Item();
                                item2.item_id = ingredient.item_id;
                                item2.error = Config.DEFAULT_ERROR_CODE;
                                item2.text = ex.Message;
                            }
                        }
                        finally
                        {
                            if (item2 != null)
                                ingredient.Item = item2;
                        }
                    }

                }
                catch (Exception ex)
                {
                    if (item == null)
                    {
                        item = new API.ANet.Item();
                        item.item_id = output_item_id;
                        item.error = Config.DEFAULT_ERROR_CODE;
                        item.text = ex.Message;
                    }
                }
                finally
                {
                    if(recipe!=null && item!=null)
                        recipe.Item = item;

                }
            }
            return recipe;
        }
        public static List<int> GetRecipeIDs()
        {
            try
            {
                API.ANet.Recipes result = DataProvider<API.ANet.Recipes>.GetData("https://api.guildwars2.com/v1/recipes.json");
                return result.recipes;
            }
            catch
            {
                throw;
            }
        }
       
        /*private static byte[] GetBytes(string url)
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
                        return default(T);
                    }
                }
            }
            catch
            {
                throw;
            }
        }*/

    }

    class DataProvider<T>
    {
        public static T GetData(string url)
        {

            HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(url);
			httpRequest.Method = "GET";
			httpRequest.Timeout = 10000;
			httpRequest.Proxy = null;

            Stopwatch sw = new Stopwatch();
            sw.Start();
            
            using (HttpWebResponse response = (HttpWebResponse)httpRequest.GetResponse())
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    sw.Stop();

                    //using(var stream = response.GetResponseStream())
                    //{
                    //    using(var ms = new MemoryStream())
                    //    {
                    //        stream.CopyTo(ms);
                    //        var data = ms.ToArray();
                    //        var content = System.Text.Encoding.UTF8.GetString(data);
                    //        Console.WriteLine(content);
                    //        return Serializer<T>.Deserialize(content);
                    //    }
                    //}
                    return Serializer<T>.Deserialize(response.GetResponseStream());
                }
                else
                {
                    sw.Stop();
                    return default(T);
                }
            }
        }
    }

}
