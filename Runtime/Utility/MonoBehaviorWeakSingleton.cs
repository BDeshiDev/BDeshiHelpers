using System;
using UnityEngine;

namespace Bdeshi.Helpers.Utility
{
    /// <summary>
    /// Singletons with static access but no strict maintenance of single instance 
    /// DontDestroyOnLoad is not used, so it'll destroy itself on scene unload.
    /// Useful for static instances that change on scene load
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class MonoBehaviorWeakSingleton<T>:MonoBehaviour
        where T : MonoBehaviorWeakSingleton<T> 
    {
        protected static T _instance;
        public static T Instance => _instance;
        protected abstract void Initialize();
        private void Awake()
        {
            _instance = this as T;
            Initialize();
        }
    }
}