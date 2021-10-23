using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using Zenject;

namespace Horror.UI
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(CanvasGroup))]
    public class OutroCredits : MonoBehaviour
    {
        #region Inspector

        public AudioMixer mixer;

        public float delaySeconds = 2;

        #endregion

        [Inject]
        private Moon moon = null;

        [Inject]
        private void Inject(MoonBlackness moonBlackness)
        {
            moonBlackness.onFadeout.AddListener(OnFadeout);
        }

        private void OnFadeout()
        {
            moon.enabled = false;

            gameObject.SetActive(true);
            var canvasGroup = GetComponent<CanvasGroup>();
            canvasGroup.alpha = 0;
            canvasGroup.DOFade(1, duration: 1).SetDelay(delaySeconds);
            mixer.DOSetFloat("Volume", 0, 0.5f).SetDelay(delaySeconds);
        }
    }
}