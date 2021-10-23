using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Horror.Attention
{
    [DisallowMultipleComponent]
    public class ForceLook : MonoBehaviour
    {
        #region Inspector

        public float lerp = 0.005f;

        public Transform target;

        #endregion

        private void LateUpdate()
        {
            if (target == null)
                return;

            Quaternion targetRotation = Quaternion.LookRotation(target.position - transform.position, Vector3.up);

            // N.B. we can't simply use the following because it rotates also on the z axis
            //transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, lerp * Time.deltaTime);

            Vector3 targetEuler = targetRotation.eulerAngles;
            Vector3 euler = transform.eulerAngles;

            float lerpTime = lerp * Time.deltaTime;
            Vector3 lerpEuler = new Vector3(
                Mathf.LerpAngle(euler.x, targetEuler.x, lerpTime),
                Mathf.LerpAngle(euler.y, targetEuler.y, lerpTime),
                0
            );

            transform.eulerAngles = lerpEuler;
        }
    }
    
    [Serializable]
    public class UnityEventForceLook : UnityEvent<ForceLook> { }
}