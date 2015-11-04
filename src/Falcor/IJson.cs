using Newtonsoft.Json.Linq;

namespace Falcor
{
    public interface IJson
    {
        JToken ToJson();
    }
}