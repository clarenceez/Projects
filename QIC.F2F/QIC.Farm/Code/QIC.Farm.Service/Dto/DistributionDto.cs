﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QIC.Farm.Service.Dto
{
    public class DistributionDto
    {
        public string ID { get; set; }
        public int FamilyID { get; set; }

        //用于显示
        public string FamilyName { get; set; }

        public string Contacter { get; set; }
        public string Phone { get; set; }

        public int FamilyAddressID { get; set; }

        public string Address { get; set; }
        public DateTime DistributedDate { get; set; }

        public string DistributedDateStr { get; set; }

        public DateTime CreateTime { get; set; }

        //家庭详情
        public FamilyInfoDto FamilyInfo { get; set; }

        public IEnumerable<FamilyCardDto> FamilyCardInfo { get; set; }

        //配送详情(产品列表)
        public IEnumerable<DistributionDetailDto> DistributionDetails { get; set; }
    }
}
