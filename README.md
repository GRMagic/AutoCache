## AutoCache

O rvdev.autocache é um pacote que visa facilitar a implementação de cache em memória adicionando apenas um attribute no método que será cacheado.

## Como usar
#### Adicionar o pacote
`dotnet add package rmdev.autocache`

Você vai precisar adicionar o **using AutoCache**.

#### Na configuração dos serviços:
```csharp 
//services.AddScoped<ICarroRepository, CarroRepository>();     // <- Trocar essa linha
services.AddCachedScoped<ICarroRepository, CarroRepository>(); // <- Por esta linha
```

#### Na implementação original do serviço:
```csharp 
public class CarroRepository : ICarroRepository
    {
        [Cache(Seconds = 10)] // <- Adicionar essa linha, apenas os metodos com esse atributo serão cacheados
        public async Task<Carro> Get(int id)
        {
            // ...
        }
    }
```
