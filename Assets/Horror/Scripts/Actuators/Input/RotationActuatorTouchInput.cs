using JK.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace Horror.Actuators.Input
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(IRotationActuator))]
    public class RotationActuatorTouchInput : RotationActuatorInputBehaviour
    {
        #region Inspector

        public float rotationSpeed = 3;

        public float touchMultiplier = 0.1f;

        #endregion

        [Inject]
        private void Inject()
        {
            if (!PlatformUtils.IsMobile())
                Destroy(this);
        }

        private void LateUpdate()
        {
            float leftRightRotation = 0;
            float upDownRotation = 0;

            float halfScreenWidth = Screen.width / 2.0f;

            foreach (var touch in UnityEngine.Input.touches)
            {
                if (touch.phase != TouchPhase.Moved)
                    continue;

                if (touch.position.x > halfScreenWidth)
                {
                    leftRightRotation += touch.deltaPosition.x * touchMultiplier;
                    upDownRotation += touch.deltaPosition.y * touchMultiplier;
                }
            }

            GetComponent<IRotationActuator>().Input = new Vector2(leftRightRotation, upDownRotation);
        }
    }

    [Serializable]
    public class UnityEventRotationTouchInput : UnityEvent<RotationActuatorTouchInput> { }
}