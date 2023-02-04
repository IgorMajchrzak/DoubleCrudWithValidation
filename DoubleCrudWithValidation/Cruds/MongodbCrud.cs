using DoubleCrudWithValidation.Interfaces;
using DoubleCrudWithValidation.Structs;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
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
            //needs testing
            var database = _client.GetDatabase("PeopleDb");
            var peopleCollection = database.GetCollection<BsonDocument>("People");
            var filter = Builders<BsonDocument>.Filter.Empty;
            var documents = peopleCollection.Find(filter).ToList();
            List<T> result = new List<T>();
            foreach (var document in documents)
            {
                result.Add(BsonSerializer.Deserialize<T>(document));
            }
            return result;
        }

        public void Update<T>(T toUpdate) where T : struct
        {
            throw new NotImplementedException();
        }
    }
}
