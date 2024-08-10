using UnityEngine;

namespace Bdeshi.Helpers.Utility
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