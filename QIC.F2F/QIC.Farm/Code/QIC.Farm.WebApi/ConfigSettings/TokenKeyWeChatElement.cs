using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QIC.Farm.WebApi.ConfigSettings
{
    public class TokenKeyWeChatElement : ConfigurationElement
    {
        [ConfigurationProperty("name")]
        public string Name
        {
            get { return (string)this["name"]; }
        }

        /// <summary>
        /// 配置微信的Token公钥
        /// </summary>
        [ConfigurationProperty("shareKey")]
        public string ShareKey
        {
            get { return (string)this["shareKey"]; }
        }

        /// <summary>
        /// 配置微信的Token密钥
        /// </summary>
        [ConfigurationProperty("secretKey")]
        public string SecretKey
        {
            get { return (string)this["secretKey"]; }
        }

        /// <summary>
        /// 配置微信的Token超时时间
        /// </summary>
        [ConfigurationProperty("expiredTime")]
        public ulong TokenExpiredTime
        {
            get { return (ulong)this["expiredTime"]; }
        }

        /// <summary>
        /// 配置服务器时差
        /// </summary>
        [ConfigurationProperty("timeEquation")]
        public ulong TimeEquation
        {
            get { return (ulong)this["timeEquation"]; }
        }

        /// <summary>
        /// 
        /// </summary>
        [ConfigurationProperty("signature")]
        public string Signature
        {
            get { return (string)this["signature"]; }
        }
    }
}
