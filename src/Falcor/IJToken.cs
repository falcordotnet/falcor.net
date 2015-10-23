using Newtonsoft.Json.Linq;

namespace Falcor
{
    public interface IJToken
    {
        JToken ToJToken();
    }
}