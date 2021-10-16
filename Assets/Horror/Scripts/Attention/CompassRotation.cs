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

        public Transform forwardReference;

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
                Transform reference;

                if (forwardReference != null)
                    reference = forwardReference;
                else
                    reference = transform;

                Vector3 toTarget = (target.position - transform.position);
                Vector3 projectedDirectionToTarget = Vector3.ProjectOnPlane(toTarget, Vector3.up).normalized;

                Vector3 projectedReferenceForward = Vector3.ProjectOnPlane(reference.forward, Vector3.up).normalized;
                Vector3 projectedReferenceRight = Vector3.ProjectOnPlane(reference.right, Vector3.up).normalized;

                forwardDot = Vector3.Dot(projectedDirectionToTarget, projectedReferenceForward);
                rightDot = Vector3.Dot(projectedDirectionToTarget, projectedReferenceRight);

                forwardIn01 = (forwardDot + 1) / 2;
                angle = (1 - forwardIn01) * 180;

                Quaternion targetRotation = Quaternion.AngleAxis(angle * Mathf.Sign(-rightDot), Vector3.forward);
                transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, lerp);
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