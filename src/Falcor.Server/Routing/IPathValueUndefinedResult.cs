using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace Falcor.Server.Routing
{
    public interface IPathValueBuilder
    {
        IPathValueBuilderResult Atom(object value, TimeSpan? expires = null);
        IPathValueBuilderResult Ref(params KeySegment[] keys);
        IPathValueBuilderResult Error(string error = null);
        IPathValueBuilderResult Undefined();
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
        private readonly FalcorPath _currentPath;
        private readonly List<KeySegment> _currentKeys = new List<KeySegment>();
        private readonly List<PathValue> _values = new List<PathValue>();

        public PathValueResultBuilder(FalcorPath initialPath)
        {
            _currentPath = initialPath;
        }



        //public IPathValueResultResult Ref(params KeySegment[] keys) => 

        //public IPathValueResultResult Error(string error = null) => AddResult(new Error(error));

        //public IPathValueResultResult Undefined() => AddResult(null);

        //public IPathValueResultResult Atom(object value, TimeSpan? expires = null) => AddResult(new Atom(value, expires));

        private PathValueResultBuilder AddResult(object obj)
        {
            _currentKeys.ForEach(key => _values.Add(new PathValue(_currentPath.Append(key), obj)));
            return this;
        }

        public IKeyDefinedResult Key(KeySegment key)
        {
            _currentKeys.Clear();
            _currentKeys.Add(key);
            return this;
        }

        public IKeyDefinedResult Keys(params KeySegment[] keys)
        {
            _currentKeys.Clear();
            _currentKeys.AddRange(keys);
            return this;
        }

        public IPathValueBuilderResult Atom(object value, TimeSpan? expires = null) => AddResult(new Atom(value, expires));

        IPathValueDefinedResult IKeyDefinedResult.Ref(params KeySegment[] keys) => AddResult(new Ref(new FalcorPath(keys)));

        IPathValueDefinedResult IKeyDefinedResult.Error(string error = null) => AddResult(new Error(error));

        IPathValueDefinedResult IKeyDefinedResult.Undefined() => AddResult(null);

        IPathValueDefinedResult IKeyDefinedResult.Atom(object value, TimeSpan? expires) => AddResult(new Atom(value, expires));

        public IPathValueBuilderResult Ref(params KeySegment[] keys) => AddResult(new Ref(new FalcorPath(keys)));

        public IPathValueBuilderResult Error(string error = null) => AddResult(new Error(error));

        public IPathValueBuilderResult Undefined() => AddResult(null);
        public IEnumerator<PathValue> GetEnumerator() => _values.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

}