using System.Text;

namespace SMS.Common.Extensions
{
    public static class StringExtensions
    {
        public static string? FirstCharToLowerCase(this string? str)
        {
            if (!string.IsNullOrEmpty(str) && char.IsUpper(str[0]))
                return str.Length == 1 ? char.ToLower(str[0]).ToString() : char.ToLower(str[0]) + str[1..];
            return str;
        }

        public static string? FirstCharToUpperCase(this string? str)
        {
            if (!string.IsNullOrEmpty(str) && char.IsLower(str[0]))
                return str.Length == 1 ? char.ToUpper(str[0]).ToString() : char.ToUpper(str[0]) + str[1..];
            return str;
        }

        public static List<Guid>? CommaSeparatedToList(this string? str)
        {
            if (!string.IsNullOrEmpty(str) && str.Contains(','))
                return str.Split(',').Select(Guid.Parse).ToList();
            if (!string.IsNullOrEmpty(str))
                return new List<Guid> { new Guid(str) };
            return new List<Guid>();
        }

        public static byte[] ConvertToBytes(this string str) => Encoding.UTF8.GetBytes(str);
    }
}
