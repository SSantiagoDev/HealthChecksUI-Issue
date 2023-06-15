using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using System;

namespace AspNetCore.HealthChecks.Aggregator.UI;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddHealthChecksUI(settings =>
        {
            settings.SetHeaderText(Configuration.GetHealthCheckOptions().HeaderText);
            settings.SetEvaluationTimeInSeconds(Configuration.GetHealthCheckOptions().PollingInterval);

            Configuration.GetHealthCheckServices().ForEach(service =>
            {
                settings.AddHealthCheckEndpoint(service.Name, service.Url);
            });
        })
            .AddSqlServerStorage("Server=dbdevsch;Trusted_Connection=True;MultipleActiveResultSets=true;Database=HealthChecksTests");
        //.AddInMemoryStorage();

        services.AddHealthChecks()
            .AddCheck(name: "TEST", () => HealthCheckResult.Degraded())
            .AddCheck(name: "Random", () => DateTime.UtcNow.Second % 2 == 0 ? HealthCheckResult.Healthy() : HealthCheckResult.Unhealthy());
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapHealthChecksUI(setup =>
            {
                setup.ApiPath = Configuration.GetHealthCheckOptions().ApiPath;
                setup.UIPath = Configuration.GetHealthCheckOptions().UIPath;
            });

            endpoints.MapHealthChecks("/health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });
        });
    }
}