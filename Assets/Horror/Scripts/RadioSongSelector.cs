using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace Horror
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(AudioSource))]
    public class RadioSongSelector : MonoBehaviour
    {
        #region Inspector

        public AudioClip clipAfterEmergency;

        #endregion

        [Inject]
        private RoomsSequence roomsSequence = null;

        private void OnEnable()
        {
            if (roomsSequence.Index > 0)
            {
                var audioSource = GetComponent<AudioSource>();

                bool wasPlaying = audioSource.isPlaying;

                audioSource.clip = clipAfterEmergency;
                if (wasPlaying)
                    audioSource.Play();
            }
        }
    }
    
    [Serializable]
    public class UnityEventRadioSongSelector : UnityEvent<RadioSongSelector> { }
}