using System.ComponentModel;

namespace SMS.Domain.Common
{
    public interface ISoftDelete
    {
        [DefaultValue(false)]
        bool IsDeleted { get; set; }
    }
}
