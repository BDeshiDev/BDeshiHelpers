using System;
using System.Collections.Generic;
using UnityEngine;

namespace Bdeshi.Helpers.Utility
{
    [Serializable]
    public class PooledMonoBehaviorList<T>
    where T: MonoBehaviour
    {
        [SerializeField] private List<T> _spawnedItems = new ();
        public List<T> SpawnedItems => _spawnedItems;
        private SimpleManualMonoBehaviourPool<T> _pool;
        public SimpleManualMonoBehaviourPool<T> Pool => _pool;
        
        public void Initialize(T prefab, int initialPooledCount = 0, Transform spawnParent = null)
        {
            _pool = new SimpleManualMonoBehaviourPool<T>(prefab, initialPooledCount, spawnParent);
        }
        
        public PooledMonoBehaviorList(T prefab, int initialPooledCount = 0, Transform spawnParent = null)
        {
            _pool = new SimpleManualMonoBehaviourPool<T>(prefab, initialPooledCount, spawnParent);
        }
        
        public PooledMonoBehaviorList(SimpleManualMonoBehaviourPool<T> pool)
        {
            _pool = pool;
        }
        public T this[int key]
        {
            get => _spawnedItems[key];
        }
        /// <summary>
        /// If list has enough elements, return element at index i
        /// else, add elements to list until we have enough, then return element at index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public T Get(int index)
        {
            if (index >= _spawnedItems.Count)
            {
                return GetItem();
            }

            return _spawnedItems[index];
        }
        
        public T GetItem()
        {
            var item = _pool.GetItem();
            _spawnedItems.Add(item);
            return item;
        }
        
        public void ClearAndReturnToPool()
        {
            foreach (var item in _spawnedItems)
            {
                _pool.ReturnItem(item);
            }
            _spawnedItems.Clear();
        }
        
        public void ClearAndForget(int index)
        {
            _spawnedItems.Clear();
        }
        
        public void EnsureCount(int count)
        {
            _pool.EnsureSpawnListCount(_spawnedItems, count);
        }
        public void EnsureCount(int count, Action<T> addedCallback, Action<T> removedCallback)
        {
            _pool.EnsureSpawnListCount(_spawnedItems, count,addedCallback,removedCallback);
        }

        public void ReturnItem(T item)
        {
            _spawnedItems.Remove(item);
            _pool.ReturnItem(item);
        }
        
        public void ReturnAllItems()
        {
            foreach (var item in _spawnedItems)
            {
                _pool.ReturnItem(item);
            }
            _spawnedItems.Clear();
        }
    }
}