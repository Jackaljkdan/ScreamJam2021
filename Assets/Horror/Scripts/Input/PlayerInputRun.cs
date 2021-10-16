using Horror.Actuators;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace Horror.Input
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(IMovementActuator))]
    public class PlayerInputRun : MonoBehaviour
    {
        #region Inspector

        [SerializeField]
        private float _runSpeedMultiplier = 2;

        public float accelerationRate = 0.15f;
        public float decelerationRate = 0.12f;

        [Tooltip("How much time before decelerating after action?")]
        public float nonDeceleratingTime = 0.3f;

        private void OnValidate()
        {
            _runSpeedMultiplier = Mathf.Max(1, _runSpeedMultiplier);

            if (Application.isPlaying)
                RefreshSpeeds();
        }

        #endregion

        public float RunSpeedMultiplier
        {
            get => _runSpeedMultiplier;
            set
            {
                _runSpeedMultiplier = value;
                RefreshSpeeds();
            }
        }

        private float walkSpeed;
        private float runSpeed;

        private float decelerationWaitTime = 0;

        [Inject]
        private ActionKeysReader actionKeys = null;

        private void Awake()
        {
            RefreshSpeeds();
        }

        private void RefreshSpeeds()
        {
            walkSpeed = GetComponent<IMovementActuator>().Speed;
            runSpeed = walkSpeed * RunSpeedMultiplier;
        }

        private void Update()
        {
            KeyCode code = actionKeys.ReadKeyDown().FirstOrDefault();

            var movementActuator = GetComponent<IMovementActuator>();

            if (code != default(KeyCode))
            {
                StopAllCoroutines();
                StartCoroutine(AccelerateCoroutine(movementActuator));
                decelerationWaitTime = 0;
            }

            decelerationWaitTime = Mathf.Min(decelerationWaitTime + Time.deltaTime, nonDeceleratingTime);

            if (Mathf.Approximately(decelerationWaitTime, nonDeceleratingTime))
                movementActuator.Speed = Mathf.Lerp(movementActuator.Speed, walkSpeed, decelerationRate);
        }

        private IEnumerator AccelerateCoroutine(IMovementActuator movementActuator)
        {
            float elapsed = 0;
            float duration = 0.5f;

            while (elapsed < duration)
            {
                movementActuator.Speed = Mathf.Lerp(movementActuator.Speed, runSpeed, accelerationRate);
                elapsed += Time.deltaTime;
                yield return null;
            }
        }
    }
    
    [Serializable]
    public class UnityEventPlayerInputRun : UnityEvent<PlayerInputRun> { }
}