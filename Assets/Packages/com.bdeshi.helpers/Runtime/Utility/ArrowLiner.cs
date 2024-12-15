using UnityEngine;

namespace Bdeshi.Helpers.Utility
{
    public class ArrowLiner : MonoBehaviour
    {
        public LineRenderer MainLiner;
        public LineRenderer TipLiner;

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

            SyncLineRenderersWithEndPoints();
        }

        public void updateArrowEndPoints(Vector3 start,Vector3 end)
        {
            _startPoint = start;
            _endPoint = end;

            SyncLineRenderersWithEndPoints();
        }

        public void SyncLineRenderersWithEndPoints(bool tipAtEndpoints = false)
        {
            Vector3 dir = _endPoint - _startPoint;
            float len = dir.magnitude;

            MainLineEndPoint = _startPoint + dir.normalized * Mathf.Max(len - TipLenMax, 0);
            MainLiner.SetPosition(0, _startPoint);
            MainLiner.SetPosition(1, MainLineEndPoint);

            TipLiner.SetPosition(0, MainLineEndPoint);
            TipLiner.SetPosition(1, _endPoint);
        }

        public void SetPositions(Vector3 mainLinerFrom, Vector3 mainLinerTo, Vector3 tipLinerTo)
        {            
            MainLiner.SetPosition(0, mainLinerFrom);
            MainLiner.SetPosition(1, mainLinerTo);

            TipLiner.SetPosition(0, mainLinerTo);
            TipLiner.SetPosition(1, tipLinerTo);
        }

        public void toggleLineRenderers(bool shouldBeOn)
        {
            MainLiner.enabled = TipLiner.enabled = shouldBeOn;
        }

        public void UpdateTipSize(float tipWidth, float tipLenMax)
        {
            TipWidth = tipWidth;
            TipLenMax = tipLenMax;
            TipLiner.startWidth = tipWidth;
            SyncLineRenderersWithEndPoints();
        }
    }
}
