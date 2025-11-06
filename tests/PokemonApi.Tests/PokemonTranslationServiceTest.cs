using System;
using FluentAssertions;
using Moq;
using PokemonApi.Core.Model;
using PokemonApi.Core.Ports;
using PokemonApi.Core.Services;

namespace PokemonApi.Tests;

public class PokemonTranslationServiceTest
{
    private static readonly PokemonInfo TestPokemonInfo = new()
    {
        Name = "Test Pokemon",
        Description = "Description",
        Habitat = "test",
        IsLegendary = false
    };

    const string TranslatedDescription = "Translation";

    private readonly IPokemonInfoGateway _pokemonInfoGateway;
    private readonly ITranslationGateway _translationGateway;

    public PokemonTranslationServiceTest()
    {
        var pokemonInfoGatewayMock = new Mock<IPokemonInfoGateway>();
        pokemonInfoGatewayMock.Setup(m => m.GetAsync(TestPokemonInfo.Name)).Returns(Task.FromResult<PokemonInfo?>(TestPokemonInfo));
        _pokemonInfoGateway = pokemonInfoGatewayMock.Object;

        var translationGatewayMock = new Mock<ITranslationGateway>();
        translationGatewayMock.Setup(m => m.TranslateAsync(It.IsAny<string>())).Returns(Task.FromResult(TranslatedDescription));
        _translationGateway = translationGatewayMock.Object;
    }


    [Fact]
    public async Task SuccessfulTranslation()
    {
        var sut = new PokemonTranslationService(_pokemonInfoGateway, _translationGateway);

        var translatedInfo = await sut.GetTranslatedInfoAsync(TestPokemonInfo.Name);

        translatedInfo.Should().Be(TestPokemonInfo with { Description = TranslatedDescription});
    }
}
