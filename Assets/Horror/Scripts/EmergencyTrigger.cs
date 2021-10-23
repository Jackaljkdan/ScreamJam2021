using DG.Tweening;
using Horror.Interaction;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Zenject;

namespace Horror
{
    [DisallowMultipleComponent]
    public class EmergencyTrigger : MonoBehaviour
    {
        #region Inspector

        [SerializeField]
        private AudioClip emergencyClip = null;

        [SerializeField]
        private RadioInteractable target = null;

        #endregion

        [Inject]
        private Moon moon = null;

        [Inject(Id = "music")]
        private AudioSource music = null;

        [Inject(Id = "subtitles")]
        private Text subtitles = null;

        private void OnTriggerEnter(Collider other)
        {
            StartCoroutine(EmergencyCoroutine());
        }

        Tween tween;

        private IEnumerator EmergencyCoroutine()
        {
            if (!target.IsOn)
                target.Interact(new RaycastHit());

            target.MainSource.clip = emergencyClip;
            target.MainSource.time = 0;
            target.MainSource.Play();

            target.enabled = false;

            yield return new WaitForSeconds(3);

            tween = subtitles.DOText(
                "EMERGENCY ALERT",
                duration: 1
            );
            tween.SetEase(Ease.Linear);

            yield return tween.WaitForCompletion();

            yield return new WaitForSeconds(0.5f);

            subtitles.text = "";

            tween = subtitles.DOText(
                "THIS MESSAGE IS ISSUED BY THE NATIONAL SECURITY AGENCY",
                duration: 3.5f
            );
            tween.SetEase(Ease.Linear);

            yield return tween.WaitForCompletion();

            yield return new WaitForSeconds(0.5f);

            subtitles.text = "";

            tween = subtitles.DOText(
                "DO NOT LOOK AT THE MOON - THIS IS NOT A TEST",
                duration: 3
            );
            tween.SetEase(Ease.Linear);

            yield return new WaitForSeconds(1);

            moon.enabled = true;
            music.Play();
            target.enabled = true;

            yield return tween.WaitForCompletion();
            
            yield return new WaitForSeconds(1);

            subtitles.text = "";
            tween = subtitles.DOText(
                "DO NOT LOOK AT THE MOON",
                duration: 1.5f
            );
            tween.SetEase(Ease.Linear);

            yield return tween.WaitForCompletion();

            yield return new WaitForSeconds(1);

            subtitles.text = "";

            Destroy(gameObject);
        }

        private void OnDisable()
        {
            moon.enabled = true;

            if (!music.isPlaying)
                music.Play();

            target.enabled = true;

            tween?.Kill();
            subtitles.text = "";
            
            Destroy(gameObject);
        }
    }
}