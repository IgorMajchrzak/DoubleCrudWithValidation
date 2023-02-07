using DoubleCrudWithValidation.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoubleCrudWithValidation.StringBuilders
{
    internal class MongodbConnectionStringBuilder : IConnectionStringBuilder
    {
        private string _connectionString = "";
        public MongodbConnectionStringBuilder(string uri, string port, string username, string password)
        {
            if (port != "")
            {
                _connectionString = "mongodb://";
            }
            else
            {
                _connectionString = "mongodb+srv://";
            }
            if (username != "")
            {
                _connectionString += username;
                if (password != "")
                {
                    _connectionString += $":{password}";
                }
                _connectionString += $"@{uri}";
                if (port != "")
                {
                    _connectionString += $":{port}";
                }
                _connectionString += "/?authMechanism=DEFAULT";
            }
            else
            {
                _connectionString += $"{uri}";
                if (port != "")
                {
                    _connectionString += $":{port}";
                }
                _connectionString += "/";
            }
        }

        public string ConnectionString()
        {
            return _connectionString;
        }
    }
}
