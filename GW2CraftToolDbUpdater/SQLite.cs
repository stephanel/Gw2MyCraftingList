using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Data;

// tutorial to start : http://www.dreamincode.net/forums/topic/157830-using-sqlite-with-c%23/
// http://www.techcoil.com/blog/my-experience-with-system-data-sqlite-in-c/
// about using LINQ : http://www.codeproject.com/Articles/236918/Using-SQLite-embedded-database-with-entity-framewo
// generating dbml file for sqlite : http://vashisthamohit.blogspot.fr/2011/02/linq-to-sql-for-sqlite.html

namespace GW2CraftToolDbUpdater.Data
{
    class SQLite
    {
          String dbConnection;

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

        public static List<DbLine> GetAllCheckedLines(string dbname)
        {
            try
            {
                SQLite db = new SQLite(dbname);
                DataTable recipe;
                String query = "select id, is_checked FROM Recipe WHERE is_checked<>0;";
                recipe = db.GetDataTable(query);

                List<DbLine> lines = new List<DbLine>();

                foreach (DataRow r in recipe.Rows)
                {
                    int id = Convert.ToInt32(r["id"]);
                    bool is_checked = (bool)r["is_checked"];
                    lines.Add(new DbLine(id, is_checked));
                }

                return lines;
            }
            catch
            {
                throw;
            }
        }
        public static void UpdateLine(string inputFile, int id)
        {
            try
            {
                string connectionString = String.Format(@"Data Source={0}", inputFile);
                using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();

                    string query = null;
                    // so, update data
                    query = 
                        String.Format("update [recipe] set [is_checked]=1 where [id]={0}", id);

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
