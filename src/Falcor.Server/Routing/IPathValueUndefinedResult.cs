using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Falcor.Server.Routing
{
    public interface IPathValueBuilder
    {
        IPathValueBuilderResult Atom(object value, TimeSpan? expires = null);
        IPathValueBuilderResult Ref(params KeySegment[] keys);
        IPathValueBuilderResult Error(string error = null);
        IPathValueBuilderResult Undefined();
        IPathValueBuilder Append(params KeySegment[] keys);
        IKeyDefinedResult Key(KeySegment key);
        IKeyDefinedResult Keys(params KeySegment[] keys);
    }

    public interface IKeyDefinedResult
    {
        IPathValueDefinedResult Atom(object value, TimeSpan? expires = null);
        IPathValueDefinedResult Ref(params KeySegment[] keys);
        IPathValueDefinedResult Error(string error = null);
        IPathValueDefinedResult Undefined();
    }

    public interface IPathValueDefinedResult : IPathValueBuilderResult
    {
        IKeyDefinedResult Key(KeySegment key);
        IKeyDefinedResult Keys(params KeySegment[] keys);
    }

    public interface IPathValueBuilderResult : IEnumerable<PathValue> { }

    public class PathValueResultBuilder : IPathValueBuilder, IKeyDefinedResult, IPathValueDefinedResult
    {
        private static List<KeySegment> EmptyKeySegmentList { get; } = new List<KeySegment>();
        private static List<PathValue> EmptyPathValueList { get; } = new List<PathValue>();
        private FalcorPath Path { get; }
        private IReadOnlyList<KeySegment> CurrentKeys { get; }
        private IReadOnlyList<PathValue> Results { get; }

        public PathValueResultBuilder(FalcorPath path, IReadOnlyList<KeySegment> keys = null, IReadOnlyList<PathValue> results = null)
        {
            Path = path;
            CurrentKeys = keys ?? EmptyKeySegmentList;
            Results = results ?? EmptyPathValueList;
        }

        private PathValueResultBuilder WithResult(object value) =>
            new PathValueResultBuilder(Path, CurrentKeys,
                Results.Concat(CurrentKeys.Any() ? CurrentKeys.Select(k => new PathValue(Path.Append(k), value))
                    : new List<PathValue>() { new PathValue(Path, value) }).ToList());

        public IPathValueBuilder Append(params KeySegment[] keys) => new PathValueResultBuilder(Path.AppendAll(new FalcorPath(keys)));

        public IKeyDefinedResult Key(KeySegment key) => Keys(key);
        public IKeyDefinedResult Keys(params KeySegment[] keys) =>
            new PathValueResultBuilder(Path, keys.ToList(), Results);

        public IPathValueBuilderResult Atom(object value, TimeSpan? expires = null) => WithResult(new Atom(value, expires));

        IPathValueDefinedResult IKeyDefinedResult.Ref(params KeySegment[] keys) => WithResult(new Ref(new FalcorPath(keys)));

        IPathValueDefinedResult IKeyDefinedResult.Error(string error) => WithResult(new Error(error));

        IPathValueDefinedResult IKeyDefinedResult.Undefined() => WithResult(null);

        IPathValueDefinedResult IKeyDefinedResult.Atom(object value, TimeSpan? expires) => WithResult(new Atom(value, expires));

        public IPathValueBuilderResult Ref(params KeySegment[] keys) => WithResult(new Ref(new FalcorPath(keys)));

        public IPathValueBuilderResult Error(string error) => WithResult(new Error(error));

        public IPathValueBuilderResult Undefined() => WithResult(null);

        public IEnumerator<PathValue> GetEnumerator() => Results.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

}