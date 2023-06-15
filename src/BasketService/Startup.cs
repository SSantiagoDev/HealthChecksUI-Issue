using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;

namespace BasketService;

public class Startup
{
    //public static bool isHealthy1 = true;
    public static bool isHealthy2 = true;
    public static bool isHealthy3 = false;
    public static Random rnd = new Random(5);

    public void ConfigureServices(IServiceCollection services)
    {
        //var a = services.AddHealthChecks();

        //for (int i = 0; i < 10; i++)
        //{
        //    a.AddCheck(name: $"REAL Random{i}",
        //        () =>
        //        {
        //            var test = rnd.Next(0, 10);

        //            return test % 2 == 0
        //                ? HealthCheckResult.Healthy()
        //                : HealthCheckResult.Unhealthy();
        //        });
        //}

        services.AddHealthChecks()
            //.AddCheck(name: "TEST1", () => HealthCheckResult.Unhealthy())
            //.AddCheck(name: "TEST1", () =>
            //{
            //    if (isHealthy1)
            //    {
            //        isHealthy1 = false;
            //        return HealthCheckResult.Unhealthy();
            //    }
            //    else
            //    {
            //        isHealthy1 = true;
            //        return HealthCheckResult.Healthy();
            //    }
            //})
            .AddCheck(name: "TEST2", () =>
            {
                if (isHealthy2)
                {
                    isHealthy2 = false;
                    return HealthCheckResult.Unhealthy();
                }
                else
                {
                    isHealthy2 = true;
                    return HealthCheckResult.Healthy();
                }
            })
            .AddCheck(name: "TEST3", () =>
            {
                if (isHealthy3)
                {
                    isHealthy3 = false;
                    return HealthCheckResult.Unhealthy();
                }
                else
                {
                    isHealthy3 = true;
                    return HealthCheckResult.Healthy();
                }
            }, tags: new List<string> { "b" });
        //.AddCheck(name: "REAL Random1",
        //    () =>
        //    {
        //        var test = rnd.Next(0, 10);

        //        return test % 2 == 0
        //            ? HealthCheckResult.Healthy()
        //            : HealthCheckResult.Unhealthy();
        //    });
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
                //Predicate = _ => true,
                Predicate = (check) => !check.Tags.Contains("b"),
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            endpoints.MapHealthChecks("/health/b", new HealthCheckOptions()
            {
                Predicate = (check) => check.Tags.Contains("b"),
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });
        });
    }
}