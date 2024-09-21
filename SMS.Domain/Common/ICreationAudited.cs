namespace SMS.Domain.Common
{
    public interface ICreationAudited : IHasCreationTime
    {
        string? CreatedBy { get; set; }
    }
}
