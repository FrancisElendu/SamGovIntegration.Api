using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Extensions.Http;
using SamGovIntegration.Api.Data;
using SamGovIntegration.Api.Options;
using SamGovIntegration.Api.Repositories;
using SamGovIntegration.Api.Services;


namespace SamGovIntegration.Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSamGovIntegration(this IServiceCollection services, IConfiguration configuration)
        {
            // Options
            services.Configure<SamApiOptions>(configuration.GetSection(SamApiOptions.SectionName));

            // Database
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            // Services
            services.AddScoped<ISamGovService, SamGovService>();
            services.AddScoped<IContractRepository, ContractRepository>();

            // HTTP Client with retry policies
            services.AddHttpClient<ISamGovService, SamGovService>((serviceProvider, client) =>
            {
                var options = serviceProvider.GetRequiredService<IOptions<SamApiOptions>>().Value;
                client.BaseAddress = new Uri(options.BaseUrl);
                client.Timeout = TimeSpan.FromSeconds(options.TimeoutSeconds);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            })
            .AddPolicyHandler(GetRetryPolicy())
            .AddPolicyHandler(GetCircuitBreakerPolicy());

            return services;
        }

        private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
                .WaitAndRetryAsync(
                    retryCount: 5,
                    sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }

        private static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .CircuitBreakerAsync(
                    handledEventsAllowedBeforeBreaking: 3,
                    durationOfBreak: TimeSpan.FromMinutes(1));
        }
    }
}
