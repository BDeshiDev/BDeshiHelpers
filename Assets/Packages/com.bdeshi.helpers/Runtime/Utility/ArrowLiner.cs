using UnityEngine;

namespace Bdeshi.Helpers.Utility
{
    public class ArrowLiner : MonoBehaviour
    {
        public LineRenderer MainLiner { get; private set; }
        public LineRenderer TipLiner { get; private set; }

        public float MainLineWidth = .25f;
        public float TipWidth = .25f;
        public float TipLenMax = .25f;

        [SerializeField] Vector3 _startPoint;
        [SerializeField] Vector3 _endPoint;

        public Vector3 MainLineEndPoint { get; private set; }

        public void SetColor(Color color)
        {
            MainLiner.startColor = MainLiner.endColor = TipLiner.startColor = TipLiner.endColor = color;
        }

        private void Awake()
        {
            MainLiner = GetComponent<LineRenderer>();
            TipLiner = transform.GetChild(0).GetComponent<LineRenderer>();
        }

        private void OnValidate()
        {
            if (MainLiner == null)
            {
                MainLiner = GetComponent<LineRenderer>();
            }

            if (TipLiner == null)
            {
                TipLiner = transform.GetChild(0).GetComponent<LineRenderer>();
            }

            MainLiner.widthMultiplier = MainLineWidth;
            TipLiner.widthMultiplier = TipWidth;

            updateLineRenderers();
        }

        public void updateArrowEndPoints(Vector3 start,Vector3 end)
        {
            _startPoint = start;
            _endPoint = end;

            updateLineRenderers();
        }

        private void updateLineRenderers()
        {
            Vector3 dir = _endPoint - _startPoint;
            float len = dir.magnitude;

            MainLineEndPoint = _startPoint + dir.normalized * Mathf.Max(len - TipLenMax, 0);
            MainLiner.SetPosition(0, _startPoint);
            MainLiner.SetPosition(1, MainLineEndPoint);

            TipLiner.SetPosition(0, MainLineEndPoint);
            TipLiner.SetPosition(1, _endPoint);
        }

        public void toggleLineRenderers(bool shouldBeOn)
        {
            MainLiner.enabled = TipLiner.enabled = shouldBeOn;
        }
    }
}
