﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QIC.Farm.WebApi.Enums
{
    public enum LoginError
    {
        SystemError = 0,
        Success,
        UserNotExist,
        WrongPassword
    }
}