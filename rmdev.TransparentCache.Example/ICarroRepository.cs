using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutoCache.Example
{
    public interface ICarroRepository
    {
        Task<IEnumerable<Carro>> GetAll();
        Task<Carro> Get(int id);
    }
}
