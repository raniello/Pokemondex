using System;
using PokemonApi.Core.Model;
using PokemonApi.Core.Ports;

namespace PokemonApi.Core.Services;


public class PokemonTranslationService(IPokemonInfoGateway pokemonInfoGateway, ITranslationGateway translationGateway)
{
    private readonly IPokemonInfoGateway _pokemonInfoGateway = pokemonInfoGateway;
    private readonly ITranslationGateway _translationGateway = translationGateway;

    public async Task<PokemonInfo?> GetTranslatedInfoAsync(string pokemonName)
    {
        var basicInfo = await _pokemonInfoGateway.GetAsync(pokemonName);
        if (basicInfo == null)
        {
            return null;
        }
        var translationType = basicInfo.IsLegendary || basicInfo.Habitat == "cave" ? TranslationType.Yoda : TranslationType.Shakespeare;
        var translatedDescription = await _translationGateway.TranslateAsync(translationType, basicInfo.Description);
        return basicInfo with { Description = translatedDescription };
    }

}
