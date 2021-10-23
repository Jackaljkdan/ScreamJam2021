using DG.Tweening;
using Horror.Actuators;
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

        public bool win = false;

        public AudioMixer mixer;

        #endregion

        [Inject]
        private Moon moon = null;

        [Inject(Id = "outro")]
        private CanvasGroup credits = null;

        [Inject]
        private RigidBodyMovementActuator movement = null;

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

            yield return mixer.DOSetFloat("Volume", -80, 0.5f).WaitForCompletion();

            yield return new WaitForSeconds(delaySeconds);

            movement.enabled = false;
            mixer.DOSetFloat("Volume", 0, 0.5f);

            if (!win)
            {
                SceneManager.LoadSceneAsync("Game");
                yield break;
            }

            credits.gameObject.SetActive(true);
            credits.alpha = 0;

            credits.DOFade(1, duration: 1);
        }
    }
    
    [Serializable]
    public class UnityEventEndGame : UnityEvent<EndGame> { }
}