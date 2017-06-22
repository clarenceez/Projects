﻿using QIC.Farm.WebApi.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QIC.Farm.WebApi.Dtos
{
    public class LoginResult
    {
        public string Message { get; set; }
        public LoginError LoginError { get; set; }
    }
}