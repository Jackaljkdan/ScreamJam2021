using JK.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Horror.Actuators
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Rigidbody))]
    public class KinematicBodyMovementActuator : MonoBehaviour, IMovementActuator
    {
        #region Inspector

        [SerializeField]
        private float _speed = 3;

        public Transform _directionReference;

        [SerializeField]
        private UnityEventVector3 _onMovement = new UnityEventVector3();

        private void Reset()
        {
            DirectionReference = transform;
        }

        private void OnValidate()
        {
            GetComponent<Rigidbody>().isKinematic = true;
        }

        #endregion

        public float Speed
        {
            get => _speed;
            set => _speed = value;
        }

        public Vector3 Input { get; set; }

        public Transform DirectionReference
        {
            get => _directionReference;
            set => _directionReference = value;
        }

        public UnityEventVector3 onMovement => _onMovement;

        private void FixedUpdate()
        {
            if (Input.sqrMagnitude == 0)
                return;

            var body = GetComponent<Rigidbody>();
            
            Vector3 directionedInput = DirectionReference.TransformDirection(Input);
            directionedInput.y = 0;
            directionedInput = directionedInput.normalized;

            body.MovePosition(body.position + directionedInput * Speed * Time.fixedDeltaTime);

            onMovement.Invoke(Input);
        }
    }
    
    [Serializable]
    public class UnityEventKinematicBodyMovementActuator : UnityEvent<KinematicBodyMovementActuator> { }
}