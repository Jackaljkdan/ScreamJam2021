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
    public class MoonBlackness : MonoBehaviour
    {
        #region Inspector

        public float fadeSeconds = 1.5f;

        public float threshold = 0.97f;

        public UnityEvent onFadeout = new UnityEvent();

        #endregion

        [Inject]
        private Moon moon = null;

        [Inject]
        private PostProcessVolume postVolume = null;

        private void LateUpdate()
        {
            if (moon.Intensity >= threshold)
            {
                StartCoroutine(FadeCoroutine());
                enabled = false;
            }
        }

        private IEnumerator FadeCoroutine()
        {
            var colorGrading = postVolume.profile.GetSetting<ColorGrading>();

            colorGrading.enabled.value = true;

            yield return DOTween.To(
                () => colorGrading.colorFilter.value,
                value => colorGrading.colorFilter.value = value,
                Color.black,
                fadeSeconds
            ).WaitForCompletion();

            onFadeout.Invoke();
        }
    }
}