using System;
using UnityEngine;

namespace com.bdeshi.helpers.Utility
{
    /// <summary>
    /// If Go == null, action is not executed and
    /// this is removed from whatever list it belongs to
    /// Safe to use lambdas too.
    /// </summary>
    public class SafeAction
    {
        public GameObject go;
        public Action action;

        public SafeAction(GameObject go, Action action)
        {
            this.go = go;
            this.action = action;
        }
    }
    /// <summary>
    /// If Go == null, action is not executed and
    /// this is removed from whatever list it belongs to
    /// Safe to use lambdas too.
    /// </summary>
    public class SafeAction<T>
    {
        public GameObject go;
        public Action<T> action;

        public SafeAction(GameObject go, Action<T> action)
        {
            this.go = go;
            this.action = action;
        }
        
    }
    //This is clearly unscalable
    /// <summary>
    /// If Go == null, action is not executed and
    /// this is removed from whatever list it belongs to
    /// Safe to use lambdas too.
    /// </summary>
    public class SafeAction<T1,T2>
    {
        public GameObject go;
        public Action<T1,T2> action;

        public SafeAction(GameObject go, Action<T1,T2> action)
        {
            this.go = go;
            this.action = action;
        }
    }
}