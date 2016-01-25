using System.Configuration;
using Battleships.Models.Interfaces;

namespace Battleships.Models
{
    public class ConfigurationManagerWrapper : IConfigurationManager
    {
        public System.Collections.Specialized.NameValueCollection AppSettings
        {
            get
            {
                return ConfigurationManager.AppSettings;
            }
        }
    }
}
