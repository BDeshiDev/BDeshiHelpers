using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Bdeshi.Helpers.Utility
{
    public class AutoMonobehaviourPool<T> where T : MonoBehaviour, AutoPoolable<T>
    {
        private List<T> _pool;
        private HashSet<T> loaned = new HashSet<T>();
        
        protected T prefab;
        private Transform spawnParent;
        public bool debug = false;

        public AutoMonobehaviourPool(T prefab, int initialCount, Transform spawnParent = null)
        {
            this.prefab = prefab;
            this.spawnParent = spawnParent;
            _pool = new List<T>();
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


            return item;
        }


        void createAndAddToPool()
        {
            var item = createItem();
            
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
            for (int i = _pool.Count; i <= count; i++)
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

            _pool.Add(item);
        }
    }
    
    public interface AutoPoolable<T>
    {
        void initialize();
        /// <summary>
        /// Do cleanup before returning to pool
        /// SHOULD NOT INVOKE NormalReturnCallback
        /// </summary>
        void handleReturned();
        event Action<T> NormalReturnCallback;
    }
}