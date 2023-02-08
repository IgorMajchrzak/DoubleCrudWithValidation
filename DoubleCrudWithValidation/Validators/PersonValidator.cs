using DoubleCrudWithValidation.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoubleCrudWithValidation.Validators
{
    internal class PersonValidator : AbstractValidator<Person>
    {
        public PersonValidator()
        {
            RuleFor(p => p.FirstName).NotEmpty().MinimumLength(2).MaximumLength(30);
            RuleFor(p => p.LastName).NotEmpty().MinimumLength(2).MaximumLength(60);
            RuleFor(p => p.Age).GreaterThan(0);
        }
    }
}
