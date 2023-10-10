using UnityEngine;

namespace com.bdeshi.helpers.Utility
{
    public class MonoBehaviourLazySingleton<T> : MonoBehaviour
        where T : MonoBehaviourLazySingleton<T> 
    {
        private static T _instance;
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {

                    if (appicationQuitting)
                    {
                        Debug.Log("app quit  " + _instance, _instance);

                        return null;

                    }


                    _instance = FindObjectOfType<T>();
                    if (_instance == null)
                    {
                        GameObject obj = new GameObject(typeof(T).ToString());
                        // Debug.Log(obj + "create");
                        //obj.hideFlags = HideFlags.HideAndDontSave;
                        _instance = obj.AddComponent<T>();
                        _instance.initialize();
                    }
                }
                // Debug.Log("end "+ (_instance == null));

                return _instance;
            }
        }


        private static bool appicationQuitting;

        protected virtual void initialize() { }

        protected virtual void Awake()
        {
            if (_instance == null)
            {
                _instance = this as T;
                DontDestroyOnLoad(gameObject);
            }
            else if(_instance != this)
            {
                Destroy(gameObject);
            }
        }


        private void OnApplicationQuit()
        {
            appicationQuitting = true;
        }

        public static void PlayModeEnterCleanup()
        {
            appicationQuitting = false;

            if (_instance != null)
                _instance.PlayModeEnterCleanupInternal();
        }
        
        public static void PlayModeExitCleanup()
        {
            _instance = null;
        }
        
        protected virtual void PlayModeEnterCleanupInternal()
        {

        }
    }
}
