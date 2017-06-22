﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QIC.Farm.Service.Dto
{
    public class ProductReportDto
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Writer { get; set; }
        public bool IsExpired { get; set; }
        public bool IsClosed { get; set; }
        public DateTime CreateTime { get; set; }

        //报告html文本内容
        public string DescriptionHtmlContent { get; set; }
    }
}
