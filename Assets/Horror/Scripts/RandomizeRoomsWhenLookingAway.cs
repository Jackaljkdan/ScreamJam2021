using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace Horror
{
    [DisallowMultipleComponent]
    public class RandomizeRoomsWhenLookingAway : MonoBehaviour
    {
        #region Inspector

        public Transform lookTarget;

        public float dotThreshold = 0.8f;

        #endregion

        private bool alreadyRandomized = false;

        [Inject(Id = "player.camera")]
        private Transform playerCamera = null;

        [Inject]
        private RoomsSequence roomsSequence = null;

        [Inject(Id = "corridor")]
        private Room corridor = null;

        private void OnTriggerStay(Collider other)
        {
            if (alreadyRandomized)
                return;

            float dot = Vector3.Dot(playerCamera.forward, lookTarget.position - playerCamera.position);

            if (dot < dotThreshold)
                return;

            Room containingRoom = GetComponentInParent<Room>();

            roomsSequence.RandomizeExceptCurrentAnd(containingRoom);
            corridor.ActivateRandomAlternative();

            alreadyRandomized = true;
        }

        private void OnTriggerExit(Collider other)
        {
            alreadyRandomized = false;
        }
    }
}