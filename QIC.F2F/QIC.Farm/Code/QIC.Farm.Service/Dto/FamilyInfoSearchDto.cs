using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QIC.Farm.Service.Dto
{
    public class FamilyInfoSearchDto
    {
        public string Name { get; set; }
        public string NickName { get; set; }
        public string Mobile { get; set; }
        public bool? IsClosed { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
