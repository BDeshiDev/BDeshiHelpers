using UnityEngine;

namespace Bdeshi.Helpers.Utility
{
    [System.Serializable]
    public class SingleUnityLayer
    {
        [SerializeField]
        private int _layerIndex = 0;
        public int LayerIndex
        {
            get { return _layerIndex; }
        }
 
        public void Set(int _layerIndex)
        {
            if (_layerIndex > 0 && _layerIndex < 32)
            {
                this._layerIndex = _layerIndex;
            }
        }
 
        public int Mask
        {
            get { return 1 << _layerIndex; }
        }
    }
}