using System;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using SampleService.Contracts;

namespace SampleService.Filters;

public class TenantMessageFilter : IFilter<PublishContext<ITenantMessage>>, IFilter<ConsumeContext<ITenantMessage>>, IFilter<ConsumeContext<IsItTime>>
{
    private readonly ILogger<TenantMessageFilter> _logger;

    public TenantMessageFilter(ILogger<TenantMessageFilter> logger)
    {
        _logger = logger;
    }

    public void Probe(ProbeContext context) => context.CreateFilterScope("tenant-message-filter");
    public Task Send(PublishContext<ITenantMessage> context, IPipe<PublishContext<ITenantMessage>> next)
    {
        context.Message.TenantId = Random.Shared.Next();

        _logger.LogInformation("PUBLISH-FILTER: Processing {messageType}, set tenant id to {tenantId}", context.Message.GetType().Name, context.Message.TenantId);

        return next.Send(context);
    }

    public Task Send(ConsumeContext<ITenantMessage> context, IPipe<ConsumeContext<ITenantMessage>> next)
    {
        _logger.LogInformation("CONSUME-FILTER: Processing {messageType}, has tenant id {tenantId}", context.Message.GetType().Name, context.Message.TenantId);

        return next.Send(context);
    }

    public Task Send(ConsumeContext<IsItTime> context, IPipe<ConsumeContext<IsItTime>> next)
    {
        _logger.LogInformation("CONSUME-FILTER: Specific IsItTime filter, has tenant id {tenantId}", context.Message.TenantId);

        return next.Send(context);
    }
}
