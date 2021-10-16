using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Horror.Attention
{
    [DisallowMultipleComponent]
    public class CompassRotation : MonoBehaviour
    {
        #region Inspector

        public Transform target;

        public float lerp = 0.1f;

        public float aimlessSpeed = 800;

        [Header("Runtime")]
        public float forwardDot;

        public float rightDot;
        
        public float forwardIn01;

        public float angle;

        #endregion

        private void Update()
        {
            if (target != null)
            {
                Vector3 toTarget = (target.position - transform.position);
                Vector3 finalUp = Vector3.ProjectOnPlane(toTarget, -transform.forward).normalized;
                Quaternion finalRotation = Quaternion.LookRotation(transform.forward, finalUp);
                transform.rotation = Quaternion.Lerp(transform.rotation, finalRotation, lerp);
            }
            else
            {
                transform.localRotation = Quaternion.AngleAxis(Time.deltaTime * aimlessSpeed, Vector3.forward) * transform.localRotation;
            }
        }
    }
    
    [Serializable]
    public class UnityEventCompassRotation : UnityEvent<CompassRotation> { }
}