﻿using QIC.Farm.Service.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QIC.Farm.Service.Dto
{
    public class ProductReportResult
    {
        public int ID { get; set; }
        public string Message { get; set; }
        public ProductReportError ProductReportError { get; set; }
    }
}
