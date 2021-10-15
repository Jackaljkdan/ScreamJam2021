using JK.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Horror.Actuators
{
    public interface IMovementActuator
    {
        float Speed { get; set; }
        Vector3 Input { get; set; }
        Transform DirectionReference { get; set; }
        UnityEventVector3 onMovement { get; }
    }
    
    [Serializable]
    public class UnityEventIMovementActuator : UnityEvent<IMovementActuator> { }
}