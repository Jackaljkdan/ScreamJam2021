using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.UserInput
{
    [DisallowMultipleComponent]
    public class SetCursorModeOnInput : MonoBehaviour
    {
        #region Inspector

        public CursorLockMode mode = CursorLockMode.Locked;

        public DestroyMode destroyMode = DestroyMode.DestroyGameObject;

        #endregion

        [Serializable]
        public enum DestroyMode
        {
            None,
            DestroySelf,
            DestroyGameObject,
        }

        private void Update()
        {
            if (UnityEngine.Input.anyKeyDown)
            {
                Cursor.lockState = mode;

                if (destroyMode != DestroyMode.None)
                {
                    if (destroyMode == DestroyMode.DestroyGameObject)
                        Destroy(gameObject);
                    else
                        Destroy(this);
                }
            }
        }
    }
}