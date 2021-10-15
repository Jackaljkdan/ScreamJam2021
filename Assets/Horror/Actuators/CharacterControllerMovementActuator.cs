using JK.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Horror.Actuators
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(CharacterController))]
    public class CharacterControllerMovementActuator : MonoBehaviour, IMovementActuator
    {
        #region Inspector

        [SerializeField]
        private float _speed = 3;

        [SerializeField]
        private Transform _directionReference;

        [SerializeField]
        private UnityEventVector3 _onMovement = new UnityEventVector3();

        private void Reset()
        {
            DirectionReference = transform;
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

        private void Update()
        {
            if (Input.sqrMagnitude == 0)
                return;

            var cc = GetComponent<CharacterController>();

            Vector3 velocity = Input * Speed;

            //Debug.Log($"vel {velocity}");
            //Debug.Log($"input {Input.GetAxis("Horizontal")} {Input.GetAxis("Vertical")}");

            cc.SimpleMove(DirectionReference.TransformDirection(velocity));

            onMovement.Invoke(Input);

            Input = Vector3.zero;
        }
    }
    
    [Serializable]
    public class UnityEventCharacterControllerMovementActuator : UnityEvent<CharacterControllerMovementActuator> { }
}