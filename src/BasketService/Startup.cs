using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using System;

namespace BasketService;

public class Startup
{
    public static Random rnd = new(5);

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddHealthChecks()
            .AddCheck(name: "Always Healthy", () => HealthCheckResult.Healthy())
            .AddCheck(name: "Random",
            () =>
            {
                int rndValue = rnd.Next(0, 10);
                return rndValue % 2 == 0 ? HealthCheckResult.Healthy() : HealthCheckResult.Unhealthy();
            });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
            app.UseDeveloperExceptionPage();

        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapHealthChecks("/health", new HealthCheckOptions
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });
        });
    }
}