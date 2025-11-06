using System;
using PokemonApi.Core.Model;

namespace PokemonApi.Core.Ports;

public interface IPokemonInfoGateway
{
    Task<PokemonInfo?> GetAsync(string pokemonName);
}
