using System;
using UnityEngine;
using UnityEngine.Events;

namespace Bdeshi.Helpers.Utility
{
    public class EventOnAwake : MonoBehaviour
    {
        public bool CallInAwake = true;
        public bool CallInStart = false;
        public  bool logEnable;
        public UnityEvent e;
        private void Awake()
        {
            if(CallInAwake)
                e.Invoke();
        }

        private void Start()
        {
            if(CallInStart)
                e.Invoke();
        }

        private void OnEnable()
        {
            if (logEnable)
            {
                Debug.Log($"ENABLE LOGGGGGGGGGG {this}", gameObject);
            } 
        }
        
        private void OnDisable()
        {
            if (logEnable)
            {
                Debug.Log($"DISABLE LOGGGGGGGGGG {this}", gameObject);
            } 
        }
    }
}
