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

        public float cooldownSeconds = 2f;

        public float endSeconds = 33.8f;

        public float minDot = 0.7f;

        public float lookAwayMultiplier = 0.5f;

        public Transform targetAnchor;

        [SerializeField]
        private Transform initialPositionAnchor = null;

        [Header("Runtime")]

        [SerializeField]
        private float audioTime = 0;

        [SerializeField]
        private float secondsSpentLooking = 0;

        [SerializeField]
        private float cooldownElapsed = 0;

        [SerializeField]
        private bool isLooking = false;

        #endregion

        private Tween tween;

        [Inject(Id = "player.camera")]
        private Transform playerCamera = null;

        private void Start()
        {
            initialPositionAnchor.position = transform.position;

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

                cooldownElapsed = 0;
                OnKeepLooking();
            }
            else
            {
                if (isLooking)
                {
                    cooldownElapsed += Time.deltaTime;

                    if (cooldownElapsed >= cooldownSeconds)
                    {
                        OnStopLooking(audioSource);
                        OnKeepLookingAway(audioSource);
                    }
                    else
                    {
                        OnKeepLooking();
                    }
                }
                else
                {
                    OnKeepLookingAway(audioSource);
                }
            }

            audioTime = audioSource.time;

            transform.position = Vector3.Lerp(
                initialPositionAnchor.position,
                targetAnchor.position,
                Mathf.Min(endSeconds, secondsSpentLooking) / endSeconds
            );
        }

        private void OnBeginLooking(AudioSource audioSource)
        {
            isLooking = true;

            tween?.Kill();
            tween = audioSource.DOFade(1, fadeSeconds * (1 - audioSource.volume));

            if (Mathf.Approximately(audioSource.volume, 0))
            {
                audioSource.Play();
                audioSource.time = secondsSpentLooking;
            }
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

        private void OnKeepLookingAway(AudioSource audioSource)
        {
            if (!Mathf.Approximately(audioSource.volume, 0))
            {
                OnKeepLooking();
                return;
            }

            secondsSpentLooking = Mathf.Max(
                secondsSpentLooking - Time.deltaTime * lookAwayMultiplier,
                0
            );
        }

        public void ForceStopLooking()
        {
            OnStopLooking(GetComponent<AudioSource>());
        }
    }
    
    [Serializable]
    public class UnityEventMoon : UnityEvent<Moon> { }
}