using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace Horror.Attention
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(ForceLook))]
    public class ForceLookAtMoonIntensity : MonoBehaviour
    {
        #region Inspector

        public float minLookLerp = 0;
        public float maxLookLerp = 0.05f;

        #endregion

        [Inject]
        private Moon moon = null;

        private void Start()
        {
            GetComponent<ForceLook>().target = moon.transform;
        }

        private void LateUpdate()
        {
            var forceLook = GetComponent<ForceLook>();

            if (moon.IsLooking)
                forceLook.lerp = Mathf.Lerp(minLookLerp, maxLookLerp, moon.Intensity);
            else
                forceLook.lerp = Mathf.Lerp(forceLook.lerp, 0, maxLookLerp);
        }
    }
}