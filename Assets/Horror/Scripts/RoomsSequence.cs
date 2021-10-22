using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Horror
{
    [DisallowMultipleComponent]
    public class RoomsSequence : MonoBehaviour
    {
        #region Inspector

        public List<GameObject> sequence;

        [Header("Runtime")]

        [SerializeField]
        private int _index = 0;

        private void OnValidate()
        {
            foreach (GameObject elem in sequence)
            {
                if (elem == null)
                    throw new ArgumentException($"sequence can't have null elements");

                var room = elem.GetComponentInParent<Room>();
                if (room == null)
                    throw new ArgumentException($"{elem.name} is not in a room");
            }
        }

        [ContextMenu("Advance")]
        private void InspectorAdvance()
        {
            Advance();
        }

        [ContextMenu("Randomize")]
        private void InspectorRandomize()
        {
            Randomize();
        }

        #endregion

        public int Index
        {
            get => _index;
            private set => _index = value;
        }

        private void Start()
        {
            Index = -1;
            Advance();
        }

        public IEnumerable<Room> GetRooms()
        {
            foreach (Transform child in transform)
                if (child.TryGetComponent(out Room room))
                    yield return room;
        }

        public void Advance()
        {
            Index++;

            Room preselected = null;

            if (Index < sequence.Count)
            {
                GameObject element = sequence[Index];
                preselected = element.GetComponentInParent<Room>();
                preselected.ActivateAlternative(element);
            }

            Randomize(preselected);
        }

        public void Randomize(Room preselected = null)
        {
            foreach (Room room in GetRooms())
                if (room != preselected)
                    room.ActivateRandomAlternative();
        }
    }
    
    [Serializable]
    public class UnityEventRoomsSequence : UnityEvent<RoomsSequence> { }
}