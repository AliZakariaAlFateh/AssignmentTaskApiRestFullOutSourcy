using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Enums
{
    public enum ResponseStatusCode
    {
        OK = 200,
        Created = 201,
        BadRequest = 400,
        NotFound = 404,
        InternalServerError = 500
    }
}
