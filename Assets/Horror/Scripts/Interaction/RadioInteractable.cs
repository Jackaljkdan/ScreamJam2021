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
        private AudioSource mainSource = null;

        [SerializeField]
        private AudioSource backgroundSource = null;

        [SerializeField]
        private MeshRenderer emissiveButtonRenderer = null;

        [SerializeField]
        private bool startsOn = true;

        #endregion

        public bool IsOn { get; private set; }

        public AudioSource MainSource => mainSource;

        private IEnumerable<AudioSource> sources
        {
            get
            {
                yield return mainSource;
                yield return backgroundSource;
            }
        }

        private Dictionary<AudioSource, float> volumes;

        private void Start()
        {
            volumes = new Dictionary<AudioSource, float>();

            foreach (var src in sources)
                volumes.Add(src, src.volume);

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
                foreach (var src in sources)
                    src.volume = volumes[src];

                emissiveButtonRenderer.material.EnableKeyword("_EMISSION");
            }
            else
            {
                foreach (var src in sources)
                    src.volume = 0;

                emissiveButtonRenderer.material.DisableKeyword("_EMISSION");
            }
        }
    }
    
    [Serializable]
    public class UnityEventRadioInteractable : UnityEvent<RadioInteractable> { }
}