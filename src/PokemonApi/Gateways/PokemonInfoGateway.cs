using System;
using System.Text.Json.Serialization;
using PokemonApi.Core.Model;
using PokemonApi.Core.Ports;

namespace PokemonApi.Gateways;

public class PokemonInfoGateway(string pockemonSpeciesUri): IPokemonInfoGateway
{
    private readonly HttpClient _httpClient = new();
    private readonly Uri _pockemonSpeciesUri = new(pockemonSpeciesUri.TrimEnd('/') + "/");

    public async Task<PokemonInfo?> GetAsync(string pokemonName)
    {
        var uri = new Uri(_pockemonSpeciesUri, pokemonName);
        var response = await _httpClient.GetAsync(uri);
        response.EnsureSuccessStatusCode();

        var dto = await response.Content.ReadFromJsonAsync<PockemonSpeciesDto>();
        return dto != null ? new PokemonInfo
        {
            Name = dto.Name,
            Description = dto.FlavourTextEntries.Where(e => e.Language.Name == "en").Select(e => e.Text).First(),
            Habitat = dto.Habitat.Name,
            IsLegendary = dto.IsLegendary
        } : null;
    }

    class PockemonSpeciesDto
    {
        public required string Name { get; init; }
        public required NamedEntry Habitat { get; init; }

        [JsonPropertyName("is_legendary")]
        public required bool IsLegendary { get; init; }

        [JsonPropertyName("flavor_text_entries")]
        public required IReadOnlyList<FlavourTextEntryDto> FlavourTextEntries { get; init; }
    }


    class FlavourTextEntryDto
    {
        [JsonPropertyName("flavor_text")]
        public required string Text { get; init; }
        public required NamedEntry Language { get; init; }
    }

    class NamedEntry
    {
        public required string Name { get; init; }
    }

}
