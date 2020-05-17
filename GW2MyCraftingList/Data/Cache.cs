using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
//using System.ComponentModel;
using System.Threading;

namespace GW2ExplorerCraftTool.Data
{
    public class Cache
    {
        private static long _session_id;

        public static long SessionID
        {
            get { return Cache._session_id; }
        }

        private static Cache _cache;
        private static string _lang = Language.EN;

        private string _directoryname = null;
        private string _recipesDbFileName;
        private string _recipesDbCopyName;
        

        private List<Data.Recipe> _recipes = new List<Data.Recipe>();
        
        public List<Data.Recipe> Recipes
        {
            get
            {
                return _cache._recipes;
            }
        }

        private Cache(){ }

        public static void Create(string lang) {
            if(_cache==null)
            {
                _lang = lang;
                _session_id = GetSessionID();
                _cache = new Cache();
                _cache._directoryname = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data");
                _cache._recipesDbFileName = String.Format("data/{0}/recipes.s3db", lang);
                _cache._recipesDbCopyName = Path.Combine(_cache._directoryname, String.Format("{0}/recipes.s3db.bak", lang));

                _cache.LoadRecipes();

            }
        }

        public Data.Recipe AddRecipe(API.ANet.Recipe recipe)
        {
            if (this._recipes.Where(r => r.Id!=null && r.Id == int.Parse(recipe.recipe_id)).Count() == 0)
            {
                Data.Recipe newrecipe = new Data.Recipe(recipe, false, false, _session_id);
                this._recipes.Add(newrecipe);
                this.SaveRecipe(newrecipe);

                return newrecipe;
            }
            return null;
        }
        public void DeleteRecipesInError()
        {
            //ThreadPool.QueueUserWorkItem(new WaitCallback(_DeleteRecipesInError));
            SQLite.DeleteRecipesInError(_cache._recipesDbFileName);
        }
        public static Data.Cache GetCache()
        {
            return _cache;
        }
        public List<Data.Recipe> GetRecipesInError()
        {
            try
            {
                //return this._recipes.Where((p) =>{ return p.InError || p.IngredientsInError; }).ToList();
                return SQLite.GetRecipesInError(_cache._recipesDbFileName);
            }
            catch
            {
                throw;
            }
        }
        public void LoadRecipes()
        {
            try
            {
                /* using SQLite DB file */
                if (!File.Exists(_cache._recipesDbFileName))
                {
                    // first, check if directory corresponding to the current lang exists. If not, create it
                    string dir = Path.Combine(_cache._directoryname, _lang);
                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }

                    SQLite.CreateNew(_cache._recipesDbFileName);
                }
                this._recipes = SQLite.GetRecipes(_cache._recipesDbFileName);
            }
            catch
            {
                throw;
            }
        }
        public void SaveRecipe(Recipe recipe)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(_SaveRecipe), recipe);
        }

        /* Internal */
        private long _recipesCounter = 0;
        internal static long GetSessionID()
        {
            try
            {
                // yyyyMMddHHmmssfff
                string dt = String.Format("{0:yyyy}{0:MM}{0:dd}{0:HH}{0:mm}{0:ss}", DateTime.Now);
                return long.Parse(dt);
            }
            catch
            {
                throw;
            }
        }
        //internal void _DeleteRecipesInError(object state)
        //{
        //    SQLite.DeleteRecipesInError(_cache._recipesDbFileName);
        //}
        internal void _SaveRecipe(object state)
        {
            ++_recipesCounter;
            Recipe recipe = (Recipe)state;
            SQLite.SaveRecipe(
                _cache._recipesDbFileName, 
                (int)recipe.Id, 
                Serializer<API.ANet.Recipe>.Serialize(recipe.AnetRecipe),
                recipe.Is_checked, 
                recipe.Is_saved,
                _session_id);
            if (_recipesCounter % 100 == 0)
            {
                // create a backup every 100 recipes
                System.IO.File.Copy(this._recipesDbFileName, this._recipesDbCopyName, true);
            }
            recipe.Is_saved = true;
        }

    }
}
