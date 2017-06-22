using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QIC.Farm.Service.Dto
{
    public class FamilyAddressDto
    {
        public int ID { get; set; }
        public int FamilyID { get; set; }

        //查询使用
        public string FamilyName { get; set; }

        public string Contacter { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string PostCode { get; set; }
        public bool IsClosed { get; set; }
    }
}
