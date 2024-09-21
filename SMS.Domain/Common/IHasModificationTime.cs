namespace SMS.Domain.Common
{
    public interface IHasModificationTime
    {
        DateTime? ModifiedAt { get; set; }
    }
}
