using UnityEngine;
using UnityEngine.Events;

namespace Bdeshi.Helpers.Utility
{
    public class EventOnAwake : MonoBehaviour
    {
        public bool CallInAwake = true;
        public bool CallInStart = false;
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
    }
}
