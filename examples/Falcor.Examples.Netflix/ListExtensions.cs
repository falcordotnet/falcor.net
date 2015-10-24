using System.Collections.Generic;

namespace Falcor.Examples.Netflix
{
    public static class ListExtensions
    {
        public static void Add<T>(this List<T> list, params T[] items) => list.AddRange(items);
    }
}