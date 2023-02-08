using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoubleCrudWithValidation.Interfaces
{
    internal interface IValidatable
    {
        public string Error
        {
            get;
        }
    }
}
