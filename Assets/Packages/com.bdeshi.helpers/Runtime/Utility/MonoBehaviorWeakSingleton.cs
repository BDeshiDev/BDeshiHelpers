using System;
using UnityEngine;

namespace Bdeshi.Helpers.Utility
{
    /// <summary>
    /// Sets self as Instance without checking
    /// BE SURE THAT THIS IS WHAT YOU WANT
    /// DOESN'T HANDLE DUPLICATES
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