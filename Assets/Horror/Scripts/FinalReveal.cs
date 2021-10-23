using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.PostProcessing;
using Zenject;

namespace Horror
{
    [DisallowMultipleComponent]
    public class FinalReveal : MonoBehaviour
    {
        #region Inspector

        public Transform earth;

        public float fadeInSeconds = 1;

        public float waitSeconds = 5;

        #endregion

        [Inject]
        private Moon moon = null;

        [Inject]
        private PostProcessVolume postVolume = null;

        private IEnumerator Start()
        {
            moon.enabled = false;

            var colorGrading = postVolume.profile.GetSetting<ColorGrading>();
            colorGrading.colorFilter.value = Color.black;

            yield return DOTween.To(
                () => colorGrading.colorFilter.value,
                value => colorGrading.colorFilter.value = value,
                Color.white,
                fadeInSeconds
            ).WaitForCompletion();

            Tween earthTween = earth.DOMoveY(300, duration: 15).SetRelative(true);

            yield return new WaitForSeconds(waitSeconds);

            moon.enabled = true;

            Destroy(gameObject);
        }
    }
    
    [Serializable]
    public class UnityEventFinalReveal : UnityEvent<FinalReveal> { }
}