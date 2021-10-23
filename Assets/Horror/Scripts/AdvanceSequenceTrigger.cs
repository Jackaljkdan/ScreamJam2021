using Horror.Attention;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace Horror
{
    [DisallowMultipleComponent]
    public class AdvanceSequenceTrigger : MonoBehaviour
    {
        #region Inspector

        public float compassDelay = 5f;

        #endregion

        [Inject]
        private CompassRotation compass = null;

        [Inject]
        private RoomsSequence roomsSequence = null;

        [Inject(Id = "room.final")]
        private Transform finalRoom = null;

        private void OnTriggerEnter(Collider other)
        {
            Destroy(GetComponent<Collider>());

            StartCoroutine(AdvanceCoroutine());
        }

        private IEnumerator AdvanceCoroutine()
        {
            compass.target = null;
            roomsSequence.Advance();

            yield return new WaitForSeconds(compassDelay);

            Room current = roomsSequence.Current;

            compass.target = current != null ? current.transform : finalRoom;

            Destroy(gameObject);
        }
    }
}