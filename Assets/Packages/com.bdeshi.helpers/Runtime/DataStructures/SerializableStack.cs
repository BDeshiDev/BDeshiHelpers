using System;
using System.Collections.Generic;
using UnityEngine;

namespace Bdeshi.Helpers.DataStructures
{
    [Serializable]
    public class SerializableStack<T>
    {
        [SerializeField] private T _last;
        [SerializeField] private List<T> _list = new List<T>();
        public List<T> BackingList => _list;
        public bool IsEmpty => _list.Count == 0;
        public bool Contains(T item) => _list.Contains(item);
        public int Count => _list.Count;

        
        /// <summary>
        /// LIFO order. NOTE: GC WARNING
        /// </summary>
        /// <returns></returns>
        public IEnumerable<T> LIFOOrder(){
            for (int i = _list.Count - 1; i >= 0; i--)
            {
                yield return _list[i];   
            }
        }
        /// <summary>
        /// FIFO order. Can get same result by looping backingList
        /// </summary>
        /// <returns></returns>
        public IEnumerable<T> FIFOOrder() => _list;

        public T Peek() => _last;
        public void Push(T item)
        {
            _list.Add(item);
            _last = item;
        }

        public T Pop()
        {
            var popped = _last;
            _list.RemoveAt(_list.Count -1);
            if (_list.Count <=0)
            {
                _last = default(T);
            }
            else
            {
                _last = _list[^1];
            }
            return popped;
        }

        public bool TryPop(out T item)
        {
            if (_list.Count <= 0)
            {
                item = default;
                return false;
            }
            else
            {
                item = Pop();
                return true;
            }
        }

        public void Clear() => _list.Clear();
    }
}