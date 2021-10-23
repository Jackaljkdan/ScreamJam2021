using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Horror.UI
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(CanvasGroup))]
    public class IntroCredits : MonoBehaviour
    {
        #region Inspector

        [SerializeField]
        private float fadeInSeconds = 0.2f;

        [SerializeField]
        private float fadeOutSeconds = 0.6f;

        #endregion

        private void Start()
        {
            GetComponent<CanvasGroup>().DOFade(1, fadeInSeconds);
        }

        private void Update()
        {
            if (UnityEngine.Input.anyKeyDown
                || UnityEngine.Input.GetAxis("Mouse X") != 0
                || UnityEngine.Input.GetAxis("Mouse Y") != 0
            )
            {
                StartCoroutine(FadeCoroutine());
                enabled = false;
            }
        }

        private IEnumerator FadeCoroutine()
        {
            yield return GetComponent<CanvasGroup>().DOFade(0, fadeOutSeconds).WaitForCompletion();
            Destroy(gameObject);
        }
    }
    
    [Serializable]
    public class UnityEventIntroCredits : UnityEvent<IntroCredits> { }
}