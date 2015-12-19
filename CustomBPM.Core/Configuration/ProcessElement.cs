using System.Configuration;

namespace CustomBPM.Core.Configuration
{
    public class ProcessElement : ConfigurationElement
    {
        [ConfigurationProperty("location", DefaultValue = "", IsKey = false, IsRequired = true)]
        public string Location
        {
            get { return ((string)(base["location"])); }
            set { base["location"] = value; }
        }
    }
}