using System;

namespace PokemonApi.Core.Ports;

public interface ITranslationGateway
{
    public Task<string> TranslateAsync(TranslationType type, string text);
}

public enum TranslationType
{
    Yoda,
    Shakespeare
}