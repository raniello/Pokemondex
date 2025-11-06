using System;
using PokemonApi.Core.Model;
using PokemonApi.Core.Ports;

namespace PokemonApi.Core.Services;

public class PokemonBasicInfoService(IPokemonInfoGateway pokemonInfoGateway) : IPokemonBasicInfoService
{
    private readonly IPokemonInfoGateway _pokemonInfoGateway = pokemonInfoGateway;

    public Task<PokemonInfo?> GetBasicInfoAsync(string pockemonName)
    {
        return _pokemonInfoGateway.GetAsync(pockemonName);
    }
}
