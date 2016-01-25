using System.Collections.Specialized;

namespace Battleships.Models.Interfaces
{
    public interface IConfigurationManager
    {
        NameValueCollection AppSettings { get; }
    }
}
