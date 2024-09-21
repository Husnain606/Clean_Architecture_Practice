namespace SMS.Domain.Common
{
    public interface IHasCreationTime
    {
        DateTime CreatedAt { get; set; }
    }
}
