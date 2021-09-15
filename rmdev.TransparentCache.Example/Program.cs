using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace AutoCache.Example
{
    class Program
    {
        static async Task Main(string[] args)
        {
            IServiceCollection services = new ServiceCollection();

            //services.AddScoped<ICarroRepository, CarroRepository>();     // <- Antes
            services.AddCachedScoped<ICarroRepository, CarroRepository>(); // <- Depois

            var serviceProvider = services.BuildServiceProvider();

            var stopwatch = new Stopwatch();


            var repository = serviceProvider.GetService<ICarroRepository>();
            Console.WriteLine("Buscando todos...");
            stopwatch.Start();
            var carros = await repository.GetAll();
            stopwatch.Stop();
            Console.WriteLine($"Encontrado {carros.Count()} carros em {stopwatch.ElapsedTicks} ticks.");

            Console.WriteLine("Buscando todos novamente...");
            stopwatch.Restart();
            carros = await repository.GetAll();
            stopwatch.Stop();
            Console.WriteLine($"Encontrado {carros.Count()} carros em {stopwatch.ElapsedTicks} ticks.");


            //Console.WriteLine("Buscando o carro 1...");
            //stopwatch.Restart();
            //var carro = await repository.Get(1);
            //stopwatch.Stop();
            //Console.WriteLine($"Encontrado o carro {carro.Placa} em {stopwatch.ElapsedTicks} ticks.");


            //Console.WriteLine("Buscando o carro 1 novamente...");
            //stopwatch.Restart();
            //carro = await repository.Get(1);
            //stopwatch.Stop();
            //Console.WriteLine($"Encontrado o carro {carro.Placa} em {stopwatch.ElapsedTicks} ticks.");

            
            //Console.WriteLine("Buscando o carro 2...");
            //stopwatch.Restart();
            //carro = await repository.Get(2);
            //stopwatch.Stop();
            //Console.WriteLine($"Encontrado o carro {carro.Placa} em {stopwatch.ElapsedTicks} ticks.");


            //Console.WriteLine("Buscando o carro 2 novamente...");
            //stopwatch.Restart();
            //carro = await repository.Get(2);
            //stopwatch.Stop();
            //Console.WriteLine($"Encontrado o carro {carro.Placa} em {stopwatch.ElapsedTicks} ticks.");


            var rand = new Random((int)DateTime.Now.Ticks);
            for(int i = 0; i < 100; i++)
            {
                var c = rand.Next(1, 5);
                Console.Write($"Buscando o carro {c}... ");
                stopwatch.Restart();
                var carro = await repository.Get(c);
                stopwatch.Stop();
                Console.WriteLine($"Encontrado o carro {carro.Placa} em {stopwatch.ElapsedTicks} ticks.");
            }
        }
    }
}
