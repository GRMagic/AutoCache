using AutoCache;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoCache.Example
{
    public class CarroRepository : ICarroRepository
    {
        private static Carro[] Carros = new[] {
            new Carro { Id = 1, Placa = "aaa1111" },
            new Carro { Id = 2, Placa = "bbb2222" },
            new Carro { Id = 3, Placa = "ccc3333" },
            new Carro { Id = 4, Placa = "ddd4444" }
        };

        [Cache(Seconds = 10)]
        public async Task<Carro> Get(int id)
        {
            await Task.Delay(5000);
            return Carros.FirstOrDefault(c => c.Id == id);
        }

        [Cache(Seconds = 10)]
        public async Task<IEnumerable<Carro>> GetAll()
        {
            await Task.Delay(5000);
            return Carros.AsEnumerable();
        }
    }
}
