using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoubleCrudWithValidation.Interfaces
{
    internal interface IDbCrud
    {
        public void Create<T>(T toCreate) where T : struct;

        public List<T> Read<T>(int id);

        public void Update<T>(T toUpdate) where T : struct;

        public void Delete(int id);
    }
}
