using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.PostProcessing;
using Zenject;

namespace Horror.Attention
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(PostProcessVolume))]
    public class MoonPostprocessing : MonoBehaviour
    {
        #region Inspector

        public float chromaticAtMax = 0.8f;

        public float colorAtMin = 0.5f;

        public Color minColor = Color.white;
        public Color maxColor = Color.black;

        public float sinSpeed = 1;

        public float sinIn01;

        public float chromaticNominalIntensity;
        public float chromaticIntensity;
        
        public float colorNominalIntensity;
        public float colorIntensity;

        #endregion

        [Inject]
        private Moon moon = null;

        private void LateUpdate()
        {
            float intensity = moon.Intensity;
            float sinOfTime = Mathf.Sin(Time.time * sinSpeed);
            sinIn01 = (sinOfTime + 1) / 2;

            var volume = GetComponent<PostProcessVolume>();

            chromaticNominalIntensity = Mathf.Min(intensity / chromaticAtMax, 1);
            chromaticIntensity = sinIn01 * chromaticNominalIntensity;

            volume.profile.GetSetting<ChromaticAberration>().intensity.value = chromaticIntensity;

            //if (intensity < colorAtMin)
            //{
            //    colorNominalIntensity = 0;
            //}
            //else
            //{
            //    // range [0, 1 - colorAtMin] -> [0,1]
            //    float oldValue = intensity - colorAtMin;
            //    colorNominalIntensity = oldValue / (1 - colorAtMin);
            //}

            //colorIntensity = sinIn01 * colorNominalIntensity;

            //volume.profile.GetSetting<ColorGrading>().colorFilter.value = Color.Lerp(minColor, maxColor, colorIntensity);
        }
    }
    
    [Serializable]
    public class UnityEventMoonPostprocessing : UnityEvent<MoonPostprocessing> { }
}