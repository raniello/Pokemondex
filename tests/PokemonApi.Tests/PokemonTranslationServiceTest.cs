using System;
using FluentAssertions;
using Moq;
using PokemonApi.Core.Model;
using PokemonApi.Core.Ports;
using PokemonApi.Core.Services;

namespace PokemonApi.Tests;

public class PokemonTranslationServiceTest
{
    public static readonly PokemonInfo DefaultPokemonInfo = new()
    {
        Name = "Default Pokemon",
        Description = "Original Description",
        Habitat = "urban",
        IsLegendary = false
    };

    public static readonly PokemonInfo LegendaryPokemonInfo = new()
    {
        Name = "Legendary Pokemon",
        Description = "Original Description",
        Habitat = "rare",
        IsLegendary = true
    };

    public static readonly PokemonInfo CavePokemonInfo = new()
    {
        Name = "Cave Pokemon",
        Description = "Original Description",
        Habitat = "cave",
        IsLegendary = false
    };

    const string YodaTranslation = "Yoda Translation";
    const string ShakespeareTranslation = "Shakespeare Translation";
    private readonly IPokemonInfoGateway _pokemonInfoGateway;
    private readonly ITranslationGateway _translationGateway;
    private readonly PokemonTranslationService _sut;

    public PokemonTranslationServiceTest()
    {
        var pokemonInfoGatewayMock = new Mock<IPokemonInfoGateway>();
        pokemonInfoGatewayMock.Setup(m => m.GetAsync(DefaultPokemonInfo.Name)).Returns(Task.FromResult<PokemonInfo?>(DefaultPokemonInfo));
        pokemonInfoGatewayMock.Setup(m => m.GetAsync(LegendaryPokemonInfo.Name)).Returns(Task.FromResult<PokemonInfo?>(LegendaryPokemonInfo));
        pokemonInfoGatewayMock.Setup(m => m.GetAsync(CavePokemonInfo.Name)).Returns(Task.FromResult<PokemonInfo?>(CavePokemonInfo));
        _pokemonInfoGateway = pokemonInfoGatewayMock.Object;

        var translationGatewayMock = new Mock<ITranslationGateway>();
        translationGatewayMock.Setup(m => m.TranslateAsync(TranslationType.Yoda, It.IsAny<string>())).Returns(Task.FromResult(YodaTranslation));
        translationGatewayMock.Setup(m => m.TranslateAsync(TranslationType.Shakespeare, It.IsAny<string>())).Returns(Task.FromResult(ShakespeareTranslation));
        _translationGateway = translationGatewayMock.Object;

        _sut = new PokemonTranslationService(_pokemonInfoGateway, _translationGateway);

    }

    [Fact]
    public async Task SuccessfulShakeSpeareTranslation()
    {
        var translatedInfo = await _sut.GetTranslatedInfoAsync(DefaultPokemonInfo.Name);

        translatedInfo.Should().Be(DefaultPokemonInfo with { Description = ShakespeareTranslation });
    }


    [Fact]
    public async Task SuccessfulYodaTranslationForLegendaryPokemon()
    {
        var translatedInfo = await _sut.GetTranslatedInfoAsync(LegendaryPokemonInfo.Name);

        translatedInfo.Should().Be(LegendaryPokemonInfo with { Description = YodaTranslation });
    }
    
    [Fact]
    public async Task SuccessfulYodaTranslationForCavePokemon()
    {
        var translatedInfo = await _sut.GetTranslatedInfoAsync(CavePokemonInfo.Name);

        translatedInfo.Should().Be(CavePokemonInfo with { Description = YodaTranslation  });
    }
}
