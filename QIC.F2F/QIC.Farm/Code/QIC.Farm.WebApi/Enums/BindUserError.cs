﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QIC.Farm.WebApi.Enums
{
    public enum BindUserError
    {
        SystemError = 0,
        Success,
        CaptchaError,
        OpenIDIsNull
    }
}