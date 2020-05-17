using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Data;

// tutorial to start : http://www.dreamincode.net/forums/topic/157830-using-sqlite-with-c%23/
// http://www.techcoil.com/blog/my-experience-with-system-data-sqlite-in-c/
// about using LINQ : http://www.codeproject.com/Articles/236918/Using-SQLite-embedded-database-with-entity-framewo
// generating dbml file for sqlite : http://vashisthamohit.blogspot.fr/2011/02/linq-to-sql-for-sqlite.html

namespace GW2ExplorerCraftTool.Data
{
    class SQLite
    {
          String dbConnection;

        ///// <summary>
        /////     Constructor with no parameter.
        ///// </summary>
        //public SQLite()
        //{
        //    dbConnection = "Data Source=gw2data.s3db;Version=3;";
        //}

        /// <summary>
        ///     Single Param Constructor for specifying the DB file.
        /// </summary>
        /// <param name="inputFile">The File containing the DB</param>
        public SQLite(String inputFile)
        {
            dbConnection = String.Format("Data Source={0};Version=3;", inputFile);
        }

        /// <summary>
        ///     Single Param Constructor for specifying advanced connection options.
        /// </summary>
        /// <param name="connectionOpts">A dictionary containing all desired options and their values</param>
        public SQLite(Dictionary<String, String> connectionOpts)
        {
            String str = "";
            foreach (KeyValuePair<String, String> row in connectionOpts)
            {
                str += String.Format("{0}={1}; ", row.Key, row.Value);
            }
            str = str.Trim().Substring(0, str.Length - 1);
            dbConnection = str;
        }

