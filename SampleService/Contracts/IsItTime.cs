namespace SampleService.Contracts
{
    public record IsItTime() : ITenantMessage
    {
        public long TenantId { get; set; }
    }
}