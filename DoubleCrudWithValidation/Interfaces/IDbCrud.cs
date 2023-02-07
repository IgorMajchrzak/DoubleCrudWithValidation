using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoubleCrudWithValidation.Interfaces
{
    internal interface IDbCrud
    {
        public void Create<T>(T toCreate) where T : class;

        public List<T> Read<T>(string idString);

        public void Update<T>(T toUpdate) where T : class;

        public void Delete(string idString);
    }
}
