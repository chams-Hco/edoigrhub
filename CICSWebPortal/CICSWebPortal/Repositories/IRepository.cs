using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CICSWebPortal.Repositories
{
   public interface IRepository<T> where T:class
    {
        IEnumerable<T> GetAll();
        T Find(int id);
        void Add(T entity);
        void Attach(T entity);
        void Delete(T entity);
    }
}
