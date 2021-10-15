using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Horror.Input
{
    public class ActionKeysReader : IInputKeysReader
    {

        public ActionKeysReader()
        {
            InitActionKeys();
        }

        public IEnumerable<KeyCode> ReadKeyDown()
        {
            if (!UnityEngine.Input.anyKeyDown)
                yield break;

            foreach (KeyCode code in actionKeys)
            {
                if (!UnityEngine.Input.GetKeyDown(code))
                    continue;

                yield return code;
            }
        }

        public IEnumerable<KeyCode> ReadKey()
        {
            if (!UnityEngine.Input.anyKey)
                yield break;

            foreach (KeyCode code in actionKeys)
            {
                if (!UnityEngine.Input.GetKey(code))
                    continue;

                yield return code;
            }
        }

        private List<KeyCode> actionKeys;

        private void InitActionKeys()
        {
            actionKeys = new List<KeyCode>()
            {
                KeyCode.Escape,
                KeyCode.Tilde,
                KeyCode.Backslash,
                KeyCode.Tab,
                KeyCode.LeftShift,
                KeyCode.LeftControl,
                KeyCode.LeftAlt,
                KeyCode.Space,
                KeyCode.AltGr,
                KeyCode.RightAlt,
                KeyCode.RightControl,
                KeyCode.RightShift,
                KeyCode.Return,
                KeyCode.Backspace,
                KeyCode.Mouse0,
                KeyCode.Mouse1,
                KeyCode.Mouse2,
            };

            for (KeyCode letter = KeyCode.A; letter <= KeyCode.Z; letter++)
            {
                switch (letter)
                {
                    case KeyCode.W:
                    case KeyCode.A:
                    case KeyCode.S:
                    case KeyCode.D:
                        continue;
                    default:
                        actionKeys.Add(letter);
                        break;
                }
            }

            for (KeyCode number = KeyCode.Alpha0; number <= KeyCode.Alpha9; number++)
                actionKeys.Add(number);
        }
    }
}