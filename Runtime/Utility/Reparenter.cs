using UnityEngine;

namespace com.bdeshi.helpers.Utility
{
    public class Reparenter: MonoBehaviour
    {
        public Transform newParent = null;
        private void Awake()
        {
            transform.parent = newParent;
        }
    }
}