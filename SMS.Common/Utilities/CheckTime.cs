namespace SMS.Common.Utilities
{
    public static class CheckTime
    {
        public static string GetTimeDifference(DateTime pastDate)
        {
            DateTime now = DateTime.Now;
            TimeSpan timeSpan = now - pastDate;

            int years = now.Year - pastDate.Year;
            int months = now.Month - pastDate.Month;
            int days = now.Day - pastDate.Day;

            // Adjust for negative days or months
            if (days < 0)
            {
                months--;
                days += DateTime.DaysInMonth(now.Year, now.Month);
            }

            if (months < 0)
            {
                years--;
                months += 12;
            }

            // Constructing the result string
            string result = $"{years} years, {months} months, {days} days";

            return result;
        }


    }
}
