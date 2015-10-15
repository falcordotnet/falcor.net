using System;
using System.Collections.Generic;

namespace Falcor
{
    public interface IDataSource
    {
        IObservable<JsonGraphEnvelope> Call(
            FalcorPath functionPath,
            IEnumerable<object> args,
            IEnumerable<FalcorPath> refSuffixes,
            IEnumerable<FalcorPath> thisPaths);

        IObservable<JsonGraphEnvelope> Get(params FalcorPath[] pathSets);
        IObservable<JsonGraphEnvelope> Set(JsonGraphEnvelope jsonGraphEnvelope);
    }
}