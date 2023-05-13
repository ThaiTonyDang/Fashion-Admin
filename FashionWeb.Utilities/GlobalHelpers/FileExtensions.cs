using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;

namespace FashionWeb.Utilities.GlobalHelpers
{
    public static class FileExtensions
    {
        public static string GetPriceFormat(this decimal price)
        {
            CultureInfo cultureInfo = CultureInfo.GetCultureInfo("en-US");
            return string.Format(cultureInfo, "{0:C0}", price);
        }

        public static bool IsParseToGuidSuccess(this string str)
        {
            Guid output;
            return Guid.TryParse(str, out output);
        }
    }
}
