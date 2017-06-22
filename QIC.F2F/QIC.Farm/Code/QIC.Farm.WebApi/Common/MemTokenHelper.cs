﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using QIC.Infrastructure.WebFramework.Model;
using QIC.Infrastructure.WebFramework.Token;
using QIC.Infrastructure.Common.Safety.Dto;
using QIC.Farm.WebApi.Models;
using QIC.Farm.WebApi.ConfigSettings;

namespace QIC.Farm.WebApi.Common
{
    public class MemTokenHelper : TokenHelper
    {
        public MemTokenHelper(System.Web.HttpContext context, bool allowTourist)
            : base(context, allowTourist)
        {
        }
        public MemTokenHelper(System.Web.HttpContextBase context, bool allowTourist)
            : base(context, allowTourist)
        {
        }

        protected override void CheckOpenID(PayloadDto dto)
        {
            base.CheckOpenID(dto);
            if (dto.tourist) return;
        }       
    }
}