using System.Configuration;

namespace CustomBPM.Core.Configuration
{
    public class BpmConfigurationSection : ConfigurationSection
    {
        [ConfigurationProperty("processes")]
        public ProcessElement Processes
        {
            get { return ((ProcessElement)(base["processes"])); }
        }
    }
}