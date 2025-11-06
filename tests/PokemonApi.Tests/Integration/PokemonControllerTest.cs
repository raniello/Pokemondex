using System;
using System.Net.Http.Json;
using FluentAssertions;
using Moq;
using PokemonApi.Core.Model;

namespace PokemonApi.Tests.Integration;

public class PokemonControllerTest(TestWebApplicationFactory factory) : IClassFixture<TestWebApplicationFactory>
{
      private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task BasicInfoRoute()
    {
        const string pokemonName = "ditto";
        var expectedPokemonInfo = new PokemonInfo
        {
            Name = pokemonName,
            Description = "Description",
            Habitat = "Habitat",
            IsLegendary = false
        };
        factory.BasicInfoServiceMock.Reset();
        factory.BasicInfoServiceMock.Setup(m => m.GetBasicInfoAsync(pokemonName)).Returns(Task.FromResult<PokemonInfo?>(expectedPokemonInfo));
        var response = await _client.GetAsync("/pokemon/" + pokemonName);
        response.EnsureSuccessStatusCode();

        var pokemonInfo = await response.Content.ReadFromJsonAsync<PokemonInfo>();

        pokemonInfo.Should().Be(expectedPokemonInfo);
    }

    [Fact]
    public async Task TranslatedRoot()
    {
        const string pokemonName = "ditto";
        var expectedPokemonInfo = new PokemonInfo
        {
            Name = pokemonName,
            Description = "Translated Description",
            Habitat = "Habitat",
            IsLegendary = false
        };
        factory.TranslationGatewayMock.Reset();
        factory.TranslationGatewayMock.Setup(m => m.GetTranslatedInfoAsync(pokemonName)).Returns(Task.FromResult<PokemonInfo?>(expectedPokemonInfo));
        var response = await _client.GetAsync("/pokemon/translated/" + pokemonName);
        response.EnsureSuccessStatusCode();

        var pokemonInfo = await response.Content.ReadFromJsonAsync<PokemonInfo>();

        pokemonInfo.Should().Be(expectedPokemonInfo);
    }
}
