using System;
using UnityEngine;

namespace Bdeshi.Helpers.Utility
{
    /// <summary>
    /// If Go == null, action is not executed and
    /// this is removed from whatever list it belongs to
    /// Safe to use lambdas too.
    /// </summary>
    public class SafeAction
    {
        public GameObject Go;
        public Action Action;

        public SafeAction(GameObject go, Action action)
        {
            this.Go = go;
            this.Action = action;
        }
    }
    /// <summary>
    /// If Go == null, action is not executed and
    /// this is removed from whatever list it belongs to
    /// Safe to use lambdas too.
    /// </summary>
    public class SafeAction<T>
    {
        public GameObject Go;
        public Action<T> Action;

        public SafeAction(GameObject go, Action<T> action)
        {
            this.Go = go;
            this.Action = action;
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
        public GameObject Go;
        public Action<T1,T2> Action;

        public SafeAction(GameObject go, Action<T1,T2> action)
        {
            this.Go = go;
            this.Action = action;
        }
    }
}