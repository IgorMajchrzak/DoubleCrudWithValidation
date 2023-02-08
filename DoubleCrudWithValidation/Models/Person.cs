using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoubleCrudWithValidation.Interfaces;
using DoubleCrudWithValidation.Validators;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;

namespace DoubleCrudWithValidation.Models
{
    public class Person : IValidatable
    {
        private readonly PersonValidator _validator;

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }

        [BsonConstructor]
        public Person(string? id, string firstName, string lastName, int age)
        {
            _validator = new PersonValidator();
            if (id == null)
            {
                id = ObjectId.GenerateNewId().ToString();
            }
            Id = id ;
            FirstName = firstName;
            LastName = lastName;
            Age = age;
        }

        [BsonIgnore]
        public string Error
        {
            get
            {
                if (_validator != null)
                {
                    var results = _validator.Validate(this);
                    if (results != null && results.Errors.Any())
                    {
                        var errors = string.Join(Environment.NewLine, results.Errors.Select(x => x.ErrorMessage).ToArray());
                        return errors;
                    }
                }
                return string.Empty;
            }
        }
    }
}
