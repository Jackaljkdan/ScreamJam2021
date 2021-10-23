using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Horror
{
    [DisallowMultipleComponent]
    public class FinalStarsTrigger : MonoBehaviour
    {
        #region Inspector

        public GameObject finalStars;

        #endregion

        private void Start()
        {
            finalStars.SetActive(false);
        }

        private void OnTriggerEnter(Collider other)
        {
            finalStars.SetActive(true);
        }

        private void OnTriggerExit(Collider other)
        {
            finalStars.SetActive(false);
        }
    }
}