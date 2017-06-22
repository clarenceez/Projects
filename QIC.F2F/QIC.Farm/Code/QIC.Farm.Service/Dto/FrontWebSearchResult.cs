using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QIC.Farm.Service.Dto
{
    public class FrontWebSearchResult<T>:CommonSearchResult<T>
    {
        public string MemberName { get; set; }
    }
}
