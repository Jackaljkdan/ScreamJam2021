using JK.Interaction;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace Horror.Interaction
{
    public class RadioInteractable : InteractableBehaviour
    {
        #region Inspector

        [SerializeField]
        private AudioSource audioSource = null;

        [SerializeField]
        private MeshRenderer emissiveButtonRenderer = null;

        #endregion

        private void Start()
        {
            emissiveButtonRenderer.material.DisableKeyword("_EMISSION");
        }

        protected override void PerformInteraction(RaycastHit hit)
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
                emissiveButtonRenderer.material.DisableKeyword("_EMISSION");
            }
            else
            {
                audioSource.Play();
                emissiveButtonRenderer.material.EnableKeyword("_EMISSION");
            }
        }
    }
    
    [Serializable]
    public class UnityEventRadioInteractable : UnityEvent<RadioInteractable> { }
}