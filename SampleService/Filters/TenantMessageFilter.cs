using System;
using System.Threading.Tasks;
using MassTransit;
using SampleService.Contracts;

namespace SampleService.Filters;

public class TenantMessageFilter : IFilter<PublishContext<ITenantMessage>>
{
    public void Probe(ProbeContext context) => context.CreateFilterScope("tenant-message-filter");
    public Task Send(PublishContext<ITenantMessage> context, IPipe<PublishContext<ITenantMessage>> next)
    {
        context.Message.TenantId = Random.Shared.Next();

        return next.Send(context);
    }
}
