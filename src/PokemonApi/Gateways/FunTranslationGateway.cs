using System;
using PokemonApi.Core.Ports;

namespace PokemonApi.Gateways;

public class FunTranslationGateway(string funTranslationUri) : ITranslationGateway
{
    private readonly HttpClient _httpClient = new();
    private readonly Uri _funTranslationUri = new(funTranslationUri.TrimEnd('/') + "/");
    public async Task<string> TranslateAsync(TranslationType type, string text)
    {
        var uri = new Uri(_funTranslationUri, $"{type.ToString().ToLowerInvariant()}.json");
        var response = await _httpClient.PostAsJsonAsync(uri, new { text });
        response.EnsureSuccessStatusCode();
        var dto = await response.Content.ReadFromJsonAsync<TranslationDto>();
        return dto!.Contents.Translated;
    }

    class TranslationDto
    {
        public required Contents Contents { get; init; }
    }

    class Contents
    {
        public required string Translated { get; init; }
    }
}
