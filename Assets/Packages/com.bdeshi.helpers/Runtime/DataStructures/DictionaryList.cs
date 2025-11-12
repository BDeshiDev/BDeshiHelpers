using System.Collections;
using System.Collections.Generic;

namespace Bdeshi.Helpers.DataStructures
{
    public class DictionaryList<TKey, TVal>:
        IEnumerable<KeyValuePair<TKey, List<TVal>>>
    {
        private IDictionary<TKey, List<TVal>> _dict;
        public IDictionary<TKey, List<TVal>> Dict =>_dict;
        public IEnumerable<TKey> Keys => _dict.Keys;
        public IEnumerable<List<TVal>> Values => _dict.Values;
        
        List<List<TVal>> pool = new List<List<TVal>>();

        public DictionaryList(IDictionary<TKey, List<TVal>> dict)
        {
            _dict = dict;
        }
        
        public DictionaryList()
        {
            _dict = new Dictionary<TKey, List<TVal>>();
        }

        /// <summary>
        /// returns true if adding new List
        /// False if list exists.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Add(TKey key, TVal item)
        {
            if (_dict.TryGetValue(key, out var list))
            {
                list.Add(item);
                return true;
            }
            else
            {
                var l  =  GetNewList();
                l.Add(item);
                _dict[key] = l;
                return false;
            }
        }

        public List<TVal> GetNewList()
        {
            if (pool.Count > 0)
            {
                var l = pool[pool.Count - 1];
                pool.RemoveAt(pool.Count - 1);
                return l;
            }
            return new List<TVal>() {  };
        }
        
        /// <summary>
        /// returns true if adding new List
        /// False if list exists.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public List<TVal> GetOrAddEmptyList(TKey key)
        {
            if (_dict.TryGetValue(key, out var list))
            {
                return list;
            }
            else
            {
                list = GetNewList();
                _dict[key] = list;
                return list;
            }
        }
        
        public bool Contains(TKey key)
        {
            return _dict.ContainsKey(key);
        }
        
        public bool Contains(TKey key, TVal val)
        {
            return _dict.TryGetValue(key, out var listUnderKey) && listUnderKey.Contains(val);
        }
        
        public bool TryGetValue(TKey key, out List<TVal> list)
        {
            if (_dict.TryGetValue(key, out list))
            {
                return true;
            }
            list = null;
            return false;
        }
        
        public List<TVal> this[TKey key]
        {
            get => _dict[key];
            set => _dict[key] = value;
        }


        public IEnumerator<KeyValuePair<TKey, List<TVal>>> GetEnumerator()
        {
            return _dict.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => _dict.GetEnumerator();

        public void Clear()
        {
            foreach (var l in _dict.Values)
            {
                l.Clear();
                pool.Add(l);
            }
            _dict.Clear();
        }
    }
}