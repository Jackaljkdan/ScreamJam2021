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

        [SerializeField]
        private bool startsOn = true;

        public float onVolume = 1;

        #endregion

        public bool IsOn { get; private set; }

        private void Start()
        {
            emissiveButtonRenderer.material.DisableKeyword("_EMISSION");
            IsOn = startsOn;
            SetOn(IsOn);
        }

        protected override void PerformInteraction(RaycastHit hit)
        {
            if (!enabled)
                return;

            SetOn(!IsOn);
        }

        public void SetOn(bool on)
        {
            IsOn = on;

            if (IsOn)
            {
                audioSource.volume = onVolume;
                emissiveButtonRenderer.material.EnableKeyword("_EMISSION");
            }
            else
            {
                audioSource.volume = 0;
                emissiveButtonRenderer.material.DisableKeyword("_EMISSION");
            }
        }
    }
    
    [Serializable]
    public class UnityEventRadioInteractable : UnityEvent<RadioInteractable> { }
}