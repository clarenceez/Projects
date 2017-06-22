using QIC.Farm.WebApi.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QIC.Farm.WebApi.Dtos
{
    public class TokenValidateResult
    {
        public TokenError Result { get; set; }
        public object Data { get; set; }
        public TokenValidateResult()
        {
            Result = TokenError.Success;
        }
    }
}