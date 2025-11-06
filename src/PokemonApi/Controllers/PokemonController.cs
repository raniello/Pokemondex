using System;
using Microsoft.AspNetCore.Mvc;
using PokemonApi.Core.Services;

namespace PokemonApi.Controllers;

[ApiController]
[Route("[controller]")]
public class PokemonController(
    IPokemonBasicInfoService pokemonBasicInfoService,
    IPokemonTranslationService pokemonTranslationService) : ControllerBase
{
    IPokemonBasicInfoService _pokemonBasicInfoService = pokemonBasicInfoService;
    IPokemonTranslationService _pokemonTranslationService = pokemonTranslationService;

    [HttpGet("{pokemonName}")]
    public async Task<IActionResult> GetBasicInfoAsync(string pokemonName) {
        var result = await _pokemonBasicInfoService.GetBasicInfoAsync(pokemonName);
        return result == null ? NotFound() : Ok(result);
    }

    [HttpGet("translated/{pokemonName}")]
    public async Task<IActionResult> GetTranslatedInformationAsync(string pokemonName)
    {
        var result = await _pokemonTranslationService.GetTranslatedInfoAsync(pokemonName);
        return result == null ? NotFound() : Ok(result);
    }
    
}

