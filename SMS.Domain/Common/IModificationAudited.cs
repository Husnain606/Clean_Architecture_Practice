namespace SMS.Domain.Common
{
    public interface IModificationAudited : IHasModificationTime
    {
        string? ModifiedBy { get; set; }
    }
}
