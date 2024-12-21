namespace TestAssessment.Helpers;

public static class ConversionUtils
{
    public static DateTime ConvertToUtc(DateTime dateTime, string timeZoneId)
    {
        var timeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
        
        return TimeZoneInfo.ConvertTimeToUtc(dateTime, timeZone);
    }

    public static string? ConvertFlag(string? flag)
    {
        if (string.IsNullOrWhiteSpace(flag))
        {
            return null;
        }
        
        var trimmed = flag.Trim();
        
        return trimmed.Equals("Y", StringComparison.OrdinalIgnoreCase) ? "Yes" : "No";
    }
}