using System;
using System.Collections.Generic;
using UnityEngine;


namespace Bdeshi.Helpers.Utility
{
    /// <summary>
    /// manual pool for POCOs
    /// Creation is done through the Func
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SimpleManualPool<T>
    {
        [SerializeField] private List<T> _pool;
        private readonly Func<T> _creationMethod;

        public SimpleManualPool(Func<T> creationMethod)
        {
            _creationMethod = creationMethod;
            _pool = new List<T>();
        }

        void CreateAndAddToPool()
        {
            var item = _creationMethod();

            _pool.Add(item);
        }

        public T GetItem()
        {
            T item = default;
            if (_pool.Count > 0)
            {
                item = _pool[_pool.Count -1];
                _pool.RemoveAt(_pool.Count - 1);
            }
            else
            {
                item = _creationMethod();
            }

            return item;
        }

        public void EnsurePoolHasAtleast(int count)
        {
            for (int i = _pool.Count; i <= count; i++)
            {
                CreateAndAddToPool();
            }
        }

        public void ReturnItem(T item)
        {
            _pool.Add(item);
        }
    }
}