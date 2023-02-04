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
        private readonly IMongoDatabase _database;

        public MongodbCrud(string connection)
        {
            var client = new MongoClient(connection);
            _database = client.GetDatabase("PeopleDb");
        }

        public void Create<T>(T toCreate) where T : struct
        {
            var collection = _database.GetCollection<T>("People");
            collection.InsertOne(toCreate);
        }

        public void Delete(int id)
        {
            var collection = _database.GetCollection<Person>("People");
            var filter = Builders<Person>.Filter.Eq("PersonId", id);
            collection.DeleteOne(filter);
        }

        public List<T> Read<T>(int id)
        {
            //needs testing
            var peopleCollection = _database.GetCollection<Person>("People");
            List<Person> result = new List<Person>();
            if (id > 0)
            {
                var filter = Builders<Person>.Filter.Eq("PersonId", id);
                var document = peopleCollection.Find(filter).FirstOrDefault();
                result.Add(document);
            }
            else
            {
                var filter = Builders<Person>.Filter.Empty;
                result = peopleCollection.Find(filter).ToList();
            }
            return result as List<T>;
        }
        
        public void Update<T>(T toUpdate) where T : struct
        {
            if (toUpdate is Person updatedPerson)
            {
                var peopleCollection = _database.GetCollection<Person>("People");
                var filter = Builders<Person>.Filter.Eq("PersonId", updatedPerson.PersonId);
                peopleCollection.ReplaceOne(filter, updatedPerson);
            }

        }
    }
}
