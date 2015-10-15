using System;

namespace Falcor
{
    public abstract class FalcorNode
    {
        public abstract bool IsValue();
        public bool IsTree() => !IsValue();
        public virtual FalcorValue AsValue() => null;
        public virtual FalcorTree AsTree() => null;
        public abstract T Match<T>(Func<FalcorValue, T> value, Func<FalcorTree, T> tree);
        public FalcorValue Get(FalcorPath path) => Match(value => value, tree =>
        {
            FalcorNode child;
            //jif (!(path.IsEmpty() || !tree.Children.TryGetValue(path.First(), out child)))
                //return child.Get(path);
            return null;
        });

        public bool Contains(FalcorPath path) => Get(path) != null;

        public FalcorNode Put(FalcorPath path, FalcorValue newValue)
        {
            throw new NotImplementedException();
            //if (path.IsEmpty()) throw new ArgumentException($"{nameof(path)} is empty");
            //return Match(value =>
            //{
            //    throw new InvalidOperationException("put on value node");
            //}, tree =>
            //{
            //    var head = path.First();
            //});
        }


    }
}