using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Zenject;

namespace Horror
{
    [DisallowMultipleComponent]
    public class EndGame : MonoBehaviour
    {
        #region Inspector

        public float delaySeconds = 2;

        public AudioMixer mixer;

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
            StartCoroutine(RestartCoroutine());
        }

        private IEnumerator RestartCoroutine()
        {
            moon.enabled = false;

            yield return new WaitForSeconds(delaySeconds);

            yield return mixer.DOSetFloat("Volume", -80, 0.5f).WaitForCompletion();

            SceneManager.LoadSceneAsync("Game");
        }
    }
    
    [Serializable]
    public class UnityEventEndGame : UnityEvent<EndGame> { }
}