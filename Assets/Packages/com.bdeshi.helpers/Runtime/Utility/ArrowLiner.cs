using UnityEngine;

namespace com.bdeshi.helpers.Utility
{
    public class ArrowLiner : MonoBehaviour
    {
        public LineRenderer mainLiner { get; private set; }
        public LineRenderer tipLiner { get; private set; }

        public float mainLineWidth = .25f;
        public float tipWidth = .25f;
        public float tipLenMax = .25f;

        [SerializeField] Vector3 startPoint;
        [SerializeField] Vector3 endPoint;

        public Vector3 mainLineEndPoint { get; private set; }

        public void setColor(Color color)
        {
            mainLiner.startColor = mainLiner.endColor = tipLiner.startColor = tipLiner.endColor = color;
        }

        private void Awake()
        {
            mainLiner = GetComponent<LineRenderer>();
            tipLiner = transform.GetChild(0).GetComponent<LineRenderer>();
        }

        private void OnValidate()
        {
            if (mainLiner == null)
            {
                mainLiner = GetComponent<LineRenderer>();
            }

            if (tipLiner == null)
            {
                tipLiner = transform.GetChild(0).GetComponent<LineRenderer>();
            }

            mainLiner.widthMultiplier = mainLineWidth;
            tipLiner.widthMultiplier = tipWidth;

            updateLineRenderers();
        }

        public void updateArrowEndPoints(Vector3 start,Vector3 end)
        {
            startPoint = start;
            endPoint = end;

            updateLineRenderers();
        }

        private void updateLineRenderers()
        {
            Vector3 dir = endPoint - startPoint;
            float len = dir.magnitude;

            mainLineEndPoint = startPoint + dir.normalized * Mathf.Max(len - tipLenMax, 0);
            mainLiner.SetPosition(0, startPoint);
            mainLiner.SetPosition(1, mainLineEndPoint);

            tipLiner.SetPosition(0, mainLineEndPoint);
            tipLiner.SetPosition(1, endPoint);
        }

        public void toggleLineRenderers(bool shouldBeOn)
        {
            mainLiner.enabled = tipLiner.enabled = shouldBeOn;
        }
    }
}
