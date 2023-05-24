using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FashionWeb.Domain.Model
{
    public class ErrorResponseApi<T> : BaseReponseApi
    {
        public T Errors { get; set; }
    }
}
