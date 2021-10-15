using Horror.Actuators;
using JK.Sounds;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace Horror.Input
{
    [RequireComponent(typeof(IMovementActuator))]
    public class PlayerInputFootsteps : MonoBehaviour
    {
        #region Inspector

        [SerializeField]
        private float secondsBetweenSteps = 0.5f;

        [SerializeField]
        private List<AudioClip> clips = null;

        #endregion

        private int lastSourceIndex = 0;
        private bool wasMoving = false;
        private float secondsSinceFootstep = 0;
        private float slowestSpeed;

        [Inject]
        private List<RandomClipsPlayer> players = null;

        [Inject]
        private void Inject()
        {
            foreach (RandomClipsPlayer player in players)
                player.clips = clips;
        }

        private void Start()
        {
            var controller = GetComponent<IMovementActuator>();
            controller.onMovement.AddListener(OnMovement);
            slowestSpeed = controller.Speed;
        }

        private void OnMovement(Vector3 velocity)
        {
            if (velocity.sqrMagnitude > 0)
            {
                float speedFactor = GetComponent<IMovementActuator>().Speed / slowestSpeed;
                float proportionalSecondsBetweenSteps = secondsBetweenSteps / speedFactor;

                if (!wasMoving)
                {
                    float halfSecondsBetween = proportionalSecondsBetweenSteps / 2;
                    if (secondsSinceFootstep < halfSecondsBetween)
                        secondsSinceFootstep = halfSecondsBetween;

                    wasMoving = true;
                }

                secondsSinceFootstep += Time.deltaTime;

                if (secondsSinceFootstep >= proportionalSecondsBetweenSteps)
                {
                    secondsSinceFootstep = 0;
                    PlayFootstep();
                }
            }
            else if (wasMoving)
            {
                wasMoving = false;
            }
        }

        private void PlayFootstep()
        {
            lastSourceIndex = (lastSourceIndex + 1) % players.Count;
            players[lastSourceIndex].PlayRandom();
        }

        private void OnDestroy()
        {
            GetComponent<IMovementActuator>().onMovement.RemoveListener(OnMovement);
        }
    }
    
    [Serializable]
    public class UnityEventPlayerInputFootsteps : UnityEvent<PlayerInputFootsteps> { }
}