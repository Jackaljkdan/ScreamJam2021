using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Horror.Actuators.Input
{
    public class RotationActuatorInputBehaviour : MonoBehaviour
    {
        #region Inspector

        

        #endregion
    }
    
    [Serializable]
    public class UnityEventRotationActuatorInputBehaviour : UnityEvent<RotationActuatorInputBehaviour> { }
}