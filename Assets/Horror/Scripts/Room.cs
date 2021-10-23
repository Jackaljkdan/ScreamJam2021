using Horror.Interaction;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Horror
{
    [DisallowMultipleComponent]
    public class Room : MonoBehaviour
    {
        #region Inspector

        public Transform compassTarget;

        public SlidingDoorInteractable door;

        #endregion

        public IEnumerable<RoomAlternative> GetAlternatives()
        {
            foreach (Transform child in transform)
                if (child.TryGetComponent(out RoomAlternative alt))
                    yield return alt;
        }

        public void ActivateRandomAlternative()
        {
            IEnumerable<RoomAlternative> all = GetAlternatives();
            List<RoomAlternative> common = new List<RoomAlternative>(all.Where(alt => !alt.isSpecial));
            int randomIndex = UnityEngine.Random.Range(0, common.Count);
            ActivateAlternative(common[randomIndex], all);
        }

        public void ActivateAlternative(RoomAlternative alternative)
        {
            ActivateAlternative(alternative, GetAlternatives());
        }

        private void ActivateAlternative(RoomAlternative selected, IEnumerable<RoomAlternative> all)
        {
            foreach (RoomAlternative alt in all)
                alt.gameObject.SetActive(alt == selected);

            selected.gameObject.SetActive(true);

            door.ForceClose();
        }
    }
    
    [Serializable]
    public class UnityEventRoom : UnityEvent<Room> { }
}