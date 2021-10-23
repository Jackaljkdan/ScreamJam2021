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
    [RequireComponent(typeof(CompassRotation))]
    public class InitialCompassTarget : MonoBehaviour
    {
        #region Inspector



        #endregion

        [Inject]
        private RoomsSequence roomsSequence = null;

        private void Start()
        {
            GetComponent<CompassRotation>().target = roomsSequence.Current.transform;

            Destroy(this);
        }
    }
}