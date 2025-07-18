using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using Random = UnityEngine.Random;
#if UNITY_EDITOR
using UnityEditor;
# endif
namespace Bdeshi.Helpers.Utility.Extensions
{
    public static class Unity
    {
        public static void SetColor(this LineRenderer line, Color color)
        {
            line.startColor = line.endColor = color;
        }
        
        public static T GetComponentIfNull<T>(this MonoBehaviour m, T componentVal)
        {
            if (componentVal == null)
            {
                return m.GetComponent<T>();
            }

            return componentVal;
        }
        
        
        public static T GetComponentInParentIfNull<T>(this MonoBehaviour m, T componentVal)
        {
            if (componentVal == null)
            {
                return m.GetComponentInParent<T>();
            }

            return componentVal;
        }
        
        public static void SetAlpha(this LineRenderer line, float alpha)
        {
            line.startColor = line.startColor.overrideAlpha(alpha); 
            line.endColor = line.endColor.overrideAlpha(alpha);
        }
        
        public static void SetAlpha(this SpriteRenderer spriter, float alpha)
        {
            spriter.color = spriter.color.overrideAlpha(alpha); 
        }
        
        public static void SetAlpha(this Image image, float alpha)
        {
            image.color = image.color.overrideAlpha(alpha); 
        }
        
        public static void allignToDir2D(this Transform transform, Vector2 dir)
        {
            float angle = get2dAngle(dir);
            transform.set2dRotation(angle);
        }

        public static void allignToDir2D(this Transform transform, Vector2 dir, float angleOffsetInDegrees)
        {
            float angle = get2dAngle(dir) + angleOffsetInDegrees;
            transform.set2dRotation(angle);
        }


        public static void allignToDir(this Rigidbody2D rb2D, Vector2 dir)
        {
            rb2D.rotation = get2dAngle(dir);
        }

        public static Vector2Int CeilToInt(this Vector2 v)
        {
            return new Vector2Int(Mathf.CeilToInt(v.x), Mathf.CeilToInt(v.y));
        }
        
        public static Vector2Int RoundToInt(this Vector2 v)
        {
            return new Vector2Int(Mathf.RoundToInt(v.x), Mathf.RoundToInt(v.y));
        }
        
        public static bool exceedsDist(this Vector3 vec, float dist)
        {
            return vec.sqrMagnitude > (dist * dist);
        }

        public static bool IsBoundedBy(this Vector2 vec, Vector2 min, Vector2 max)
        {
            return vec.x >= min.x && vec.y >= min.y &&
                   vec.x <= max.x && vec.y <= max.y;
        }
#if UNITY_EDITOR
        public static T CreateChildScriptable<T>(this ScriptableObject parentSO, Action<T> initializeFunc)
            where T : ScriptableObject
        {
            var childSO = ScriptableObject.CreateInstance<T>();
            initializeFunc(childSO);

            AssetDatabase.AddObjectToAsset(childSO, parentSO);
            AssetDatabase.SaveAssets();

            EditorUtility.SetDirty(parentSO);
            return childSO; 
        }
# endif
        
        
        /// <summary>
        /// Imagine a cone with the "with" vec as forward, is the "dir" within a given angle with "with" 
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="with"></param>
        /// <param name="halfAngle"></param>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static  bool isWithinAngleWith(this Vector3 dir, Vector3 with, float halfAngle, out float angle)
        {
            angle = Vector2.Angle(with, dir);

            return angle <= halfAngle;
        }
        public static bool withinRange(this Vector3 vec, float minDist, float maxDist)
        {
            var d = vec.sqrMagnitude;
            return d >= (minDist * minDist) && d <= (maxDist * maxDist);
        }

