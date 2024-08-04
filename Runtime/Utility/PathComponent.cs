using UnityEngine;

namespace Bdeshi.Helpers.Utility
{
    public class PathComponent:MonoBehaviour
    {
        [SerializeField] public Color GizmoColor = Color.yellow;
        public Vector3[] Path;
        
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = GizmoColor;


            drawPathGizmos(Path);
        }

        private void drawPathGizmos(Vector3[] p)
        {
            for (int i = 0; i < p.Length; i++)
            {
                Gizmos.DrawWireSphere(p[i], .25f);
                if (i > 0)
                    Gizmos.DrawLine(p[i - 1], p[i]);
            }
        }
    }
}