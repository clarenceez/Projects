using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QIC.Farm.WebApi.Models
{
    public class DistributionSearchModel
    {
        public string OpenID { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}