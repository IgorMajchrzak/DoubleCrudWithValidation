using DoubleCrudWithValidation.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoubleCrudWithValidation.Validators
{
    internal class ProductValidator : AbstractValidator<Product>
    {
        public ProductValidator()
        {
            RuleFor(p => p.Name).NotEmpty().MaximumLength(200);
            RuleFor(p => p.Price).GreaterThanOrEqualTo(0).PrecisionScale(6,2,true);
            RuleFor(p => p.NumberInStock).GreaterThanOrEqualTo(0);
        }
    }
}
