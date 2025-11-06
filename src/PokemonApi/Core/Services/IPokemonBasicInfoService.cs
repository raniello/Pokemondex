using PokemonApi.Core.Model;

namespace PokemonApi.Core.Services;

public interface IPokemonBasicInfoService
{
    Task<PokemonInfo?> GetBasicInfoAsync(string pockemonName);
}
