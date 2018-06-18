namespace PolyclinicProject.Domain.Extensions
{
    public static class StringExtensions
    {
        public static string ToFirstUpper(this string str)
        {
            return str?.Substring(0, 1)?.ToUpper() + (str?.Length > 1 ? str.Substring(1) : "")?.ToLower();
        }
    }
}