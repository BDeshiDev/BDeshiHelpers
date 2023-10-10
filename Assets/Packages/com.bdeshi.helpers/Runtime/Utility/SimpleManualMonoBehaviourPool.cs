using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;



namespace com.bdeshi.helpers.Utility
{
    /// <summary>
    /// Simple pool with normal instantiation automated.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SimpleManualMonoBehaviourPool<T> where T : MonoBehaviour
    {
        private List<T> pool;
        protected T prefab;
        public Transform spawnParent;

        public SimpleManualMonoBehaviourPool(T prefab, int initialCount, Transform spawnParent = null)
        {
            this.prefab = prefab;
            this.spawnParent = spawnParent;
            pool = new List<T>();
            while (initialCount > 0)
            {
                initialCount--;
                createAndAddToPool();
            }
        }

        T createItem()
        {
            if (spawnParent != null)
            {
                return Object.Instantiate(this.prefab, spawnParent,false);
            }

            return Object.Instantiate(this.prefab);
        }


        void createAndAddToPool()
        {
            var item = createItem();
            item.gameObject.SetActive(false);
            pool.Add(item);
        }

        public T getItem()
        {
            T item = null;
            if (pool.Count > 0)
            {
                item = pool[pool.Count -1];
                pool.RemoveAt(pool.Count - 1);
                item.transform.SetParent(spawnParent, false); 
                item.gameObject.SetActive(true);
            }
            else
            {
                item = createItem();
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
            item.gameObject.SetActive(false);
            pool.Add(item);
        }
        
        /// <summary>
        /// Will fill/trim a list to match a desired count using  pool
        /// excess is returned to pool
        /// additional items are fetched from pool
        /// </summary>
        /// <param name="spawnedList"></param>
        /// <param name="desiredSpawnCount"></param>
        public void updateSpawnListSize(List<T> spawnedList, int desiredSpawnCount)
        {
            if (spawnedList.Count != desiredSpawnCount)
            {
                for (int i = spawnedList.Count; i <= desiredSpawnCount; i++)
                {
                    spawnedList.Add(getItem());
                }
                for (int i = spawnedList.Count-1 ; i >= desiredSpawnCount; i--)
                {
                    var item = spawnedList[i];
                    spawnedList.RemoveAt(i);
                
                    returnItem(item);
                }
            }
            
        }
    }



}