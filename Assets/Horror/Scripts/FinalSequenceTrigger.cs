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

        #endregion

        private void OnTriggerEnter(Collider other)
        {
            door.ForceClose();
            door.IsLocked = true;

            Destroy(gameObject);
        }

    }
    
    [Serializable]
    public class UnityEventFinalSequenceTrigger : UnityEvent<FinalSequenceTrigger> { }
}