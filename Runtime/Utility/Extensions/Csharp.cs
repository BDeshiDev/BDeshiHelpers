using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Object = System.Object;
using Random = UnityEngine.Random;

namespace Bdeshi.Helpers.Utility.Extensions
{
    public static class Csharp
    {
        public static T getRandomItem<T>(this List<T> list) => list[Random.Range(0, list.Count)];
        [CanBeNull]
        public static T getRandomItemExcluding<T>(this List<T> list, T item)
        {
            if (list.Count <= 0)
                return default(T);
            int randomIndex = list.getRandomIndex(list.Count);
            if (list[randomIndex].Equals(item))
            {
                if (list.Count == 1)
                    return default(T);
                else
                {
                    return list[(randomIndex + 1) % list.Count];
                }
            }
            else
            {
                return list[randomIndex];
            }
        }
        public static string ObjectNullOrToString(Object o)
        {
            if (o == null)
                return "null";
            return o.ToString();
        }

        public static T binarySearchFirstGreater<T>(this List<T> list, T val)
        {
            int i = list.BinarySearch(val);
            // Debug.Log(i);
            if (i >= 0)
            {
                if (i < list.Count)
                {
                    return list[i];
                }
            }else
            {
                i = -i -1 ;
                if (i < list.Count)
                {
                    return list[i];
                }
                else
                {
                    return list[list.Count - 1];
                }
            }
            return default(T); 
        }
        
        public static int getRandomIndex<T>(this List<T> list, int maxIndexPlusOne) => Random.Range(0, maxIndexPlusOne);
        public static T getRandomItem<T>(this List<T> list, int maxIndexPlusOne) => list[Random.Range(0, maxIndexPlusOne)];

        public static void removeItemAndSwapWithLast<T>(this List<T> list, T item) where T : class
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] == item)
                {
                    if (i != (list.Count - 1))
                    {
                        var t = list[i];
                        list[i] = list[list.Count - 1];
                        list[list.Count - 1] = t;
                        list.RemoveAt(list.Count - 1);
                        break;
                    }
                }
            }
        }

        public static string ToStringOrNull(object o) => o == null ? "null" : o.ToString(); 
        

        public static void removeAndSwapWithLast<T>(this List<T> list, int index)
        {
            if(index < 0 || index > list.Count)
                return;

            if (index != (list.Count - 1))
            {
                (list[index], list[list.Count - 1]) = (list[list.Count - 1], list[index]);
                list.RemoveAt(list.Count - 1);
            }
        }
        
        public static void swapToLast<T>(this List<T> list, int index)
        {
            if(index < 0 || index >= (list.Count - 1))
                return;
            
            if (index != (list.Count - 1))
            {
                var t = list[index];
                list[index] = list[list.Count - 1];
                list[list.Count - 1] = t;
            }
        }
    }
}