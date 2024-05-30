using System;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using SampleService.Contracts;

namespace SampleService
{
    public class TimeConsumer :
        IConsumer<IsItTime>
    {
        private readonly ILogger<TimeConsumer> _logger;

        public TimeConsumer(ILogger<TimeConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<IsItTime> context)
        {
            _logger.LogInformation("Tenant id is {tenantId}", context.Message.TenantId);
            var now = DateTimeOffset.Now;
            if (now.DayOfWeek == DayOfWeek.Friday && now.Hour >= 17)
            {
                return context.RespondAsync<YesItIs>(new { });
            }

            return context.RespondAsync<NoNotYet>(new { });
        }
    }
}