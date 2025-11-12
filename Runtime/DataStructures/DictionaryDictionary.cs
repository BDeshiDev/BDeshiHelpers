using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bdeshi.Helpers.DataStructures
{
    public class DictionaryDictionary<TKey1, TKey2, TVal>:
        IEnumerable<KeyValuePair<TKey1, Dictionary<TKey2, TVal>>>
    {
        private IDictionary<TKey1, Dictionary<TKey2, TVal>> _dict;
        public IDictionary<TKey1, Dictionary<TKey2, TVal>> Dict =>_dict;
        public IEnumerable<TKey1> Keys => _dict.Keys;
        public IEnumerable<Dictionary<TKey2, TVal>> Values => _dict.Values;
        List<Dictionary<TKey2, TVal>> pool = new List<Dictionary<TKey2, TVal>>();

        public DictionaryDictionary(IDictionary<TKey1, Dictionary<TKey2, TVal>> dict)
        {
            _dict = dict;
        }
        
        public DictionaryDictionary()
        {
            _dict = new Dictionary<TKey1, Dictionary<TKey2, TVal>>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key1"></param>
        /// <param name="key2"></param>
        /// <param name="item"></param>
        /// <returns>True if new dict added, false if overwrite key in same dict</returns>
        public bool AddOrSet(TKey1 key1, TKey2 key2, TVal item)
        {
            if (!_dict.TryGetValue(key1, out var innerDict))
            {
                _dict[key1] = innerDict = GetInnerDict();
                innerDict[key2] = item;
                return true;
            }

            innerDict[key2] = item;
            return false;
        }

        private  Dictionary<TKey2, TVal> GetInnerDict()
        {
            if (pool.Count > 0)
            {
                var d = pool[pool.Count - 1];
                pool.RemoveAt(pool.Count - 1);
                return d;
            }
            return new Dictionary<TKey2, TVal>() {};
        }

        /// <summary>
        /// Adds val without checking, if inner dict doesn't exist, throw errors
        /// </summary>
        /// <param name="key1"></param>
        /// <param name="key2"></param>
        /// <param name="item"></param>
        public void Set(TKey1 key1, TKey2 key2, TVal item)
        {
            _dict[key1][key2] = item;
        }
        
        public bool Contains(TKey1 key1)
        {
            return _dict.ContainsKey(key1);
        }
        
        public bool Contains(TKey1 key1, TKey2 key2)
        {
            if (_dict.TryGetValue(key1, out var innerDict))
            {
                return innerDict.ContainsKey(key2);
            }

            return false;
        }
        
        public bool TryGetInnerDict(TKey1 key, out Dictionary<TKey2, TVal> val)
        {
            return _dict.TryGetValue(key, out val);
        }
        
        public bool TryGetInnerVal(TKey1 key1, TKey2 key2,out TVal val)
        {
            if (_dict.TryGetValue(key1, out var innerDict))
            {
                return innerDict.TryGetValue(key2, out val);
            }

            val = default(TVal);
            return false;
        }

        public Dictionary<TKey2, TVal> EnsureEmptyInnerDict(TKey1 key1)
        {
            Dictionary<TKey2, TVal> d;
            if (_dict.TryGetValue(key1, out var innerDict))
            {
                d = _dict[key1];
                d.Clear();
            }
            else
            {
                d = _dict[key1] = new();
            }

            return d;
        }
        
        public Dictionary<TKey2, TVal> this[TKey1 key1]
        {
            get => _dict[key1];
            set => _dict[key1] = value;
        }
        
        public TVal this[TKey1 key1, TKey2 key2]
        {
            get => _dict[key1][key2];
            set => AddOrSet(key1, key2,value);
        }


        public IEnumerator<KeyValuePair<TKey1, Dictionary<TKey2, TVal>>> GetEnumerator()
        {
            return _dict.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Clear()
        {
            foreach (var id in _dict.Values) 
            {
                id.Clear();
                pool.Add(id);
            }
            _dict.Clear();
        }

        public bool TryGetValue(TKey1 key1, TKey2 key2, out TVal o)
        {
            if (_dict.TryGetValue(key1, out var innerDict))
            {
                return innerDict.TryGetValue(key2, out o);
            }

            o = default;
            return false;
        }
    }
}