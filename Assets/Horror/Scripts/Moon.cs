using DG.Tweening;
using JK.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
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

        public AnimationCurve fadeCurve;

        public float cooldownSeconds = 2f;

        public float endSeconds = 33.8f;

        public float minDot = 0.7f;

        public bool ignoreRaycast = false;

        public LayerMask raycastMask;

        public float lookAwayMultiplier = 0.5f;

        public AudioMixer mixer;

        public Transform targetAnchor;

        public AnimationCurve movementCurve;

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

        public float Intensity => Mathf.Min(endSeconds, secondsSpentLooking) / endSeconds;

        public bool IsLooking => isLooking;

        private LayerMask playerLayer;

        private Tween tween;

        [Inject(Id = "player.camera")]
        private Transform playerCamera = null;

        private void Start()
        {
            initialPositionAnchor.position = transform.position;
            playerLayer = LayerMask.NameToLayer("Player");

            var audioSource = GetComponent<AudioSource>();
            audioSource.volume = 0;
        }

        private void Update()
        {
            var audioSource = GetComponent<AudioSource>();

            Vector3 directionFromPlayer = (transform.position - playerCamera.position).normalized;
            float lookDot = Vector3.Dot(playerCamera.forward, directionFromPlayer);

            bool isMoonVisible = lookDot >= minDot;

            if (isMoonVisible && !ignoreRaycast)
            {
                isMoonVisible = false;

                if (RaycastUtils.Cast(transform.position, playerCamera, out RaycastHit hit, raycastMask))
                    isMoonVisible = hit.collider.gameObject.layer == playerLayer;
            }

            if (isMoonVisible)
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
                movementCurve.Evaluate(Intensity)
            );

            float globalVolume = (1 - audioSource.volume);
            float lowestDb = -30;
            globalVolume = (globalVolume * -lowestDb) + lowestDb;

            if (mixer != null)
                mixer.SetFloat("Volume", globalVolume);
        }

        private void OnBeginLooking(AudioSource audioSource)
        {
            isLooking = true;

            tween?.Kill();
            tween = audioSource.DOFade(1, fadeSeconds * (1 - audioSource.volume));
            tween.SetEase(fadeCurve);

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