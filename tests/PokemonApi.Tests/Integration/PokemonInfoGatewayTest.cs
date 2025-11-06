using FluentAssertions;
using PokemonApi.Gateways;

namespace PokemonApi.Tests.Integration;

public class PokemonInfoGatewayTest
{
    [Fact]
    public async Task GetPockemonInformation()
    {
        const string pokemonName = "ditto";
        var sut = new PokemonInfoGateway("https://pokeapi.co/api/v2/pokemon-species");
        var pokemonInfo = await sut.GetAsync(pokemonName);
        pokemonInfo.Should().NotBeNull();
        pokemonInfo.Name.Should().Be(pokemonName);
    }
}
