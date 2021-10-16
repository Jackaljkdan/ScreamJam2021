using JK.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Zenject;

namespace Horror.Actuators.Input
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(IMovementActuator))]
    public class MovementActuatorTouchInput : MovementActuatorInputBehaviour
    {
        #region Inspector

        public float multiplier = 0.01f;

        public float threshold = 1f;

        #endregion

        private Vector3 input = Vector3.zero;

        private int? currentFingerId = null;

        [Inject(Id = "dev")]
        private Text devTextUi = null;

        [Inject]
        private void Inject()
        {
            if (!PlatformUtils.IsMobile())
                Destroy(this);
        }

        private void Update()
        {
            Touch? relevantTouch;

            if (!currentFingerId.HasValue)
                relevantTouch = GetFirstRelevantTouch();
            else
                relevantTouch = GetTouchByFingerId(currentFingerId.Value);

            if (relevantTouch == null)
            {
                currentFingerId = null;
                return;
            }

            Touch touch = relevantTouch.Value;
            currentFingerId = touch.fingerId;

            devTextUi.text = $"{touch.deltaPosition}";

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    input = Vector3.forward;
                    break;
                case TouchPhase.Moved:
                    if (Mathf.Abs(touch.deltaPosition.x) >= threshold)
                        input.x += touch.deltaPosition.x * multiplier * 60 * Time.deltaTime;

                    if (Mathf.Abs(touch.deltaPosition.y) >= threshold)
                        input.z += touch.deltaPosition.y * multiplier * 60 * Time.deltaTime;

                    input = Vector3.ClampMagnitude(input, 1);

                    break;
            }

            GetComponent<IMovementActuator>().Input = input;
        }

        private Touch? GetFirstRelevantTouch()
        {
            foreach (var touch in UnityEngine.Input.touches)
                if (touch.position.x < Screen.width / 2.0)
                    return touch;

            return null;
        }

        private Touch? GetTouchByFingerId(int fingerId)
        {
            foreach (var touch in UnityEngine.Input.touches)
                if (touch.fingerId == fingerId)
                    return touch;

            return null;
        }
    }
    
    [Serializable]
    public class UnityEventMovementActuatorTouchInput : UnityEvent<MovementActuatorTouchInput> { }
}