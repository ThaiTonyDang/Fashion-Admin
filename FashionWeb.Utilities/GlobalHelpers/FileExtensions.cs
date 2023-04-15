using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FashionWeb.Utilities.GlobalHelpers
{
    public static class FileExtensions
    {
        public static async Task<byte[]> GetBytes(this IFormFile formFile)
        {
            using (var memoryStream = new MemoryStream())
            {
                await formFile.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }
        }

        public static string GetPriceFormat(this decimal price)
        {
            CultureInfo cultureInfo = CultureInfo.GetCultureInfo("en-GB");
            return string.Format(cultureInfo, "{0:C0}", price);
        }

        public static string GetStringFromBoolenFormat(this bool value, string strTrue, string strFalse)
        {
            if (!value) return strFalse.Replace(" ", "").Trim();
            return strTrue.Replace(" ", "").Trim();
        }

        public static bool GetBoolenFromStringFormat(this string value, string strTrue)
        {
            value = value.Replace(" ", "").Trim().ToLower();
            strTrue = strTrue.Replace(" ", "").Trim().ToLower();

            if (value.Equals(strTrue)) return true;
            return false;
        }

        public static bool IsParseToGuidSuccess(this string str)
        {
            Guid output;
            return Guid.TryParse(str, out output);
        }
    }
}
