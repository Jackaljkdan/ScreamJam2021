using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace Horror
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(AudioSource))]
    public class Moon : MonoBehaviour
    {
        #region Inspector

        public float fadeSeconds = 2f;

        public float minDot = 0.7f;

        public float lookAwayMultiplier = 0.5f;

        public Transform targetAnchor;

        [Header("Runtime")]

        [SerializeField]
        private float secondsSpentLooking = 0;

        [SerializeField]
        private bool isLooking = false;

        #endregion

        private Vector3 originalPosition;

        private Tween tween;

        [Inject(Id = "player.camera")]
        private Transform playerCamera = null;

        private void Start()
        {
            originalPosition = transform.position;

            var audioSource = GetComponent<AudioSource>();
            audioSource.volume = 0;
        }

        private void Update()
        {
            var audioSource = GetComponent<AudioSource>();

            Vector3 directionFromPlayer = (transform.position - playerCamera.position).normalized;
            float lookDot = Vector3.Dot(playerCamera.forward, directionFromPlayer);

            if (lookDot >= minDot)
            {
                if (!isLooking)
                    OnBeginLooking(audioSource);

                OnKeepLooking();
            }
            else
            {
                if (isLooking)
                    OnStopLooking(audioSource);

                OnKeepLookingAway();
            }

            transform.position = Vector3.Lerp(
                originalPosition,
                targetAnchor.position,
                Mathf.Min(audioSource.clip.length, secondsSpentLooking) / audioSource.clip.length
            );
        }

        private void OnBeginLooking(AudioSource audioSource)
        {
            isLooking = true;

            if (audioSource.volume == 0)
                audioSource.time = secondsSpentLooking;

            tween?.Kill();
            tween = audioSource.DOFade(1, fadeSeconds * (1 - audioSource.volume));
            audioSource.Play();
        }

        private void OnStopLooking(AudioSource audioSource)
        {
            isLooking = false;
            
            tween?.Kill();
            tween = audioSource.DOFade(0, fadeSeconds * audioSource.volume);
        }

        private void OnKeepLooking()
        {
            secondsSpentLooking += Time.deltaTime;
        }

        private void OnKeepLookingAway()
        {
            secondsSpentLooking = Mathf.Max(
                secondsSpentLooking - Time.deltaTime * lookAwayMultiplier,
                0
            );
        }
    }
    
    [Serializable]
    public class UnityEventMoon : UnityEvent<Moon> { }
}