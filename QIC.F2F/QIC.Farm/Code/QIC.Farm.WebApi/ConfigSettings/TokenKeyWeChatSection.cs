using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QIC.Farm.WebApi.ConfigSettings
{
    public class TokenKeyWeChatSection : ConfigurationElement
    {
        private static readonly ConfigurationProperty Property = new ConfigurationProperty(string.Empty, typeof(TokenKeyWeChatElementCollection), null, ConfigurationPropertyOptions.IsDefaultCollection);

        /// <summary>
        /// 微信元素配置的集合
        /// </summary>
        [ConfigurationProperty("", Options = ConfigurationPropertyOptions.IsDefaultCollection)]
        public TokenKeyWeChatElementCollection Wechat
        {
            get { return (TokenKeyWeChatElementCollection)base[Property]; }
        }
    }
}
