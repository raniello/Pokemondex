using System;

namespace PokemonApi.Core.Ports;

public interface ITranslationGateway
{
    Task<string> TranslateAsync(string text);
}
