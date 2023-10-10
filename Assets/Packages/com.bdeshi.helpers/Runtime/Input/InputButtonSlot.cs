using System;
using System.Collections.Generic;
using com.bdeshi.helpers.Utility;
using com.bdeshi.helpers.Utility.Extensions;
using UnityEngine;
using UnityEngine.InputSystem;

namespace com.bdeshi.helpers.Input
{
    /// <summary>
    /// You may also assume that this can be safely stored in fields
    /// as this is not serialized
    /// </summary>
    public class InputButtonSlot
    {
        private List<SafeAction> onPerformedCallbacks = new List<SafeAction>();
        private List<SafeAction> OnCancelledCallbacks = new List<SafeAction>();
            
        public bool isHeld { get; private set; }
        public float lastHeld;
        private InputActionReference action;
        public string buttonName;

        public InputButtonSlot(string buttonName)
        {
            this.buttonName = buttonName;
        }

        public bool wasHeld(float heldWithinThreshold)
        {
            return isHeld || (Time.time - lastHeld) < heldWithinThreshold;
        }
        
        public bool wasReleased(float heldWithinThreshold)
        {
            return !isHeld && (Time.time - lastHeld) < heldWithinThreshold;
        }

        public void clearPressedStatus()
        {
            isHeld = false;
        }

        public void addPerformedCallback(GameObject go, Action a)
        {
            onPerformedCallbacks.Add(new SafeAction(go ,a ));
        }
            
        public void addCancelledCallback(GameObject go, Action a)
        {
            OnCancelledCallbacks.Add(new SafeAction(go ,a ));
        }

        public void bind(InputActionReference iar)
        {
            if(iar == null)
                return;

            action = iar;
            iar.action.performed += ActionOnperformed;
            iar.action.canceled += ActionOncanceled;
        }

        private void unBind(InputActionReference iar)
        {               
            if(iar == null)
                return;
            iar.action.performed -= ActionOnperformed;
            iar.action.canceled -= ActionOncanceled;
        }
        
        public void unBind()
        {
            unBind(action);
            action = null;
        }

        private void ActionOncanceled(InputAction.CallbackContext obj)
        {
            isHeld = false;
            for (int i = OnCancelledCallbacks.Count -1 ; i >=0; i--)
            {
                if (OnCancelledCallbacks[i].go == null)
                {
                    OnCancelledCallbacks.removeAndSwapToLast(i);
                }
                else
                {
                    OnCancelledCallbacks[i].action.Invoke();
                }
            }
        }

        private void ActionOnperformed(InputAction.CallbackContext obj)
        {
            isHeld = true;
            lastHeld = Time.time;

            for (int i = onPerformedCallbacks.Count -1 ; i >=0; i--)
            {
                if (onPerformedCallbacks[i].go == null)
                {
                    onPerformedCallbacks.removeAndSwapToLast(i);
                }
                else
                {
                    onPerformedCallbacks[i].action.Invoke();
                }
            }
        }

        public void debugLog()
        {
            Debug.Log(buttonName + "  performed x " + onPerformedCallbacks.Count);
            for (int i = onPerformedCallbacks.Count -1 ; i >=0; i--)
            {
                var go = onPerformedCallbacks[i].go;
                Debug.Log((go != null ? go : "null") + " subbed ", go);
            }
            
            Debug.Log( buttonName + " cancelled x " + OnCancelledCallbacks.Count);
            for (int i = OnCancelledCallbacks.Count -1 ; i >=0; i--)
            {
                var go = OnCancelledCallbacks[i].go;
                Debug.Log((go != null ? go : "null") + " subbed ", go);
            }
        }
        
        public void cleanup()
        {
            isHeld = false;
            onPerformedCallbacks.Clear();
            OnCancelledCallbacks.Clear();
            lastHeld = -9999;
        }


    }
}