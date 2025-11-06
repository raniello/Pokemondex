using PokemonApi.Core.Model;

namespace PokemonApi.Core.Services;

public interface IPokemonTranslationService
{
    Task<PokemonInfo?> GetTranslatedInfoAsync(string pokemonName);
}