        public static void set2dRotation(this Transform transform, float angle)
        {
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        public static void addAngleOffset(this Transform transform, float angleOffset)
        {
            transform.rotation = Quaternion.AngleAxis(transform.rotation.eulerAngles.z + angleOffset, Vector3.forward);
        }
        
        /// <summary>
        /// Get 2d angle of this vector
        /// </summary>
        /// <param name="normalizedDir">FOR THE LOVE OF GOD NORMALIZE THIS</param>
        /// <returns></returns>
        public static float get2dAngle(this Vector3 normalizedDir)
        {
            return Mathf.Atan2(normalizedDir.y, normalizedDir.x) * Mathf.Rad2Deg;
        }

        public static void SetDirty(this UnityEngine.Object obj)
        {
# if UNITY_EDITOR
            EditorUtility.SetDirty(obj);
# endif
        }

        public static float get2dAngle(this Vector2 dir)
        {
            return Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        }
        public static float get2dAngle(this Transform t)
        {
            return get2dAngle(t.up);
        }
        public static void lookAlongTopDown(this Transform transform, Vector3 dir)
        {
            transform.rotation = Quaternion.LookRotation(dir, Vector3.up);
        }

        public static bool withinRange(this Vector2 range, float value)
        {
            return value >= range.x && value <= range.y;
        }

        public static Vector3 toTopDown(this Vector2 dir)
        {
            return new Vector3(dir.x, 0, dir.y);
        }

        public static void Rotate(this Vector2 v, float degrees)
        {
            float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
            float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

            float tx = v.x;
            float ty = v.y;
            v.x = (cos * tx) - (sin * ty);
            v.y = (sin * tx) + (cos * ty);
        }
        
        public static Vector2 Rotated(this Vector2 v, float degrees)
        {
            float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
            float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

            float tx = v.x;
            float ty = v.y;
            return new Vector2((cos * tx) - (sin * ty),(sin * tx) + (cos * ty));
        }

        public static bool Contains(this LayerMask mask, int layer)
        {
            return mask == (mask | (1 << layer));
        }

        public static bool Contains(this LayerMask mask, GameObject obj)
        {
            return mask == (mask | (1 << obj.layer));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mask"></param>
        /// <param name="hit">ASSUME THAT the raycast has HIT SOMETHING</param>
        /// <returns></returns>
        public static bool Contains(this LayerMask mask, RaycastHit2D hit)
        {
            return mask == (mask | (1 << hit.collider.gameObject.layer));
        }
        public static bool Contains(this LayerMask mask, RaycastHit hit)
        {
            return mask == (mask | (1 << hit.collider.gameObject.layer));
        }

        public static void reparentAndReset(this Transform transform, Transform parent)
        {
            transform.parent = parent;
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
        }
        
        public static float distanceBetween(this Transform transform, Transform t)
        {
            return (transform.position - t.position).magnitude;
        }

        public static RaycastHit2D raycastFromInsideCollider2D(Vector2 origin, Vector2 direction, float length, LayerMask layer)
        {
            bool usedToHit = Physics2D.queriesStartInColliders;
            Physics2D.queriesStartInColliders = true;
            var result = Physics2D.Raycast(origin, direction, length, layer);
            Physics2D.queriesStartInColliders = usedToHit;

            return result;
        }

        public static Vector2 getRaycastEndpoint2D(Vector2 origin, Vector2 dir, float length, LayerMask layer, out RaycastHit2D hit)
        {
            dir.Normalize();
            hit = raycastFromInsideCollider2D(origin, dir, length, layer);
            return hit ? (hit.point) : (origin + dir * length);
        }


        public static Vector2 multiplyDimensions(this Vector2 v, Vector2 other)
        {
            return new Vector2(v.x * other.x, v.y * other.y);
        }

        public static Vector3 multiplyDimensions(this Vector3 v, Vector3 other)
        {
            return new Vector3(v.x * other.x, v.y * other.y, v.z * other.z);
        }
        // public static void Shuffle<T>(this IList<T> list)  
        // {  
        //     for (int i = list.Count -1 ; i < list.Count; i--)
        //     {
        //         int k = Random.Range(0, i+1); 
        //         
        //         (list[k], list[i]) = (list[i], list[k]);
        //     }
        //
        // }
        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = Random.Range(0, n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static float evaluateCurve(this  FiniteTimer timer, AnimationCurve curve)
        {
            return curve.Evaluate(timer.Ratio);
        }

        public static Color overrideAlpha(this Color color, float a)
        {
            return new Color(color.r, color.g, color.b, a);
        }

        public static bool IsBetween(this Vector3Int vec, Vector3Int min, Vector3Int max)
        {
            return vec.x >= min.x && vec.y >= min.y && vec.z >= min.z &&
                   vec.x <= max.x && vec.y <= max.y && vec.z <= max.z;
        }
        public static Vector3Int OverrideX(this Vector3Int vec, int x) => new Vector3Int(x, vec.y, vec.z);
        public static Vector3 OverrideX(this Vector3 vec, float x) => new Vector3(x, vec.y, vec.z);
        public static Vector3Int OverrideY(this Vector3Int vec, int y) => new Vector3Int(vec.x, y, vec.z);
        public static Vector3 OverrideY(this Vector3 vec, float y) => new Vector3(vec.x, y, vec.z);
        public static Vector3Int OverrideZ(this Vector3Int vec, int z) => new Vector3Int(vec.x, vec.y, z);
        public static Vector3 OverrideZ(this Vector3 vec, float z) => new Vector3(vec.x, vec.y, z);
        public static Vector3 OverrideX(this Vector3 vec, Vector3 target) => vec.OverrideX(target.x);
        public static Vector3 OverrideY(this Vector3 vec, Vector3 target) => vec.OverrideY(target.y);
        public static Vector3 OverrideZ(this Vector3 vec, Vector3 target) => vec.OverrideZ(target.z);

        public static Vector2Int OverrideX(this Vector2Int vec, int x) => new Vector2Int(x, vec.y);
        public static Vector2 OverrideX(this Vector2 vec, float x) => new Vector2(x, vec.y);
        public static Vector2Int OverrideY(this Vector2Int vec, int y) => new Vector2Int(vec.x, y);
        public static Vector2 OverrideY(this Vector2 vec, float y) => new Vector2(vec.x, y);
        public static Vector3 WithZ(this Vector2 vec, float z) => new Vector3(vec.x, vec.y, z);
        public static Vector3 MaintainX(this Vector3 vec1, Vector3 vec2) => new Vector3(vec1.x, vec2.y, vec2.z);
        public static Vector3 MaintainY(this Vector3 vec1, Vector3 vec2) => new Vector3(vec2.x, vec1.y, vec2.z);
        public static Vector3 MaintainZ(this Vector3 vec1, Vector3 vec2) => new Vector3(vec2.x, vec2.y, vec1.z);
        public static Vector3Int MaintainZ(this Vector3Int vec1, Vector3Int vec2) => new Vector3Int(vec2.x, vec2.y, vec1.z);
        public static Vector3Int MaintainX(this Vector3Int vec1, Vector3Int vec2) => new Vector3Int(vec1.x, vec2.y, vec2.z);
        public static Vector3Int MaintainY(this Vector3Int vec1, Vector3Int vec2) => new Vector3Int(vec2.x, vec1.y, vec2.z);
        public static float MaxAxis(this Vector3 vec) => Mathf.Max(vec.x, vec.y, vec.z);
        public static List<Color> GenerateDifferentColors(int n, float alpha =1)
        {
            List<Color> colors = new List<Color>();

            // Starting hue value
            float startHue = 0;

            for (int i = 0; i < n; i++)
            {
                float hue = (startHue + (float)i / (float)n) % 1f; // Calculate hue

                // Convert HSL to RGB
                Color color = HSLToRGB(hue, 0.5f, 0.5f);
                color.a = alpha;
                colors.Add(color); // Add the color to the list
            }

            return colors;
        }

        // Helper function to convert HSL to RGB
        private static Color HSLToRGB(float hue, float saturation, float lightness)
        {
            float c = (1 - Mathf.Abs(2 * lightness - 1)) * saturation;
            float x = c * (1 - Mathf.Abs((hue * 6) % 2 - 1));
            float m = lightness - c / 2;

            float r, g, b;

            if (0 <= hue && hue < 1 / 6f)
            {
                r = c; g = x; b = 0;
            }
            else if (1 / 6f <= hue && hue < 2 / 6f)
            {
                r = x; g = c; b = 0;
            }
            else if (2 / 6f <= hue && hue < 3 / 6f)
            {
                r = 0; g = c; b = x;
            }
            else if (3 / 6f <= hue && hue < 4 / 6f)
            {
                r = 0; g = x; b = c;
            }
            else if (4 / 6f <= hue && hue < 5 / 6f)
            {
                r = x; g = 0; b = c;
            }
            else
            {
                r = c; g = 0; b = x;
            }

            return new Color(r + m, g + m, b + m);
        }


#if UNITY_EDITOR
        public static void DrawWireCapsule(Vector3 _pos, Quaternion _rot, float _radius, float _height, Color _color = default(Color))
        {
            if (_color != default(Color))
                Handles.color = _color;
            Matrix4x4 angleMatrix = Matrix4x4.TRS(_pos, _rot, Handles.matrix.lossyScale);
            using (new Handles.DrawingScope(angleMatrix))
            {
                var pointOffset = (_height - (_radius * 2)) / 2;

                //draw sideways
                Handles.DrawWireArc(Vector3.up * pointOffset, Vector3.left, Vector3.back, -180, _radius);
                Handles.DrawLine(new Vector3(0, pointOffset, -_radius), new Vector3(0, -pointOffset, -_radius));
                Handles.DrawLine(new Vector3(0, pointOffset, _radius), new Vector3(0, -pointOffset, _radius));
                Handles.DrawWireArc(Vector3.down * pointOffset, Vector3.left, Vector3.back, 180, _radius);
                //draw frontways
                Handles.DrawWireArc(Vector3.up * pointOffset, Vector3.back, Vector3.left, 180, _radius);
                Handles.DrawLine(new Vector3(-_radius, pointOffset, 0), new Vector3(-_radius, -pointOffset, 0));
                Handles.DrawLine(new Vector3(_radius, pointOffset, 0), new Vector3(_radius, -pointOffset, 0));
                Handles.DrawWireArc(Vector3.down * pointOffset, Vector3.back, Vector3.left, -180, _radius);
                //draw center
                Handles.DrawWireDisc(Vector3.up * pointOffset, Vector3.up, _radius);
                Handles.DrawWireDisc(Vector3.down * pointOffset, Vector3.up, _radius);

            }
        }

        public static void DrawPathGizmos(List<Vector3> path)
        {
            for (int i = 1; i < path.Count; i++)
            {
                Gizmos.DrawLine(path[i-1], path[i ]);
            }
        }

        public static void setGizmoLocalMatrix(this Transform transform)
        {
            Gizmos.matrix = transform.localToWorldMatrix;;
        }
#endif

    }
}