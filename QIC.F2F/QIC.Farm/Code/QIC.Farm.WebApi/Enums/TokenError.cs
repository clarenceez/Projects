using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QIC.Farm.WebApi.Enums
{
    public enum TokenError
    {       
        SystemError = 0,
        Success = 1,
        TokenInvalid,
        SignatureError,
        TokenDecodeError,
        OverTime,
        PlayloadError,
    }
}