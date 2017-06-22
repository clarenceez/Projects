using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QIC.Farm.WebApi.ConfigSettings
{
    public class TokenKeyInfo : ConfigurationSection
    {
        /// <summary>
        /// MongoDb配置节
        /// </summary>
        [ConfigurationProperty("Wechat", IsDefaultCollection = true)]
        public TokenKeyWeChatSection WechatSection
        {
            get { return (TokenKeyWeChatSection)this["Wechat"]; }
        }
    }
}
