using QIC.Farm.Service.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QIC.Farm.WebApi.Models
{
    public class DistributionModel
    {
        public DistributionDto Distribution { get; set; }
        public List<DistributionDetailDto> DistributionDetails { get; set; }
    }
}