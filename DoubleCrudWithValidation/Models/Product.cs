using DoubleCrudWithValidation.Interfaces;
using DoubleCrudWithValidation.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoubleCrudWithValidation.Models
{
    public class Product : IValidatable
    {
        private readonly ProductValidator _validator;
        
        public int ProductId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int NumberInStock { get; set; }

        public Product(int productId, string? name, string? description, decimal price, int numberInStock)
        {
            _validator = new ProductValidator();
            ProductId = productId;
            Name = name;
            Description = description;
            Price = price;
            NumberInStock = numberInStock;
        }

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
