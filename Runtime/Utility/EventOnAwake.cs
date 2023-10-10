using UnityEngine;
using UnityEngine.Events;

namespace com.bdeshi.helpers.Utility
{
    public class EventOnAwake : MonoBehaviour
    {
        public bool callInAwake = true;
        public bool callInStart = false;
        public UnityEvent e;
        private void Awake()
        {
            if(callInAwake)
                e.Invoke();
        }

        private void Start()
        {
            if(callInStart)
                e.Invoke();
        }
    }
}
