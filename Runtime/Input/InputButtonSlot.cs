using System;
using System.Collections.Generic;
using Bdeshi.Helpers.Utility;
using Bdeshi.Helpers.Utility.Extensions;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Bdeshi.Helpers.Input
{
    /// <summary>
    /// You may also assume that this can be safely stored in fields
    /// as this is not serialized
    /// </summary>
    public class InputButtonSlot
    {
        private List<SafeAction> _onPerformedCallbacks = new List<SafeAction>();
        private List<SafeAction> _onCancelledCallbacks = new List<SafeAction>();
            
        public bool IsHeld { get; private set; }
        public float LastHeld;
        private List<InputActionReference> _boundActions = new();
        public string ButtonName;

        public InputButtonSlot(string buttonName)
        {
            this.ButtonName = buttonName;
        }

        public bool WasHeld(float heldWithinThreshold)
        {
            return IsHeld || (Time.time - LastHeld) < heldWithinThreshold;
        }
        
        public bool WasReleased(float heldWithinThreshold)
        {
            return !IsHeld && (Time.time - LastHeld) < heldWithinThreshold;
        }

        public void ClearPressedStatus()
        {
            IsHeld = false;
        }

        public void AddPerformedCallback(GameObject go, Action a)
        {
            _onPerformedCallbacks.Add(new SafeAction(go ,a ));
        }
            
        public void AddCancelledCallback(GameObject go, Action a)
        {
            _onCancelledCallbacks.Add(new SafeAction(go ,a ));
        }

        public void Bind(InputActionReference iar)
        {
            if(iar == null)
                return;

            Debug.Log($"BIND {ButtonName} to {iar}", iar);
            _boundActions.Add(iar);
            iar.action.performed += ActionOnPerformed;
            iar.action.canceled += ActionOncanceled;
        }

        private void UnBind(InputActionReference iar)
        {               
            if(iar == null)
                return;

            _boundActions.Remove(iar);
            iar.action.performed -= ActionOnPerformed;
            iar.action.canceled -= ActionOncanceled;
        }
        
        public void UnBind()
        {
            for (var index = _boundActions.Count -1; index >=0 ; index--)
            {
                var boundAction = _boundActions[index];
                UnBind(boundAction);
            }
        }

        private void ActionOncanceled(InputAction.CallbackContext obj)
        {
            IsHeld = false;
            for (int i = _onCancelledCallbacks.Count -1 ; i >=0; i--)
            {
                if (_onCancelledCallbacks[i].Go == null)
                {
                    _onCancelledCallbacks.removeAndSwapWithLast(i);
                }
                else
                {
                    _onCancelledCallbacks[i].Action.Invoke();
                }
            }
        }

        public void ManualPerform() => ActionOnPerformed(default);
        public void ManualCancel() => ActionOncanceled(default);
        private void ActionOnPerformed(InputAction.CallbackContext obj)
        {
            IsHeld = true;
            LastHeld = Time.time;

            for (int i = _onPerformedCallbacks.Count -1 ; i >=0; i--)
            {
                if (_onPerformedCallbacks[i].Go == null)
                {
                    _onPerformedCallbacks.removeAndSwapWithLast(i);
                }
                else
                {
                    _onPerformedCallbacks[i].Action.Invoke();
                }
            }
        }

        public void DebugLog()
        {
            Debug.Log(ButtonName + "  performed x " + _onPerformedCallbacks.Count);
            for (int i = _onPerformedCallbacks.Count -1 ; i >=0; i--)
            {
                var go = _onPerformedCallbacks[i].Go;
                Debug.Log((go != null ? go : "null") + " subbed ", go);
            }
            
            Debug.Log( ButtonName + " cancelled x " + _onCancelledCallbacks.Count);
            for (int i = _onCancelledCallbacks.Count -1 ; i >=0; i--)
            {
                var go = _onCancelledCallbacks[i].Go;
                Debug.Log((go != null ? go : "null") + " subbed ", go);
            }
        }
        
        public void Cleanup()
        {
            IsHeld = false;
            _onPerformedCallbacks.Clear();
            _onCancelledCallbacks.Clear();
            LastHeld = -9999;
        }


    }
}