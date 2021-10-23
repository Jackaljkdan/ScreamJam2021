using Horror.Interaction;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace Horror
{
    [DisallowMultipleComponent]
    public class RoomsSequence : MonoBehaviour
    {
        #region Inspector

        public List<RoomAlternative> sequence;

        [Header("Runtime")]

        [SerializeField]
        private int _index = 0;

        [ContextMenu("Advance")]
        private void InspectorAdvance()
        {
            if (Application.isPlaying)
                Advance();
        }

        [ContextMenu("Randomize")]
        private void InspectorRandomize()
        {
            if (Application.isPlaying)
                RandomizeExceptCurrentAnd();
        }

        #endregion

        public int Index
        {
            get => _index;
            private set => _index = value;
        }

        public Room Current
        {
            get
            {
                if (Index >= 0 && Index < sequence.Count)
                    return sequence[Index].GetRoom();
                else
                    return null;
            }
        }

        [Inject(Id="room.final")]
        private SlidingDoorInteractable finalDoor = null;

        private void Awake()
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
            Room prevSelected = null;
            Room nextSelected = null;

            if (Index >= 0 && Index < sequence.Count)
                prevSelected = sequence[Index].GetRoom();

            Index++;

            if (Index < sequence.Count)
            {
                RoomAlternative element = sequence[Index];
                nextSelected = element.GetRoom();
                nextSelected.ActivateAlternative(element);
            }
            else if (Index == sequence.Count)
            {
                finalDoor.IsLocked = false;
            }

            if (prevSelected != null || nextSelected != null)
                RandomizeExcept(prevSelected, nextSelected);
        }

        public void RandomizeExcept(params Room[] except)
        {
            Randomize(except);
        }

        public void Randomize(IEnumerable<Room> except)
        {
            foreach (Room room in GetRooms())
                if (except == null || !except.Contains(room))
                    room.ActivateRandomAlternative();
        }

        public void RandomizeExceptCurrentAnd(params Room[] except)
        {
            Room[] current = new Room[1] { Current };

            Randomize(except != null
                ? current.Concat(except)
                : current
            );
        }
    }
    
    [Serializable]
    public class UnityEventRoomsSequence : UnityEvent<RoomsSequence> { }
}