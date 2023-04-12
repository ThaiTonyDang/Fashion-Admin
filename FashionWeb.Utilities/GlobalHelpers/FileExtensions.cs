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
            CultureInfo cultureInfo = CultureInfo.GetCultureInfo("en-US");
            return string.Format(cultureInfo, "{0:C}", price);
        }
    }
}
