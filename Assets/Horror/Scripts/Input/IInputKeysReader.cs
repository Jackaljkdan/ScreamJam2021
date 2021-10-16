using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Horror.Input
{
    public interface IInputKeysReader
    {
        IEnumerable<KeyCode> ReadKeyDown();
        IEnumerable<KeyCode> ReadKey();
    }
    
    [Serializable]
    public class UnityEventIInputKeysReader : UnityEvent<IInputKeysReader> { }
}