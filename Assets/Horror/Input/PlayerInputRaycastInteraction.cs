using DG.Tweening;
using JK.Interaction;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace Horror.Input
{
    public class PlayerInputRaycastInteraction : MonoBehaviour
    {
        #region Inspector

        public LayerMask layerMask = ~0;

        public float maxDistance = 5;

        #endregion

        [Inject]
        private ActionKeysReader actionKeys = null;

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void Update()
        {
            KeyCode code = actionKeys.ReadKeyDown().FirstOrDefault();

            if (code == default)
                return;

            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, maxDistance * GetDistanceMultiplier(), layerMask))
            {
                if (hit.collider.TryGetComponent(out IInteractable interactable))
                    interactable.Interact(hit);
            }
        }

        private float GetDistanceMultiplier()
        {
            Vector3 straightForward = transform.forward;
            straightForward.y = 0;
            straightForward = straightForward.normalized;

            float dot = Vector3.Dot(straightForward, transform.forward);
            float halfDot = 0.5f - Mathf.Abs(dot - 0.5f);
            float multiplier = 1 + halfDot;

            //Debug.Log($"dot from straight {dot} half {halfDot} mul {multiplier}");

            return multiplier;
        }

#if UNITY_EDITOR

        private int activeRayFrames = 16;
        private int remainingRayFrames = 0;

        private void OnDrawGizmos()
        {
            if (actionKeys == null)
                return;

            KeyCode code = actionKeys.ReadKeyDown().FirstOrDefault();

            if (code != default)
                remainingRayFrames = activeRayFrames;
            else if (remainingRayFrames > 0)
                remainingRayFrames--;

            Gizmos.color = remainingRayFrames > 0 ? Color.green : Color.red;
            Gizmos.DrawLine(transform.position, transform.position + transform.forward * maxDistance * GetDistanceMultiplier());
        }
#endif
    }
    
    [Serializable]
    public class UnityEventPlayerInputRaycastInteraction : UnityEvent<PlayerInputRaycastInteraction> { }
}