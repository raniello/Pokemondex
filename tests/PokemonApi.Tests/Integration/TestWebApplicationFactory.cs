using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using PokemonApi.Core.Ports;
using PokemonApi.Core.Services;

namespace PokemonApi.Tests.Integration;

public class TestWebApplicationFactory : WebApplicationFactory<Program>
{

    public Mock<IPokemonBasicInfoService> BasicInfoServiceMock { get; } = new Mock<IPokemonBasicInfoService>();
    public Mock<IPokemonTranslationService> TranslationGatewayMock { get; } = new Mock<IPokemonTranslationService>();
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {

        builder.ConfigureTestServices(services =>
        {
            services.AddControllers().AddApplicationPart(typeof(Program).Assembly);
            services.RemoveAll<IPokemonInfoGateway>();
            services.RemoveAll<ITranslationGateway>();

            services.RemoveAll<IPokemonBasicInfoService>();
            services.AddSingleton(BasicInfoServiceMock.Object);

            services.RemoveAll<IPokemonTranslationService>();
            services.AddSingleton(TranslationGatewayMock.Object);
        });
    }


}
