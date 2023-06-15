using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ProductService;

public class NTServiceHealthCheck : IHealthCheck
{
    private string ServiceName = "PFWorker Service 1";
    private ILogger<NTServiceHealthCheck> _logger;

    public NTServiceHealthCheck(ILogger<NTServiceHealthCheck> logger)
    {
        _logger = logger;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
    {

#if DEBUG

        ServiceName = "Fax";   //The PFWorker NT services are not installed on a Dev machine.
        //Automatic && Running OK
        //Manual    && Running OK
        //Automatic && Stopped NOK
        //Manual    && Stopped NOK

#endif

        string serviceMsg = $"The service '{ServiceName}'";

        try
        {
            //CHECK 1
            //using var serviceController = new ServiceController(ServiceName);
            //return serviceController.Status == ServiceControllerStatus.Running
            //    ? HealthCheckResult.Healthy($"{serviceMsg} is running")
            //    : HealthCheckResult.Unhealthy($"{serviceMsg} is not running");

            //CHECK 2
            //Process[] processes = Process.GetProcessesByName(ServiceName);
            //return processes.Length > 0
            //    ? HealthCheckResult.Healthy($"{serviceMsg} is running")
            //    : HealthCheckResult.Unhealthy($"{serviceMsg} is not running");

            //CHECK 3
            //string query = $"SELECT * FROM Win32_Service WHERE Name = '{ServiceName}'";
            //using var searcher = new ManagementObjectSearcher(query);
            //ManagementObjectCollection services = searcher.Get();
            //return services.Count > 0
            //    ? HealthCheckResult.Healthy($"{serviceMsg} is running")
            //    : HealthCheckResult.Unhealthy($"{serviceMsg} is not running");

            //CHECK 4
            //if (!OperatingSystem.IsWindows())
            //    return HealthCheckResult.Unhealthy("This check works only on Windows machines.");

            //ServiceController[] services = ServiceController.GetServices();

            //if (!services.IsNotNullOrEmpty() || services.All(x => x.DisplayName != ServiceName))
            //    return HealthCheckResult.Unhealthy($"{serviceMsg} is unknown!");

            //ServiceController? service = services.FirstOrDefault(x => x.DisplayName == ServiceName);

            //return service is { Status: ServiceControllerStatus.Running }
            //    ? HealthCheckResult.Healthy($"{serviceMsg} is running")
            //    : HealthCheckResult.Unhealthy($"{serviceMsg} is not running");
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex.Message);
            return HealthCheckResult.Unhealthy($"{serviceMsg} is not installed on this system.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return HealthCheckResult.Unhealthy($"Exception occurred while getting the service: {serviceMsg} - {ex.Message}.");
        }

        return HealthCheckResult.Healthy($"THIS is running");
    }
}