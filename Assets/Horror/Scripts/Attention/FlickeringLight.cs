using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Horror.Attention
{
    [RequireComponent(typeof(Light))]
    public class FlickeringLight : MonoBehaviour
    {
        #region Inspector

        public float minIntensity = 0;
        public float maxIntensity = 1;

        public float minIntensityChangeSeconds = 0.03f;
        public float maxIntensityChangeSeconds = 0.2f;

        public float minWaitSeconds = 0.03f;
        public float maxWaitSeconds = 0.2f;

        #endregion

        public void StartFlicker()
        {
            StartCoroutine(FlickerCoroutine());
        }

        public void StopFlicker()
        {
            StopCoroutine(nameof(FlickerCoroutine));
        }

        private void Start()
        {
            StartFlicker();
        }

        private IEnumerator FlickerCoroutine()
        {
            var light = GetComponent<Light>();

            while (true)
            {
                float next = UnityEngine.Random.Range(minIntensity, maxIntensity);
                float over = UnityEngine.Random.Range(minIntensityChangeSeconds, maxIntensityChangeSeconds);

                float diff = next - light.intensity;

                float delta = diff / over;

                while ((delta > 0 && light.intensity < next) || (delta < 0 && light.intensity > next))
                {
                    light.intensity += delta * Time.deltaTime;
                    yield return null;
                }

                light.intensity = next;

                yield return new WaitForSeconds(UnityEngine.Random.Range(minWaitSeconds, maxWaitSeconds));
            }
        }
    }
    
    [Serializable]
    public class UnityEventFlickeringLight : UnityEvent<FlickeringLight> { }
}