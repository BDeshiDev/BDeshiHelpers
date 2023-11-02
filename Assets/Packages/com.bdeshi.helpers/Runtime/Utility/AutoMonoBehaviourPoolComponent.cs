using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Bdeshi.Helpers.Utility
{
    public class AutoMonoBehaviourPoolComponent<T>: MonoBehaviour 
        where T : MonoBehaviour, AutoPoolable<T>
    {
        private List<T> _pool;
        private HashSet<T> _loaned;
        
        [SerializeField]protected T prefab;
        [SerializeField] private Transform _spawnParent;
        public bool Debug = false;
        public int InitialCount = 1;
        private int _spawnCount = 0;
        private void Awake()
        {
            _pool = new List<T>();
            _loaned = new HashSet<T>();
            while (InitialCount > 0)
            {
                InitialCount--;
                createAndAddToPool();
            }
        }

        T createItem()
        {
            T item = default(T);
                        
            if (_spawnParent != null)
            {
                item = Object.Instantiate(this.prefab, _spawnParent,false);
            }
            else
            {
                item = Object.Instantiate(this.prefab);
            }

            _spawnCount++;
            item.gameObject.name += "_" + _spawnCount;

            return item;
        }


        void createAndAddToPool()
        {
            var item = createItem();
            
            item.gameObject.SetActive(false);
            _pool.Add(item);
        }

        public T getItem()
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
            
            _loaned.Add(item);
            
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
            foreach (var item in _loaned)
            {
                item.handleReturned();
                handleReturnInternal(item);
            }
            
            _loaned.Clear();
        }


        void handleNormalNormalReturn(T item)
        {
            handleReturnInternal(item);
            _loaned.Remove(item);
        }

        void handleReturnInternal(T item)
        {
            item.gameObject.SetActive(false);
            item.NormalReturnCallback -= handleNormalNormalReturn;

            _pool.Add(item);
        }
    }
 
}