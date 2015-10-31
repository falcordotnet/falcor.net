using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Falcor.Server.Routing
{
    // Fluent Grammar
    public interface IPathValueBuilder
    {
        IPathValueBuilderResult Atom(object value, TimeSpan? expires = null);
        IPathValueBuilderResult Ref(params KeySegment[] keys);
        IPathValueBuilderResult Error(string error = null);
        IPathValueBuilderResult Undefined();
        IPathValueBuilder Append(params KeySegment[] keys);
        IPathValueBuilderWithKey Key(KeySegment key);
        IPathValueBuilderWithKey Keys(params KeySegment[] keys);
    }

    public interface IPathValueBuilderWithKey
    {
        IPathValueBuilderIntermediateResult Atom(object value, TimeSpan? expires = null);
        IPathValueBuilderIntermediateResult Ref(params KeySegment[] keys);
        IPathValueBuilderIntermediateResult Error(string error = null);
        IPathValueBuilderIntermediateResult Undefined();
    }

    public interface IPathValueBuilderIntermediateResult : IPathValueBuilderResult
    {
        IPathValueBuilderWithKey Key(KeySegment key);
        IPathValueBuilderWithKey Keys(params KeySegment[] keys);
    }

    public interface IPathValueBuilderResult : IEnumerable<PathValue>
    {
    }

    // Helper
    internal class PathValueResultHelper : IPathValueBuilder, IPathValueBuilderWithKey, IPathValueBuilderIntermediateResult
    {
        public PathValueResultHelper(FalcorPath path, IReadOnlyList<KeySegment> keys = null,
            IReadOnlyList<PathValue> results = null)
        {
            Path = path;
            CurrentKeys = keys ?? EmptyKeySegmentList;
            Results = results ?? EmptyPathValueList;
        }

        private static List<KeySegment> EmptyKeySegmentList { get; } = new List<KeySegment>();
        private static List<PathValue> EmptyPathValueList { get; } = new List<PathValue>();
        private FalcorPath Path { get; }
        private IReadOnlyList<KeySegment> CurrentKeys { get; }
        private IReadOnlyList<PathValue> Results { get; }

        public IPathValueBuilder Append(params KeySegment[] keys) =>
            new PathValueResultHelper(Path.AppendAll(new FalcorPath(keys)));

        public IPathValueBuilderWithKey Key(KeySegment key) => Keys(key);

        public IPathValueBuilderWithKey Keys(params KeySegment[] keys)
            => new PathValueResultHelper(Path, keys.ToList(), Results);

        public IPathValueBuilderResult Atom(object value, TimeSpan? expires = null)
            => WithResult(new Atom(value, expires));

        public IPathValueBuilderResult Ref(params KeySegment[] keys) => WithResult(new Ref(new FalcorPath(keys)));
        public IPathValueBuilderResult Error(string error) => WithResult(new Error(error));
        public IPathValueBuilderResult Undefined() => WithResult(null);

        public IEnumerator<PathValue> GetEnumerator() => Results.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        IPathValueBuilderIntermediateResult IPathValueBuilderWithKey.Ref(params KeySegment[] keys)
            => WithResult(new Ref(new FalcorPath(keys)));

        IPathValueBuilderIntermediateResult IPathValueBuilderWithKey.Error(string error) => WithResult(new Error(error));
        IPathValueBuilderIntermediateResult IPathValueBuilderWithKey.Undefined() => WithResult(null);

        IPathValueBuilderIntermediateResult IPathValueBuilderWithKey.Atom(object value, TimeSpan? expires)
            => WithResult(new Atom(value, expires));

        private PathValueResultHelper WithResult(object value) =>
            new PathValueResultHelper(Path, CurrentKeys,
                Results.Concat(CurrentKeys.Any()
                    ? CurrentKeys.Select(k => new PathValue(Path.Append(k), value))
                    : new List<PathValue> { new PathValue(Path, value) }).ToList());
    }
}