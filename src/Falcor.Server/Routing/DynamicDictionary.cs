using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;

namespace Falcor.Server.Routing
{
    /*    
    The MIT License (MIT)

    Copyright (c) 2014 Randy Burden ( http://randyburden.com ) All rights reserved.

    Permission is hereby granted, free of charge, to any person obtaining a copy
    of this software and associated documentation files (the "Software"), to deal
    in the Software without restriction, including without limitation the rights
    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
    copies of the Software, and to permit persons to whom the Software is
    furnished to do so, subject to the following conditions:

    The above copyright notice and this permission notice shall be included in
    all copies or substantial portions of the Software.

    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
    THE SOFTWARE.
*/

    /// <summary>
    ///     A dynamic dictionary allowing case-insensitive access and returns null when accessing non-existent properties.
    /// </summary>
    /// <example>
    ///     // Non-existent properties will return null
    ///     dynamic obj = new DynamicDictionary();
    ///     var firstName = obj.FirstName;
    ///     Assert.Null( firstName );
    ///     // Allows case-insensitive property access
    ///     dynamic obj = new DynamicDictionary();
    ///     obj.SuperHeroName = "Superman";
    ///     Assert.That( obj.SUPERMAN == "Superman" );
    ///     Assert.That( obj.superman == "Superman" );
    ///     Assert.That( obj.sUpErMaN == "Superman" );
    /// </example>
    internal sealed class DynamicDictionary : DynamicObject, IDictionary<string, object>
    {
        private readonly IDictionary<string, object> _dictionary =
            new DefaultValueDictionary<string, object>(StringComparer.InvariantCultureIgnoreCase);

        #region IEnumerable<KeyValuePair<string,object>> Members

        [DebuggerStepThrough]
        public IEnumerator<KeyValuePair<string, object>> GetEnumerator() => _dictionary.GetEnumerator();

        #endregion IEnumerable<KeyValuePair<string,object>> Members

        #region IEnumerable Members

        [DebuggerStepThrough]
        IEnumerator IEnumerable.GetEnumerator() => _dictionary.GetEnumerator();

        #endregion IEnumerable Members

        #region Nested Types

        /// <summary>
        ///     A dictionary that returns the default value when accessing keys that do not exist in the dictionary.
        /// </summary>
        public class DefaultValueDictionary<TKey, TValue> : IDictionary<TKey, TValue>
        {
            private readonly IDictionary<TKey, TValue> _dictionary;

            #region IEnumerable<KeyValuePair<TKey,TValue>> Members

            [DebuggerStepThrough]
            public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => _dictionary.GetEnumerator();

            #endregion IEnumerable<KeyValuePair<TKey,TValue>> Members

            #region IEnumerable Members

            [DebuggerStepThrough]
            IEnumerator IEnumerable.GetEnumerator() => _dictionary.GetEnumerator();

            #endregion IEnumerable Members

            #region Constructors

            public DefaultValueDictionary()
            {
                _dictionary = new Dictionary<TKey, TValue>();
            }

            /// <summary>
            ///     Initializes with an existing dictionary.
            /// </summary>
            /// <param name="dictionary"></param>
            public DefaultValueDictionary(IDictionary<TKey, TValue> dictionary)
            {
                _dictionary = dictionary;
            }

            /// <summary>
            ///     Initializes using the given equality comparer.
            /// </summary>
            /// <param name="comparer"></param>
            public DefaultValueDictionary(IEqualityComparer<TKey> comparer)
            {
                _dictionary = new Dictionary<TKey, TValue>(comparer);
            }

            #endregion Constructors

            #region IDictionary<string,TValue> Members

            public void Add(TKey key, TValue value) => _dictionary.Add(key, value);

            public bool ContainsKey(TKey key) => _dictionary.ContainsKey(key);

            public ICollection<TKey> Keys => _dictionary.Keys;

            public bool Remove(TKey key) => _dictionary.Remove(key);

            public bool TryGetValue(TKey key, out TValue value) => _dictionary.TryGetValue(key, out value);

            public ICollection<TValue> Values => _dictionary.Values;

            public TValue this[TKey key]
            {
                get
                {
                    TValue value;

                    _dictionary.TryGetValue(key, out value);

                    return value;
                }
                set { _dictionary[key] = value; }
            }

            #endregion IDictionary<string,TValue> Members

            #region ICollection<KeyValuePair<string,TValue>> Members

            public void Add(KeyValuePair<TKey, TValue> item) => _dictionary.Add(item);

            public void Clear() => _dictionary.Clear();

            public bool Contains(KeyValuePair<TKey, TValue> item) => _dictionary.Contains(item);

            public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
                => _dictionary.CopyTo(array, arrayIndex);

            public int Count => _dictionary.Count;

            public bool IsReadOnly => _dictionary.IsReadOnly;

            public bool Remove(KeyValuePair<TKey, TValue> item) => _dictionary.Remove(item);

            #endregion ICollection<KeyValuePair<TKey,TValue>> Members
        }

        #endregion Nested Types

        #region DynamicObject Overrides

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = _dictionary[binder.Name];

            return true;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            if (_dictionary.ContainsKey(binder.Name))
            {
                _dictionary[binder.Name] = value;
            }
            else
            {
                _dictionary.Add(binder.Name, value);
            }

            return true;
        }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            if (_dictionary.ContainsKey(binder.Name) && _dictionary[binder.Name] is Delegate)
            {
                var delegateValue = _dictionary[binder.Name] as Delegate;

                result = delegateValue.DynamicInvoke(args);

                return true;
            }

            return base.TryInvokeMember(binder, args, out result);
        }

        #endregion DynamicObject Overrides

        #region IDictionary<string,object> Members

        public void Add(string key, object value) => _dictionary.Add(key, value);

        public bool ContainsKey(string key) => _dictionary.ContainsKey(key);

        public ICollection<string> Keys => _dictionary.Keys;

        public bool Remove(string key) => _dictionary.Remove(key);

        public bool TryGetValue(string key, out object value) => _dictionary.TryGetValue(key, out value);

        public ICollection<object> Values => _dictionary.Values;

        public object this[string key]
        {
            get
            {
                object value = null;

                _dictionary.TryGetValue(key, out value);

                return value;
            }
            set { _dictionary[key] = value; }
        }

        #endregion IDictionary<string,object> Members

        #region ICollection<KeyValuePair<string,object>> Members

        public void Add(KeyValuePair<string, object> item) => _dictionary.Add(item);

        public void Clear() => _dictionary.Clear();

        public bool Contains(KeyValuePair<string, object> item) => _dictionary.Contains(item);

        public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
            => _dictionary.CopyTo(array, arrayIndex);

        public int Count => _dictionary.Count;

        public bool IsReadOnly => _dictionary.IsReadOnly;

        public bool Remove(KeyValuePair<string, object> item) => _dictionary.Remove(item);

        #endregion ICollection<KeyValuePair<string,object>> Members
    }
}