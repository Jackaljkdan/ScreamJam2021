using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Horror
{
    [DisallowMultipleComponent]
    public class Room : MonoBehaviour
    {
        #region Inspector

        

        #endregion

        public IEnumerable<GameObject> GetAlternatives()
        {
            foreach (Transform child in transform)
                if (child.CompareTag("Alternative"))
                    yield return child.gameObject;
        }

        public void ActivateRandomAlternative()
        {
            List<GameObject> all = new List<GameObject>(GetAlternatives());
            int randomIndex = UnityEngine.Random.Range(0, all.Count);
            ActivateAlternative(all[randomIndex], all);
        }

        public void ActivateAlternative(GameObject alternative)
        {
            ActivateAlternative(alternative, GetAlternatives());
        }

        private void ActivateAlternative(GameObject selected, IEnumerable<GameObject> all)
        {
            foreach (GameObject alt in all)
                alt.SetActive(alt == selected);

            selected.SetActive(true);
        }
    }
    
    [Serializable]
    public class UnityEventRoom : UnityEvent<Room> { }
}