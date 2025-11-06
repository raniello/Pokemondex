using PokemonApi.Core.Ports;
using PokemonApi.Core.Services;
using PokemonApi.Gateways;

namespace PokemonApi;

public class CompositionRoot
{
     public static WebApplication BuildApp()
    {
        var builder = WebApplication.CreateBuilder();
        builder.Services.AddControllers();
        RegisterServices(builder);
        RegisterGateways(builder);
        var app = builder.Build();
        app.MapControllers();
        return app;
    }

    private static void RegisterGateways(WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton(new PockemonSpeciesUri { Value = "https://pokeapi.co/api/v2/pokemon-species" });
        builder.Services.AddSingleton(new FunTranslationUri { Value = "https://api.funtranslations.com/translate" });
        builder.Services.AddScoped<IPokemonInfoGateway, PokemonInfoGateway>();
        builder.Services.AddScoped<ITranslationGateway, FunTranslationGateway>();
    }

    private static void RegisterServices(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IPokemonBasicInfoService, PokemonBasicInfoService>();
        builder.Services.AddScoped<IPokemonTranslationService, PokemonTranslationService>();
    }
}