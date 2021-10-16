using JK.Events;
using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Horror.Actuators
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Rigidbody))]
    public class RigidBodyMovementActuator : MonoBehaviour, IMovementActuator
    {
        #region Inspector

        [SerializeField]
        private float _speed = 3;

        //[SerializeField, ReadOnly]
        //private float bodySpeed = 0;

        public Transform _directionReference;

        [SerializeField]
        private UnityEventVector3 _onMovement = new UnityEventVector3();

        private void Reset()
        {
            DirectionReference = transform;
        }

        private void OnValidate()
        {
            GetComponent<Rigidbody>().isKinematic = false;
        }

        #endregion

        public float Speed
        {
            get => _speed;
            set => _speed = value;
        }

        private float Acceleration => Speed * 21.4146f + 6.05739f;

        public Vector3 Input { get; set; }

        public Transform DirectionReference
        {
            get => _directionReference;
            set => _directionReference = value;
        }

        public UnityEventVector3 onMovement => _onMovement;

        private void FixedUpdate()
        {
            //bodySpeed = (GetComponent<Rigidbody>().velocity.magnitude + bodySpeed * 1) / 2;

            if (Input.sqrMagnitude == 0)
                return;

            var body = GetComponent<Rigidbody>();

            Vector3 clampedInput = Vector3.ClampMagnitude(Input, 1);

            Vector3 directionedInput = DirectionReference.TransformDirection(clampedInput);
            directionedInput.y = 0;
            directionedInput = directionedInput.normalized * clampedInput.magnitude;

            Vector3 directionedAcceleration = directionedInput * Acceleration;

            body.AddForce(directionedAcceleration, ForceMode.Acceleration);

            // despite this, increasing the acceleration will still allow reaching higher velocities
            // turning down the speed will decrease the maximum velocity, but not exactly to its value
            //body.velocity = Vector3.ClampMagnitude(body.velocity, Speed);

            onMovement.Invoke(Input);
        }
    }
    
    [Serializable]
    public class UnityEventRigidBodyMovementActuator : UnityEvent<RigidBodyMovementActuator> { }
}