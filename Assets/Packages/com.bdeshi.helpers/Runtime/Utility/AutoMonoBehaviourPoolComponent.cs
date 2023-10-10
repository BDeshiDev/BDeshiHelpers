using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace com.bdeshi.helpers.Utility
{
    public class AutoMonoBehaviourPoolComponent<T>: MonoBehaviour 
        where T : MonoBehaviour, AutoPoolable<T>
    {
        private List<T> pool;
        private HashSet<T> loaned;
        
        [SerializeField]protected T prefab;
        [SerializeField] private Transform spawnParent;
        public bool debug = false;
        public int initialCount = 1;
        private int spawnCount = 0;
        private void Awake()
        {
            pool = new List<T>();
            loaned = new HashSet<T>();
            while (initialCount > 0)
            {
                initialCount--;
                createAndAddToPool();
            }
        }



        T createItem()
        {
            T item = default(T);
                        
            if (spawnParent != null)
            {
                item = Object.Instantiate(this.prefab, spawnParent,false);
            }
            else
            {
                item = Object.Instantiate(this.prefab);
            }

            spawnCount++;
            item.gameObject.name += "_" + spawnCount;

            return item;
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
                item.gameObject.SetActive(true);
            }
            else
            {
                item = createItem();
            }
            item.NormalReturnCallback += handleNormalNormalReturn;
            item.initialize();
            
            loaned.Add(item);
            
            return item;
        }

        public void ensurePoolHasAtleast(int count)
        {
            for (int i = pool.Count; i <= count; i++)
            {
                createAndAddToPool();
            }
        }

        public void returnAll()
        {
            foreach (var item in loaned)
            {
                item.handleReturned();
                handleReturnInternal(item);
            }
            
            loaned.Clear();
        }


        void handleNormalNormalReturn(T item)
        {
            handleReturnInternal(item);
            loaned.Remove(item);
        }

        void handleReturnInternal(T item)
        {
            item.gameObject.SetActive(false);
            item.NormalReturnCallback -= handleNormalNormalReturn;

            pool.Add(item);
        }
    }
 
}