
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace E_Homework.Controllers
{
    public class ReadinessCheck : IHealthCheck
    {
        private volatile bool _isReady;

        public bool StartupCompleted
        {
            get => _isReady;
            set => _isReady = value;
        }

        public Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            string appappname = System.Reflection.Assembly.GetExecutingAssembly().GetName().FullName;
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            string appversion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            if (StartupCompleted)
            {
                return Task.FromResult(HealthCheckResult.Healthy($"{appappname} {appversion} Ready"));
            }

            return Task.FromResult(HealthCheckResult.Unhealthy($"{appappname} {appversion} Unready"));
        }
    }
}