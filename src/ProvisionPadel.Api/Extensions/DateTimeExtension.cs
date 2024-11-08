namespace ProvisionPadel.Api.Extensions;

public static class DateTimeExtension
{
    public static DateTime ConvertToUtcDateTime(this string dateTimeString)
    {
        return DateTime.ParseExact(dateTimeString, "yyyyMMddTHHmmssZ", CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind);
    }
}

