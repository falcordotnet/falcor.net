using System.Collections.Generic;

namespace Falcor
{
    public static class KeyHelpers
    {
        //public static NumberKey NumberKey(long key) => new NumberKey(key);
        public static NumberRange Range(int from, int to, bool inclusive = true) => new NumberRange(from, to, inclusive);
        public static KeySet KeySet(params SimpleKey[] keys) => new KeySet(keys);
        public static NumericSet NumberSet(params NumericKey[] keys) => new NumericSet(keys);
        //public static NumericSet NumberSet(params long[] keys) => NumberSet(keys);
        public static NumericSet NumberSet(IEnumerable<int> keys) => new NumericSet(keys);
    }
}