using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Falcor.Server;
using Falcor.Server.Routing;
using FluentAssertions;
using Xbehave;
using Xunit;
using Newtonsoft.Json;

namespace Falcor.Tests.Server.Routing
{
    public class ResponseSerializerTests
    {
        [Scenario]
        public void ShouldSerializeBoolean()
        {
            "ResponseSerializer should handle bollean path values".x(() =>
            {               
                var response = new List<PathValue>
                {
                    new PathValue(FalcorPath.Create("todos",1,"done"), true)
                };
                var serialize = new ResponseSerializer().Serialize(FalcorResponse.From(response.AsReadOnly()));
                dynamic result = JsonConvert.DeserializeObject(serialize);

                Assert.Equal(result["jsonGraph"]["todos"]["1"]["done"].Value,true);
            });
        }
        [Scenario]
        public void ShouldSerializeAtomBoolean()
        {
            "ResponseSerializer should handle bollean atom values".x(() =>
            {               
                var response = new List<PathValue>
                {
                    new PathValue(FalcorPath.Create("todos",1,"done"), new Atom(true))
                };
                var serialize = new ResponseSerializer().Serialize(FalcorResponse.From(response.AsReadOnly()));
                dynamic result = JsonConvert.DeserializeObject(serialize);

                Assert.Equal(result["jsonGraph"]["todos"]["1"]["done"]["value"].Value,true);
            });
        }
    }
}
