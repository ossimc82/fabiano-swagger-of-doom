using System;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace AdminPanel
{
    internal class Database
    {
        private readonly string connStr;
        private readonly MySqlConnection conn;

        public Database(string connStr)
        {
            this.connStr = connStr;
            this.conn = new MySqlConnection(this.connStr);
        }

        public Task OpenAsync()
        {
            return this.conn.OpenAsync();
        }

        public MySqlCommand CreateQuery(string query)
        {
            var cmd = conn.CreateCommand();
            cmd.CommandText = query;
            return cmd;
        }
        public Task CreateQueryAsync(string query, params KeyValuePair<string, object>[] args)
        {
            return Task.Factory.StartNew(() =>
            {
                var cmd = CreateQuery(query);
                foreach (var parameter in args)
                    cmd.Parameters.AddWithValue(parameter.Key, parameter.Value);
                cmd.ExecuteNonQuery();
            });
        }

        public static KeyValuePair<string, object> CreateParameter(string key, object value)
        {
            return new KeyValuePair<string, object>(key, value);
        }
    }
}