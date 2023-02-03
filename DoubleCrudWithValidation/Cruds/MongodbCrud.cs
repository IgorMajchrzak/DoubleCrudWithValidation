using DoubleCrudWithValidation.Interfaces;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoubleCrudWithValidation.Cruds
{
    internal class MongodbCrud : IDbCrud
    {
        private readonly MongoClient _client;

        public MongodbCrud(string connection)
        {
            _client = new MongoClient(connection);
        }

        public void Create<T>(T toCreate) where T : struct
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public List<T> Read<T>(int id)
        {
            throw new NotImplementedException();
        }

        public void Update<T>(T toUpdate) where T : struct
        {
            throw new NotImplementedException();
        }
    }
}
