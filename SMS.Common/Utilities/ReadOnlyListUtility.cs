using System.Collections.ObjectModel;
using SMS.Common.Constants;

namespace SMS.Common.Utilities
{
    public class ReadOnlyListUtility
    {
        public static ReadOnlyCollection<string> ReadOnlyListOfSchemaNames()
        {
            return new ReadOnlyCollection<string>(
                     new List<string>(5)
                     {
                        ReadOnlyListConstants.JobsCount,
                        ReadOnlyListConstants.LocationsCount,
                        ReadOnlyListConstants.ServicesCount,
                        ReadOnlyListConstants.CreatedAt,
                        ReadOnlyListConstants.Duration
                     });
        }
    }
}
