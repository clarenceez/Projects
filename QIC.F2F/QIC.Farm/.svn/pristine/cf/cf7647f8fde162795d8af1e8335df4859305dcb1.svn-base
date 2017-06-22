using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QIC.Farm.WebApi.ConfigSettings
{
    [ConfigurationCollection(typeof(TokenKeyWeChatElement))]
    public class TokenKeyWeChatElementCollection : ConfigurationElementCollection
    {
        public new TokenKeyWeChatElement this[string name]
        {
            get
            {
                return (TokenKeyWeChatElement)base.BaseGet(name);
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new TokenKeyWeChatElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return (element as TokenKeyWeChatElement).Name;

        }
    }
}
