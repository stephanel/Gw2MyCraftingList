using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GW2ExplorerCraftTool
{
    class ErrorExportHelper
    {
        public static void ExportErrorToJson(List<Data.Recipe> recipes, string filepath)
        {
            try
            {
                using (System.IO.FileStream fs = System.IO.File.Create(filepath))
                {
                    fs.Close();
                }

                using (System.IO.StreamWriter sw = new System.IO.StreamWriter(filepath))
                {
                    //string text = null;
                    //int[] ints =recipes.Select<Data.Recipe, int>((p) => { return int.Parse(p.AnetRecipe.recipe_id); }).ToArray();
                    //text = String.Join(",", ints);
                    foreach(Data.Recipe r in recipes )
                    {
                        if (!String.IsNullOrEmpty(r.AnetRecipe.error))
                        {
                            sw.Write(String.Format("Recipe {0} : {1}{2}", r.Id, r.AnetRecipe.text, Environment.NewLine));
                        }
                        else
                        {
                            Data.API.ANet.Item item = r.Item;
                            if (!String.IsNullOrEmpty(item.error))
                            {
                                sw.Write(String.Format("Item {0} : {1}{2}", r.OutputItemId, item.text, Environment.NewLine));
                            }
                        }
                        if (r.Ingredients != null && r.Ingredients.Count() > 0)
                        {
                            foreach (Data.Ingredient i in r.Ingredients)
                            {
                                if (i.AnetItem!=null && !String.IsNullOrEmpty(i.AnetItem.error))
                                {
                                    sw.Write(String.Format("Item {0} : {1}{2}", i.Id, i.AnetItem.text, Environment.NewLine));
                                }

                            }
                        }

                    }

                }
            
            } 
            catch(Exception)
            {
                // do nothing
            }
        }
    }
}
