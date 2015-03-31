using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crossroads.Utilities.Interfaces
{
    public interface IConfigurationWrapper
    {
        int GetConfigIntValue(string key);
        string GetConfigValue(string key);
        string GetEnvironmentVarAsString(string variable);
    }
}
