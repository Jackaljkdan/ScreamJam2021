using DG.Tweening;
using JK.Interaction;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace Horror.Attention
{
    public class AffordanceRaycast : MonoBehaviour
    {
        #region Inspector

        public LayerMask layerMask = ~0;

        public float distance = 5;

        #endregion

        private void Update()
        {
            Raycast();
        }

        private void Raycast()
        {
            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, distance, layerMask))
            {
                if (hit.collider.TryGetComponent(out IAffordance affordance))
                    affordance.Highlight(hit);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + transform.forward * distance);
        }
    }
}