using System.Collections.Generic;
using UnityEngine;

namespace Bdeshi.Helpers.DataStructures
{
    public class SerializableStack<T>
    {
        [SerializeField] private List<T> _list = new List<T>();
        [SerializeField] private T _last;
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