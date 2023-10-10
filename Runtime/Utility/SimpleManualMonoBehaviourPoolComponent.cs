using System.Collections.Generic;
using UnityEngine;


namespace com.bdeshi.helpers.Utility
{
    /// <summary>
    /// Simple pool with normal instantiation automated.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SimpleManualMonoBehaviourPoolComponent<T>: MonoBehaviour
        where T : MonoBehaviour
    {
        public int initialCount = 10;
        [SerializeField]private List<T> pool;
        [SerializeField]protected T prefab;

        private void Awake()
        {
            pool = new List<T>();
            while (initialCount > 0)
            {
                initialCount--;
                createAndAddToPool();
            }
        }

        T createItem()
        {
            return Instantiate(this.prefab, transform,false);
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
    }



}