        /// <summary>
        ///     Allows the programmer to run a query against the Database.
        /// </summary>
        /// <param name="sql">The SQL to run</param>
        /// <returns>A DataTable containing the result set.</returns>
        internal DataTable GetDataTable(string sql)
        {
            try
            {
                using (var cnn = new SQLiteConnection(dbConnection))
                {
                    cnn.Open();
                    using (var mycommand = new SQLiteCommand(cnn))
                    {
                        mycommand.CommandText = sql;
                        using (var reader = mycommand.ExecuteReader())
                        {
                            DataTable dt = new DataTable();
                            dt.Load(reader);
                            reader.Close();
                            cnn.Close();
                            return dt;
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        ///     Allows the programmer to interact with the database for purposes other than a query.
        /// </summary>
        /// <param name="sql">The SQL to be run.</param>
        /// <returns>An Integer containing the number of rows updated.</returns>
        internal int ExecuteNonQuery(string sql)
        {
            using (var cnn = new SQLiteConnection(dbConnection))
            {
                cnn.Open();
                using (var mycommand = new SQLiteCommand(cnn))
                {
                    mycommand.CommandText = sql;
                    int rowsUpdated = mycommand.ExecuteNonQuery();
                    cnn.Close();
                    return rowsUpdated;
                }
            }
        }

        /// <summary>
        ///     Allows the programmer to retrieve single items from the DB.
        /// </summary>
        /// <param name="sql">The query to run.</param>
        /// <returns>A string.</returns>
        internal string ExecuteScalar(string sql)
        {
            using (var cnn = new SQLiteConnection(dbConnection))
            {
                cnn.Open();
                using (var mycommand = new SQLiteCommand(cnn))
                {
                    mycommand.CommandText = sql;
                    object value = mycommand.ExecuteScalar();
                    cnn.Close();
                    if (value != null)
                    {
                        return value.ToString();
                    }
                    return "";
                }
            }
        }

        /// <summary>
        ///     Allows the programmer to easily update rows in the DB.
        /// </summary>
        /// <param name="tableName">The table to update.</param>
        /// <param name="data">A dictionary containing Column names and their new values.</param>
        /// <param name="where">The where clause for the update statement.</param>
        /// <returns>A boolean true or false to signify success or failure.</returns>
        internal bool Update(String tableName, Dictionary<String, String> data, String where)
        {
            String vals = "";
            Boolean returnCode = true;
            if (data.Count >= 1)
            {
                foreach (KeyValuePair<String, String> val in data)
                {
                    vals += String.Format(" {0} = '{1}',", val.Key.ToString(), val.Value.ToString());
                }
                vals = vals.Substring(0, vals.Length - 1);
            }
            try
            {
                this.ExecuteNonQuery(String.Format("update {0} set {1} where {2};", tableName, vals, where));
            }
            catch
            {
                returnCode = false;
            }
            return returnCode;
        }

        /// <summary>
        ///     Allows the programmer to easily delete rows from the DB.
        /// </summary>
        /// <param name="tableName">The table from which to delete.</param>
        /// <param name="where">The where clause for the delete.</param>
        /// <returns>A boolean true or false to signify success or failure.</returns>
        internal void Delete(String tableName, String where)
        {
            try
            {
                this.ExecuteNonQuery(String.Format("delete from {0} where {1};", tableName, where));
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        ///     Allows the programmer to easily insert into the DB
        /// </summary>
        /// <param name="tableName">The table into which we insert the data.</param>
        /// <param name="data">A dictionary containing the column names and data for the insert.</param>
        /// <returns>A boolean true or false to signify success or failure.</returns>
        internal void Insert(String tableName, Dictionary<String, String> data)
        {
            String columns = "";
            String values = "";

            foreach (KeyValuePair<String, String> val in data)
            {
                columns += String.Format(" {0},", val.Key.ToString());
                values += String.Format(" '{0}',", val.Value);
            }
            columns = columns.Substring(0, columns.Length - 1);
            values = values.Substring(0, values.Length - 1);
            try
            {
                this.ExecuteNonQuery(String.Format("insert into {0} ({1}) values ({2});", tableName, columns, values));
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        ///     Allows the programmer to easily delete all data from the DB.
        /// </summary>
        /// <returns>A boolean true or false to signify success or failure.</returns>
        internal bool ClearDB()
        {
            DataTable tables;
            try
            {
                tables = this.GetDataTable("select NAME from SQLITE_MASTER where type='table' order by NAME;");
                foreach (DataRow table in tables.Rows)
                {
                    this.ClearTable(table["NAME"].ToString());
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        ///     Allows the user to easily clear all data from a specific table.
        /// </summary>
        /// <param name="table">The name of the table to clear.</param>
        /// <returns>A boolean true or false to signify success or failure.</returns>
        internal bool ClearTable(String table)
        {
            try
            {

                this.ExecuteNonQuery(String.Format("delete from {0};", table));
                return true;
            }
            catch
            {
                return false;
            }
        }


        public static void CreateNew(string inputFile)
        {
            try
            {
                string connectionString = String.Format(@"Data Source={0}", inputFile);
                using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();

                    // Define the SQL Create table statement
                    string query = "CREATE TABLE [recipe] (" +
                        "[id] INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL," +
                        "[json] TEXT NULL," +
                        "[is_checked] BOOLEAN NULL," +
                        "[session_id] BIGINT NULL" +
                        ")";

                    using (SQLiteTransaction sqlTransaction = conn.BeginTransaction())
                    {
                        // Create the table
                        SQLiteCommand createCommand = new SQLiteCommand(query, conn);
                        createCommand.ExecuteNonQuery();
                        createCommand.Dispose();

                        // Commit the changes into the database
                        sqlTransaction.Commit();
                    } // end using

                    // Close the database connection
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static void DeleteRecipesInError(string inputFile)
        {
            try
            {
                string connectionString = String.Format(@"Data Source={0}", inputFile);
                using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();

                    // Define the SQL Create table statement
                    String query = String.Format("DELETE FROM Recipe WHERE json LIKE '%\"error\":\"{0}\"%';", Config.DEFAULT_ERROR_CODE);

                    using (SQLiteTransaction sqlTransaction = conn.BeginTransaction())
                    {
                        // Create the table
                        SQLiteCommand createCommand = new SQLiteCommand(query, conn);
                        createCommand.ExecuteNonQuery();
                        createCommand.Dispose();

                        // Commit the changes into the database
                        sqlTransaction.Commit();
                    } // end using

                    // Close the database connection
                    conn.Close();
                }
            }
            catch
            {
                throw;
            }
        }
        public static List<Recipe> GetRecipes(string dbname)
        {
            try
            {
                SQLite db = new SQLite(dbname);
                DataTable recipe;
                String query = "select * FROM Recipe;";
                recipe = db.GetDataTable(query);

                List<Recipe> recipes = new List<Recipe>();

                foreach (DataRow r in recipe.Rows)
                {
                    string json = (string)r["json"];
                    bool is_checked = (bool)r["is_checked"];
                    long session_id = (long)r["session_id"];
                    API.ANet.Recipe anet_recipe = Serializer<API.ANet.Recipe>.Deserialize(json);
                    recipes.Add(new Recipe(anet_recipe, is_checked, true, session_id));
                }

                return recipes;
            }
            catch
            {
                throw;
            }
        }
        public static List<Recipe> GetRecipesInError(string dbname)
        {
            try
            {
                SQLite db = new SQLite(dbname);
                DataTable recipe;
                String query = String.Format("select * FROM Recipe WHERE json LIKE '%\"error\":\"{0}\"%';", Config.DEFAULT_ERROR_CODE);
                recipe = db.GetDataTable(query);

                List<Recipe> recipes = new List<Recipe>();

                foreach (DataRow r in recipe.Rows)
                {
                    string json = (string)r["json"];
                    bool is_checked = (bool)r["is_checked"];
                    long session_id = (long)r["session_id"];
                    API.ANet.Recipe anet_recipe = Serializer<API.ANet.Recipe>.Deserialize(json);
                    recipes.Add(new Recipe(anet_recipe, is_checked, true, session_id));
                }

                return recipes;
            }
            catch
            {
                throw;
            }
        }
        public static void SaveRecipe(string inputFile, int recipe_id, string json, bool is_checked, bool is_saved, long session_id)
        {
            try
            {
                string connectionString = String.Format(@"Data Source={0}", inputFile);
                using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();

                    string query = null;
                    if (is_saved)
                    {
                        // so, update data
                        query = 
                            String.Format("update [recipe] set [is_checked]={1} where [id]={0}",
                            recipe_id, 
                            (is_checked ? 1 : 0));
                    }
                    else
                    {
                        // insert data
                        query =
                            String.Format("insert into [recipe] ([id], [json], [is_checked], [session_id]) values ({0}, '{1}', {2}, {3})",
                            recipe_id,
                            json,
                            (is_checked ? 1 : 0),
                            session_id);
                    }

                    using (SQLiteTransaction sqlTransaction = conn.BeginTransaction())
                    {
                        // Create the table
                        SQLiteCommand createCommand = new SQLiteCommand(query, conn);
                        createCommand.ExecuteNonQuery();
                        createCommand.Dispose();

                        // Commit the changes into the database
                        sqlTransaction.Commit();
                    } // end using

                    // Close the database connection
                    conn.Close();
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
