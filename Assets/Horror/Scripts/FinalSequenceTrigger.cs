using DG.Tweening;
using Horror.Interaction;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Horror
{
    [DisallowMultipleComponent]
    public class FinalSequenceTrigger : MonoBehaviour
    {
        #region Inspector

        [SerializeField]
        private SlidingDoorInteractable door = null;

        [SerializeField]
        private GameObject starsBeginning = null;

        [SerializeField]
        private Transform rotateMoon = null;

        [SerializeField]
        private float rotateSeconds = 2;

        [SerializeField]
        private Moon moon = null;

        [SerializeField]
        private Transform moonTarget = null;

        #endregion

        private void OnTriggerEnter(Collider other)
        {
            door.ForceClose();
            door.IsLocked = true;

            starsBeginning.SetActive(false);

            moon.targetAnchor = moonTarget;

            StartCoroutine(RotateMoonCoroutine());
        }

        private IEnumerator RotateMoonCoroutine()
        {
            moon.enabled = false;
            moon.ForceStopLooking();

            rotateMoon.DORotate(new Vector3(0, -180, 0), rotateSeconds);

            yield return new WaitForSeconds(rotateSeconds / 2);

            moon.enabled = true;
            moon.minDot = -1;
            moon.ignoreRaycast = true;

            Destroy(gameObject);
        }
    }
    
    [Serializable]
    public class UnityEventFinalSequenceTrigger : UnityEvent<FinalSequenceTrigger> { }
}