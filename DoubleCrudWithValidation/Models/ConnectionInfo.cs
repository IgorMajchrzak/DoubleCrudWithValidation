using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoubleCrudWithValidation.Models
{
    public struct ConnectionInfo
    {
        public string DbType { get; set; }
        public string Uri { get; set; }
        public string Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
