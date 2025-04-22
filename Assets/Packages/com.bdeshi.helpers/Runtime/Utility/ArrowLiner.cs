using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace Bdeshi.Helpers.Utility
{
    public class ArrowLiner : MonoBehaviour
    {
        public LineRenderer MainLiner;
        public LineRenderer TipLiner;
        public LineRenderer compressionArrowExtraTip;

        public float MainLineWidth = .25f;
        public float TipWidth = .25f;
        public float TipLenMax = .25f;

        [SerializeField] Vector3 _startPoint;
        [SerializeField] Vector3 _endPoint;
        private Tween _tween;
        public float _extraTipBorderSize = .1f;
        private Vector3? _prevLineEndPoint;

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
        
        public Tween TweenArrow(Vector3 start,Vector3 end, float tweenDuration)
        {
            _startPoint = start;
            _endPoint = end;
            
            Vector3 dir = _endPoint - _startPoint;
            float len = dir.magnitude;

            var dirNormalized = dir.normalized;
            Vector3 initialEndPoint = Vector3.zero;
            if (_prevLineEndPoint.HasValue)
            {
                _prevLineEndPoint = initialEndPoint  = MainLineEndPoint;
            }
            else
            {
                _prevLineEndPoint = _startPoint;
            }

            MainLineEndPoint = _startPoint + dirNormalized * Mathf.Max(len - TipLenMax, 0);
            MainLiner.SetPosition(0, _startPoint);

            float t = 0;
            if (_tween != null && _tween.IsActive())
            {
                _tween.Kill();
            }

            _tween = DOTween.To(
                    () => t, 
                    x =>
                    {
                        t = x;
                        var lineEndPoint = Vector3.Lerp(initialEndPoint, MainLineEndPoint, t);
                        var tipEndPoint = Vector3.Lerp(initialEndPoint + TipLenMax * dirNormalized, _endPoint, t);
                        SetEndPointAndTip(lineEndPoint, tipEndPoint);
                        SyncExtraTip(dirNormalized);
                    }, 
                    1f, tweenDuration)
                .SetEase(Ease.Linear)
                .SetUpdate(true);
            SyncLineRenderersWithEndPoints();
            return _tween;
        }
        
        private void SyncExtraTip(Vector3 dir)
        {
            compressionArrowExtraTip.transform.position = TipLiner.transform.position;
            compressionArrowExtraTip.SetPosition(0,TipLiner.GetPosition(0) - dir * _extraTipBorderSize);
            compressionArrowExtraTip.SetPosition(1,TipLiner.GetPosition(1) + dir * _extraTipBorderSize);
        }


        public void SyncLineRenderersWithEndPoints(bool tipAtEndpoints = false)
        {
            Vector3 dir = _endPoint - _startPoint;
            float len = dir.magnitude;

            var dirNormalized = dir.normalized;
            MainLineEndPoint = _startPoint + dirNormalized * Mathf.Max(len - TipLenMax, 0);
            MainLiner.SetPosition(0, _startPoint);
            
            SetEndPointAndTip(MainLineEndPoint, _endPoint);
            SyncExtraTip(dirNormalized);
        }

        private void SetEndPointAndTip(Vector3 mainLineEndPoint, Vector3 tipEndPoint)
        {
            MainLiner.SetPosition(1, mainLineEndPoint);
            TipLiner.SetPosition(0, mainLineEndPoint);
            TipLiner.SetPosition(1, tipEndPoint);
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
