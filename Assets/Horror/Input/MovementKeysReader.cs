using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Horror.Input
{
    [Serializable]
    public class MovementKeysReader : IInputKeysReader
    {
        public MovementKeysReader()
        {
            InitMovementKeys();
        }

        public IEnumerable<KeyCode> ReadKeyDown()
        {
            foreach (KeyCode code in movementKeys)
                if (UnityEngine.Input.GetKeyDown(code))
                    yield return code;
        }

        public IEnumerable<KeyCode> ReadKey()
        {
            foreach (KeyCode code in movementKeys)
                if (UnityEngine.Input.GetKey(code))
                    yield return code;
        }

        private List<KeyCode> movementKeys;

        private void InitMovementKeys()
        {
            movementKeys = new List<KeyCode>(8)
            {
                KeyCode.W,
                KeyCode.A,
                KeyCode.S,
                KeyCode.D,
                KeyCode.UpArrow,
                KeyCode.LeftArrow,
                KeyCode.DownArrow,
                KeyCode.RightArrow,
            };
        }
    }
    
    [Serializable]
    public class UnityEventMovementKeysReader : UnityEvent<MovementKeysReader> { }
}