using System;
using FluentAssertions;
using Moq;
using PokemonApi.Core.Model;
using PokemonApi.Core.Ports;
using PokemonApi.Core.Services;

namespace PokemonApi.Tests;

public class PokemonBasicInfoServiceTest
{
    private readonly IPokemonInfoGateway _pokemonInfoGateway;
    private static readonly PokemonInfo DefaultPokemonInfo = new()
    {
        Name = "Test Pokemon",
        Description = "Description",
        Habitat = "test",
        IsLegendary = false
    };
    public PokemonBasicInfoServiceTest()
    {
        var mock = new Mock<IPokemonInfoGateway>();
        mock.Setup(m => m.GetAsync(It.IsAny<string>())).Returns(Task.FromResult<PokemonInfo?>(null));
        mock.Setup(m => m.GetAsync(DefaultPokemonInfo.Name)).Returns(Task.FromResult<PokemonInfo?>(DefaultPokemonInfo));
        _pokemonInfoGateway = mock.Object;
    }

    
    [Fact]
    public async Task SuccessfulBasicInformation()
    {
        var sut = new PokemonBasicInfoService(_pokemonInfoGateway);
        var response = await sut.GetBasicInfoAsync(DefaultPokemonInfo.Name);

        response.Should().Be(DefaultPokemonInfo);
    }

}
