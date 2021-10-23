using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Horror
{
    [DisallowMultipleComponent]
    public class RoomAlternative : MonoBehaviour
    {
        #region Inspector

        public bool isSpecial = false;

        private void OnValidate()
        {
            if (GetRoom() == null)
                throw new InvalidOperationException($"{name} must be in a room");
        }

        #endregion

        public Room GetRoom()
        {
            // N.B. GetComponentInParent does not work properly when called in OnValidate

            Transform ancestor = transform.parent;

            while (ancestor != null)
            {
                if (ancestor.TryGetComponent(out Room room))
                    return room;

                ancestor = ancestor.parent;
            }

            return null;
        }
    }
}