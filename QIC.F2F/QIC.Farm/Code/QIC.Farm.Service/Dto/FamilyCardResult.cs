﻿using QIC.Farm.Service.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QIC.Farm.Service.Dto
{
    public class FamilyCardResult
    {
        public string Message { get; set; }
        public FamilyCardError FamilyCardError { get; set; }
    }
}
