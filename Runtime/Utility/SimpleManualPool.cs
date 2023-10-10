using System;
using System.Collections.Generic;
using UnityEngine;


namespace com.bdeshi.helpers.Utility
{
    /// <summary>
    /// manual pool for POCOs
    /// Creation is done through the Func
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SimpleManualPool<T>
    {
        [SerializeField] private List<T> pool;
        private Func<T> CreationMethod;

        public SimpleManualPool(Func<T> creationMethod)
        {
            CreationMethod = creationMethod;
            pool = new List<T>();
        }

        void createAndAddToPool()
        {
            var item = CreationMethod();

            pool.Add(item);
        }

        public T getItem()
        {
            T item = default;
            if (pool.Count > 0)
            {
                item = pool[pool.Count -1];
                pool.RemoveAt(pool.Count - 1);
            }
            else
            {
                item = CreationMethod();
            }

            return item;
        }

        public void ensurePoolHasAtleast(int count)
        {
            for (int i = pool.Count; i <= count; i++)
            {
                createAndAddToPool();
            }
        }

        public void returnItem(T item)
        {
            pool.Add(item);
        }
    }
}