using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Falcor.Server.Routing
{
    public class RouteCollection : IList<Route>
    {
        private readonly List<Route> _routes = new List<Route>();
        public IEnumerator<Route> GetEnumerator() => _routes.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public void Add(Route item) => _routes.Add(item);
        public void Clear() => _routes.Clear();
        public bool Contains(Route item) => _routes.Contains(item);
        public void CopyTo(Route[] array, int arrayIndex) => _routes.CopyTo(array, arrayIndex);
        public bool Remove(Route item) => _routes.Remove(item);
        public int Count => _routes.Count;
        public bool IsReadOnly => false;
        public int IndexOf(Route item) => _routes.IndexOf(item);
        public void Insert(int index, Route item) => _routes.Insert(index, item);
        public void RemoveAt(int index) => _routes.RemoveAt(index);

        public Route this[int index]
        {
            get { return _routes[index]; }
            set { _routes[index] = value; }
        }
    }

    public interface IBatchProxy
    {
        Task<TResult> Batch<TParam, TResult>(TParam parameters, Func<TParam, TResult> batchable);
    }
}