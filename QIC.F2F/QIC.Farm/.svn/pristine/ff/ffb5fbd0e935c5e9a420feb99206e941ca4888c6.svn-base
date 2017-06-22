using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QIC.Farm.Service.Model;
using QIC.Farm.Service.Dto;

namespace QIC.Farm.Service.Dal
{
    public class FamilyWechatDal
    {
        public bool Add(FamilyWechatDto dto)
        {
            using (var context = new Entities())
            {
                context.FamilyWechat.Add(new FamilyWechat { 
                    FamilyID = dto.FamilyID,
                    OpenID = dto.OpenID
                });

                var count = context.SaveChanges();
                return count > 0;
            }
        }

        public bool IsOpenIDExist(string openID)
        {
            using (var context = new Entities())
            {
                return context.FamilyWechat.Any(x => x.OpenID == openID);
            }
        }
    }
}
