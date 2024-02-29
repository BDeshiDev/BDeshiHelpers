using System;
using System.Collections.Generic;
using UnityEngine;

namespace Bdeshi.Helpers.Utility
{
    [Serializable]
    public class PooledMonoBehaviorList<T>
    where T: MonoBehaviour
    {
        private List<T> _list = new ();
        public List<T> List => _list;
        private SimpleManualMonoBehaviourPool<T> _pool;
        public SimpleManualMonoBehaviourPool<T> Pool => _pool;
        
        public PooledMonoBehaviorList(T prefab, int initialPooledCount = 0, Transform spawnParent = null)
        {
            _pool = new SimpleManualMonoBehaviourPool<T>(prefab, initialPooledCount, spawnParent);
        }

        /// <summary>
        /// If list has enough elements, return element at index i
        /// else, add elements to list until we have enough, then return element at index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public T Get(int index)
        {
            if (_list.Count <= index)
            {
                _pool.EnsureSpawnListCount(_list, index+1);
            }

            return _list[index];
        }
        
        public T GetNewItem()
        {
            var item = _pool.GetItem();
            _list.Add(item);
            return item;
        }
        
        public void ClearAndReturnToPool()
        {
            foreach (var item in _list)
            {
                _pool.ReturnItem(item);
            }
            _list.Clear();
        }
        
        public void ClearAndForget(int index)
        {
            _list.Clear();
        }
        
        public void EnsureCount(int count)
        {
            _pool.EnsureSpawnListCount(_list, count);
        }
        public void EnsureCount(int count, Action<T> addedCallback, Action<T> removedCallback)
        {
            _pool.EnsureSpawnListCount(_list, count,addedCallback,removedCallback);
        }
    }
}