using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QIC.Farm.WebApi.Models
{
    public class WechatSectionModel
    {
        public string ShareKey { get; set; }
        public string SecretKey { get; set; }
        public string Signature { get; set; }
        public ulong TokenExpiredTime { get; set; }
        public ulong TimeEquation { get; set; }

    }
}