using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.SDK.Security.Configuration
{
    public class ProviderCollection : ConfigurationElementCollection
    {
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((AddSection)element).Name;
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new AddSection();
        }

        public AddSection this[int i]
        {
            get
            {
                return (AddSection)base.BaseGet(i);
            }
        }

        public new AddSection this[string key]
        {
            get
            {
                return (AddSection)base.BaseGet(key);
            }
        }
    }
}
