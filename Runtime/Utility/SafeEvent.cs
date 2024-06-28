using System;
using System.Collections.Generic;
using Bdeshi.Helpers.Utility.Extensions;
using UnityEngine;

namespace Bdeshi.Helpers.Utility
{
    public class SafeEvent<T1,T2>
    {
        private List<SafeAction<T1,T2>> actions = new List<SafeAction<T1,T2>>();

        public void Invoke(T1 ctx1, T2 ctx2)
        {
            for (int i = actions.Count -1 ; i >=0; i--)
            {
                if (actions[i].Go == null)
                {
                    actions.removeAndSwapWithLast(i);
                }
                else
                {
                    actions[i].Action.Invoke(ctx1, ctx2);
                }
            }
        }

        public void add(GameObject go, Action<T1,T2> a)
        {
            actions.Add(new SafeAction<T1,T2>(go ,a ));
        }

        public void clear()
        {
            actions.Clear();
        }
    }
    public class SafeEvent<TContext>
    {
        private List<SafeAction<TContext>> actions = new List<SafeAction<TContext>>();

        public void Invoke(TContext ctx)
        {
            for (int i = actions.Count -1 ; i >=0; i--)
            {
                if (actions[i].Go == null)
                {
                    actions.removeAndSwapWithLast(i);
                }
                else
                {
                    actions[i].Action.Invoke(ctx);
                }
            }
        }

        public void add(GameObject go, Action<TContext> a)
        {
            actions.Add(new SafeAction<TContext>(go ,a ));
        }

        public void debugLog()
        {
            foreach (var sa in actions)
            {
                Debug.Log(" action with " , sa.Go);
            }
        }

        public void clear()
        {
            actions.Clear();
        }
    }
    
    public class SafeEvent
    {
        private List<SafeAction> actions = new List<SafeAction>();

        public void Invoke()
        {
            for (int i = actions.Count -1 ; i >=0; i--)
            {
                if (actions[i].Go == null)
                {
                    actions.removeAndSwapWithLast(i);
                }
                else
                {
                    actions[i].Action.Invoke();
                }
            }
        }

        public void add(GameObject go, Action a)
        {
            actions.Add(new SafeAction(go ,a ));
        }
        
        
        public void clear()
        {
            actions.Clear();
        }
    }
}