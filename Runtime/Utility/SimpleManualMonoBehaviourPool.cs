using System;
using System.Collections.Generic;
using UnityEngine;



namespace Bdeshi.Helpers.Utility
{
    /// <summary>
    /// Simple pool with normal instantiation automated.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SimpleManualMonoBehaviourPool<T> where T : MonoBehaviour
    {
        private List<T> _pool;
        protected T _prefab;
        public int PoolReserveCount => _pool.Count;

        public Transform SpawnParent;

        public SimpleManualMonoBehaviourPool(T prefab, int initialCount, Transform spawnParent = null)
        {
            this._prefab = prefab;
            this.SpawnParent = spawnParent;
            _pool = new List<T>();
            while (initialCount > 0)
            {
                initialCount--;
                CreateAndAddToPool();
            }
        }

        T CreateItem()
        {
            if (SpawnParent != null)
            {
                return UnityEngine.Object.Instantiate(this._prefab, SpawnParent,false);
            }

            return UnityEngine.Object.Instantiate(this._prefab);
        }


        void CreateAndAddToPool()
        {
            var item = CreateItem();
            item.gameObject.SetActive(false);
            _pool.Add(item);
        }

        public T GetItem()
        {
            T item = null;
            if (_pool.Count > 0)
            {
                item = _pool[_pool.Count -1];
                _pool.RemoveAt(_pool.Count - 1);
                item.transform.SetParent(SpawnParent, false); 
                item.gameObject.SetActive(true);
            }
            else
            {
                item = CreateItem();
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
            item.gameObject.SetActive(false);
            _pool.Add(item);
        }
        
        public void RemoveFromSpawnList(List<T> spawnList)
        {
            EnsureSpawnListCount(spawnList, 0);
        }
        
        
        /// <summary>
        /// Will fill/trim a list to match a desired count using  pool
        /// excess is returned to pool
        /// additional items are fetched from pool
        /// </summary>
        /// <param name="spawnedList"></param>
        /// <param name="desiredSpawnCount"></param>
        public void EnsureSpawnListCount(List<T> spawnedList, int desiredSpawnCount)
        {
            if (spawnedList.Count != desiredSpawnCount)
            {
                for (int i = spawnedList.Count; i <= desiredSpawnCount; i++)
                {
                    spawnedList.Add(GetItem());
                }
                for (int i = spawnedList.Count-1 ; i >= desiredSpawnCount; i--)
                {
                    var item = spawnedList[i];
                    spawnedList.RemoveAt(i);
                
                    ReturnItem(item);
                }
            }
        }
        
        /// <summary>
        /// Will fill/trim a list to match a desired count using  pool
        /// excess is returned to pool
        /// additional items are fetched from pool
        /// </summary>
        /// <param name="spawnedList"></param>
        /// <param name="desiredSpawnCount"></param>
        public void EnsureSpawnListCount(List<T> spawnedList, int desiredSpawnCount,Action<T> addedCallback, Action<T> removedCallback)
        {
            if (spawnedList.Count != desiredSpawnCount)
            {
                for (int i = spawnedList.Count; i <= desiredSpawnCount; i++)
                {
                    var item = GetItem();
                    spawnedList.Add(item);
                    addedCallback.Invoke(item);
                }
                for (int i = spawnedList.Count-1 ; i >= desiredSpawnCount; i--)
                {
                    var item = spawnedList[i];
                    spawnedList.RemoveAt(i);
                
                    ReturnItem(item);
                    removedCallback.Invoke(item);
                }
            }
        }
        
                /// <summary>
        /// Will fill/trim a list to match a desired count using  pool
        /// excess is returned to pool
        /// additional items are fetched from pool
        /// </summary>
        /// <param name="spawnedList"></param>
        /// <param name="desiredSpawnCount"></param>
        public void EnsureSpawnListCount(List<T> spawnedList, int desiredSpawnCount,Action<T> addedCallback)
        {
            if (spawnedList.Count != desiredSpawnCount)
            {
                for (int i = spawnedList.Count; i <= desiredSpawnCount; i++)
                {
                    var item = GetItem();
                    spawnedList.Add(item);
                    addedCallback.Invoke(item);
                }
                for (int i = spawnedList.Count-1 ; i >= desiredSpawnCount; i--)
                {
                    var item = spawnedList[i];
                    spawnedList.RemoveAt(i);
                
                    ReturnItem(item);
                }
            }
        }
    }



}