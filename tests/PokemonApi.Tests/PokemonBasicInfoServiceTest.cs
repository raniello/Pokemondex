using System;
using FluentAssertions;
using Moq;
using PokemonApi.Core.Model;
using PokemonApi.Core.Ports;
using PokemonApi.Core.Services;

namespace PokemonApi.Tests;

public class PokemonBasicInfoServiceTest
{
    private static readonly PokemonInfo DefaultPokemonInfo = new()
    {
        Name = "Test Pokemon",
        Description = "Description",
        Habitat = "test",
        IsLegendary = false
    };
    
    private readonly IPokemonInfoGateway _pokemonInfoGateway;
    private readonly PokemonBasicInfoService _sut;

    public PokemonBasicInfoServiceTest()
    {
        var mock = new Mock<IPokemonInfoGateway>();
        mock.Setup(m => m.GetAsync(It.IsAny<string>())).Returns(Task.FromResult<PokemonInfo?>(null));
        mock.Setup(m => m.GetAsync(DefaultPokemonInfo.Name)).Returns(Task.FromResult<PokemonInfo?>(DefaultPokemonInfo));
        _pokemonInfoGateway = mock.Object;
        _sut = new PokemonBasicInfoService(_pokemonInfoGateway);
    }

    [Fact]
    public async Task SuccessfulBasicInformation()
    {
        var response = await _sut.GetBasicInfoAsync(DefaultPokemonInfo.Name);

        response.Should().Be(DefaultPokemonInfo);
    }

 [Fact]
    public async Task NotFound()
    {
        var response = await _sut.GetBasicInfoAsync("NotExisting");

        response.Should().BeNull();
    }
}
