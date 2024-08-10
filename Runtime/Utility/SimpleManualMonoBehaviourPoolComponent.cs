using System.Collections.Generic;
using UnityEngine;


namespace Bdeshi.Helpers.Utility
{
    /// <summary>
    /// Simple pool with normal instantiation automated.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SimpleManualMonoBehaviourPoolComponent<T>: MonoBehaviour
        where T : MonoBehaviour
    {
        public int InitialCount = 10;
        [SerializeField] private List<T> _pool;
        [SerializeField] protected T _prefab;

        private void Awake()
        {
            _pool = new List<T>();
            while (InitialCount > 0)
            {
                InitialCount--;
                CreateAndAddToPool();
            }
        }

        T CreateItem()
        {
            return Instantiate(this._prefab, transform,false);
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
    }



}