using UnityEngine;

namespace Bdeshi.Helpers.Utility
{
    /// <summary>
    /// Standard non-lazy singleton
    /// Registers self as instance if none exists, otherwise destroy self
    /// Dont destroy on load is set as well
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class MonoBehaviourSingletonPersistent<T> : MonoBehaviour
        where T : Component
    {
        public static T Instance { get; private set; }
        protected bool _willGetDestroyed = false;
        protected virtual void Awake()
        {
            if (Instance == null)
            {
                Instance = this as T;
                DontDestroyOnLoad(this);

                Initialize();
            }
            else
            {
                _willGetDestroyed = true;

                Destroy(gameObject);
            }
        }
        
        /// <summary>
        /// Initialize is called on awake only if this is the first instance
        /// </summary>
        protected abstract void Initialize();
    }

}