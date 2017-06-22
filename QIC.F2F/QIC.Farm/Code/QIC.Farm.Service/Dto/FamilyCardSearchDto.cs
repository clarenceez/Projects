using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QIC.Farm.Service.Dto
{
    public class FamilyCardSearchDto
    {
        public string FamilyName { get; set; }
        public string CardKindName { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }
}
