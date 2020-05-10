using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirLinesManager.DAO
{
    public interface IBasicDAO<T> where T : IPoco
    {
        T Get(long id);
        T Add(T toAdd);
        List<T> GetAll();
        void Remove(T toRemove);
        void Update(T toUpdate);
    }
}
