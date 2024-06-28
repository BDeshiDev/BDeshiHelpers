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
        public bool IsEmpty => _list.Count == 0;
        public bool Contains(T item) => _list.Contains(item);
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
    }
}