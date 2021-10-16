using Horror.Actuators;
using JK.Events;
using JK.Utils;
using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace Horror.Actuators.Input
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(IMovementActuator))]
    public class MovementActuatorAxisInput : MovementActuatorInputBehaviour
    {
        #region Inspector

        #endregion

        [Inject]
        private void Inject()
        {
            if (!PlatformUtils.IsDesktop())
                Destroy(this);
        }

        private void Update()
        {
            Vector3 input = new Vector3(
                UnityEngine.Input.GetAxis("Horizontal"),
                0,
                UnityEngine.Input.GetAxis("Vertical")
            );

            GetComponent<IMovementActuator>().Input = input;
        }
    }
}