using System;

namespace Falcor.Server
{
    public interface IMatch<TKeySegment> : IEquatable<TKeySegment> where TKeySegment : KeySegment { }
}