using System.Globalization;

namespace Utilities.GlobalHelpers
{
    public static class ExtensionHelpers
    {
        public static string GetPriceFormat(this decimal price)
        {
            var cultureInfo = CultureInfo.GetCultureInfo("en-US");
            return string.Format(cultureInfo, "{0:C2}", price);
        }

        public static bool IsConvertToNumber(this string content)
        {
            decimal number = 0;
            return decimal.TryParse(content, out number);
        }
    }
}
