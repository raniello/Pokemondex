# Pokemon Fun Pokedex API

This project implements a simple REST API that retrieves basic Pokemon information, including a "fun" translated description based on specific criteria. The API utilizes the **PokéAPI** and **FunTranslations API** to achieve this.

## 🚀 Endpoints

The API is expected to run on `http://localhost:5000` by default.

### 1. Basic Pokemon Info

Returns the standard English description, habitat, and legendary status of a given Pokemon.

| **Method** | **Path** | **Example** |
| :--- | :--- | :--- |
| `GET` | `/pokemon/{pokemonName}` | `/pokemon/mewtwo` |

### 2. Translated Pokemon Info

Returns the translated description based on the following rules:

* **Yoda Translation:** Applied if the Pokemon's habitat is `cave` or if it is `legendary`.
* **Shakespeare Translation:** Applied for all other Pokemon.
* **Fallback:** If translation fails, the standard English description is returned.

| **Method** | **Path** | **Example** |
| :--- | :--- | :--- |
| `GET` | `/pokemon/translated/{pokemonName}` | `/pokemon/translated/mewtwo` |

## 🛠️ Prerequisites

To run and develop this application, you need the following tools installed on your system:

* [**.NET SDK 9.0**](https://dotnet.microsoft.com/download/dotnet/9.0)**:** Required to build and run the C# project.

### Recommended Development Environment

The project was developed using **Visual Studio Code** with the following extensions for an optimal development experience:

* **C#** (by Microsoft)
* **C# Dev Kit** (by Microsoft)
* **NuGet Gallery** (Optional, for browsing and installing NuGet packages)
* **Rest Client** (Optional, for executing the included `.http` file to test the API endpoints)

## ⚙️ How to Run the Application

### Option 1: Direct Execution (Requires .NET SDK)

Navigate to the root directory (where `PokemonApi.sln` file is located) and execute:

dotnet run --project src/PokemonApi/PokemonApi.csproj

The API will start running, usually on `http://localhost:5000`.

### Option 2: Using Docker

A `Dockerfile` is included for containerization, which provides a clean and portable way to run the application.

1. **Build the Docker Image:**

      docker build -t pokemonapi:latest .
   
2. **Run the Container:**

      docker run --rm -p 5000:5000 pokemonapi:latest
   
   The `--rm` flag ensures the container is cleaned up after stopping. The application will be accessible at `http://localhost:5000`.

## 🎨 Design Notes

The application follows the **Hexagonal Architecture** (Ports and Adapters) paradigm to ensure separation of concerns, testability, and maintainability.

### Architecture Overview

1. **Ports (Interfaces):** Define the core application logic contracts (e.g., `IPokemonInfoService`, `IPokemonTranslationService`).
2. **Adapters (Implementations):** Implement the logic and handle interaction with external systems (e.g., `PokemonTranslationService` and the `Gateways` for API calls).

### Key Decisions

* **`PokemonInfoService` Inclusion:** This service currently acts as a *pure pass-through* between the Controller and the Gateway. While it could be eliminated to shorten the call stack (`Controller -> Gateway`), it was kept for **architectural symmetry** and to strictly adhere to the Hexagonal Architecture pattern. This decision is justified if future feature evolution is expected to introduce application or business logic into this layer.

* **`PokemonTranslationService` Improvement (Open/Closed Principle):** The translation logic is currently implemented using hardcoded conditional statements based on an internal `enum` of supported translation types.

  * **Suggested Refactor:** To make the service more compliant with the **Open/Closed Principle (OCP)**, a **Chain of Responsibility** pattern could be introduced. This would allow new translation types (e.g., Minion, Pirate) to be added without modifying the existing `PokemonTranslationService` code, only requiring a new handler in the chain.

## 📈 Production Readiness Improvements

For a production environment, several critical improvements are necessary to ensure performance, reliability, and security:

### Reliability and Resilience

| **Area** | **Issue** | **Proposed Solution** |
| :--- | :--- | :--- |
| **Caching** | Dependency on two external services (PokéAPI and FunTranslations) introduces latency and rate limiting risks, particularly with the highly constrained FunTranslations API. | Implement a **Caching Layer** (e.g., using Redis) for external API responses. This mitigates rate limits, reduces latency, and improves overall API reliability. |
| **Error Handling** | The current implementation might throw a 500 `InternalServerError` when a Pokemon is not found (`404` from PokéAPI) or when external data is incomplete. | **1. Data Validation:** Implement validation for essential fields returned by external APIs.<br> **2. 404 Handling:** Handle the NotFound status code for a non-existent `PokemonSpecies` in order to return an empty result instead of throwing an exception, correctly returning an HTTP **404 Not Found** status code downstream. |
| **Circuit Breaker** | A constant failure state from FunTranslations (e.g., 500 errors) can degrade the entire application's performance. | Implement a **Circuit Breaker** pattern. If the FunTranslations API fails consistently, "open the circuit" to immediately fallback to the standard description for a set period (e.g., 60 seconds), preventing resource waste on failing calls. |
| **Retries** | Transient network errors (e.g., 502, 503) from external APIs. | Implement a **Retry Logic** with **Exponential Backoff** to handle temporary external API errors gracefully. |

### Observability and Security

| **Area** | **Issue** | **Proposed Solution** |
| :--- | :--- | :--- |
| **Logging** | Standard logging may lack the necessary context for debugging microservices. | Implement **Structured Logging** (e.g., JSON format). Log key metrics like external call latency, cache hit/miss ratio, and correlation IDs to trace requests across the system. |
| **Monitoring** | Need to monitor the health and performance of the application and its dependencies. | **Export Metrics** (e.g., Prometheus/OpenTelemetry) to monitor the application's vital signs: external API error rates, latency percentiles, and resource usage. |
| **Configuration** | Configuration values (API URLs, timeouts) are currently hardcoded or in configuration files. | Use **Environment Variables** for all configurable values (API URLs, Cache TTL, Redis connection strings) following the [Twelve-Factor App methodology](https://12factor.net/). |
| **Rate Limiting (Self-Protection)** | The API needs protection from abuse and to protect upstream dependencies. | Implement **Our Own Rate Limiter** to control the request flow into our API. |

### Testing
* **Integration Test Endpoints:**  Refactor integration tests to target mocked endpoints instead of real production external services (e.g., using MockServer or dedicated test environments). This ensures tests are deterministic, faster, and prevents unnecessary consumption of external API quotas.

* **End-to-End (E2E) Testing:** Add E2E tests to verify the entire flow, from the API gateway down to the external service calls and back.

* **Translation Edge Cases:** Verify the functionality of the FunTranslations API (especially Yoda) with descriptions containing newlines or missing terminal punctuation (e.g., a final period), as these can corrupt the translation quality if not pre-processed correctly.

###  AI Assistance Note
This document's structure and content were refined and organized with the assistance of an AI language model.