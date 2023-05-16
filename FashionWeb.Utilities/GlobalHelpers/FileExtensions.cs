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
        public static bool IsParseToGuidSuccess(this string str)
        {
            var output = default(Guid);
            return Guid.TryParse(str, out output);
        }
    }
}
