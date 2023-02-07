using DoubleCrudWithValidation.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoubleCrudWithValidation.StringBuilders
{
    internal class MysqlConnectionStringBuilder : IConnectionStringBuilder
    {
        private string _connectionString = "";
        public MysqlConnectionStringBuilder(string uri, string port, string username, string password)
        {
            _connectionString = $"datasource={uri};port={port};username={username};password={password}";
        }

        public string ConnectionString()
        {
            return _connectionString;
        }
    }
}
