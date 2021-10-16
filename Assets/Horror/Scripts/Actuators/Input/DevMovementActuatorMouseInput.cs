using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Horror.Actuators.Input
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(IMovementActuator))]
    public class DevMovementActuatorMouseInput : MonoBehaviour
    {
        #region Inspector

        public float angleMultiplier = 0.1f;

        public float addMultiplier = 0.01f;

        public Vector3 input = Vector3.forward;

        public Vector2 axis;

        #endregion

        private void Start()
        {
            Destroy(GetComponent<MovementActuatorAxisInput>());
            Destroy(GetComponent<MovementActuatorTouchInput>());
        }

        private void Update()
        {
            axis = new Vector2(UnityEngine.Input.GetAxis("Mouse X"), UnityEngine.Input.GetAxis("Mouse Y"));

            input.x += axis.x * addMultiplier * 60 * Time.deltaTime;
            input.z += axis.y * addMultiplier * 60 * Time.deltaTime;

            input = Vector3.ClampMagnitude(input, 1);

            GetComponent<IMovementActuator>().Input = input;
        }
    }
    
    [Serializable]
    public class UnityEventMovementActuatorMouseInput : UnityEvent<DevMovementActuatorMouseInput> { }
}