using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Horror
{
    [DisallowMultipleComponent]
    public class ImitateYRotation : MonoBehaviour
    {
        #region Inspector

        public Transform target;

        #endregion

        private void Update()
        {
            if (target == null)
                return;

            transform.eulerAngles = new Vector3(0, target.eulerAngles.y, 0);
        }
    }
    
    [Serializable]
    public class UnityEventImitateYRotation : UnityEvent<ImitateYRotation> { }
}