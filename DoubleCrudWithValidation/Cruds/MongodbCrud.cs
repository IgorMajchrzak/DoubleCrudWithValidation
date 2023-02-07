using DoubleCrudWithValidation.Interfaces;
using DoubleCrudWithValidation.Models;
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

        public void Create<T>(T toCreate) where T : class
        {
            var collection = _database.GetCollection<T>("People");
            collection.InsertOne(toCreate);
        }

        public void Delete(string idString)
        {
            var collection = _database.GetCollection<Person>("People");
            var filter = Builders<Person>.Filter.Eq("Id", idString);
            collection.DeleteOne(filter);
        }

        public List<T> Read<T>(string idString)
        {
            MongoCollectionBase<Person> peopleCollection = (MongoCollectionBase<Person>)_database.GetCollection<Person>("People");
            List<Person> result = new List<Person>();
            if (idString != "")
            {
                var filter = Builders<Person>.Filter.Eq("Id", idString);
                var document = peopleCollection.Find(filter).FirstOrDefault();
                result.Add(document);
            }
            else
            {
                var a = peopleCollection.Find(_ => true);
                result = a.ToList();
            }
            return result as List<T>;
        }
        
        public void Update<T>(T toUpdate) where T : class
        {
            if (toUpdate is Person updatedPerson)
            {
                var peopleCollection = _database.GetCollection<Person>("People");
                var filter = Builders<Person>.Filter.Eq("Id", updatedPerson.Id);
                peopleCollection.ReplaceOne(filter, updatedPerson);
            }

        }
    }
}
