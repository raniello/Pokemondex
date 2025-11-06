using System;
using FluentAssertions;
using PokemonApi.Core.Ports;
using PokemonApi.Gateways;

namespace PokemonApi.Tests.Integration;

public class FunTranslationGatewayTest
{
    private const string FunTranslationTestUri = "https://api.funtranslations.com/translate";
    private const string OriginalText = "Hello, my name is Pikachu";
    private FunTranslationGateway _sut = new (FunTranslationTestUri);

    [Fact]
    public async Task YodaTranslation()
    {
        var translation = await _sut.TranslateAsync(TranslationType.Yoda, OriginalText);
        translation.Should().NotBeNullOrWhiteSpace();
        translation.Should().NotBe(OriginalText);
    }


    [Fact]
    public async Task ShakespeareTranslation()
    {
        var translation = await _sut.TranslateAsync(TranslationType.Shakespeare, OriginalText);
        translation.Should().NotBeNullOrWhiteSpace();
        translation.Should().NotBe(OriginalText);
    }
}
