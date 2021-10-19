using DG.Tweening;
using JK.Interaction;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Zenject;

namespace Horror.Input
{
    public class PlayerInputRaycastInteraction : MonoBehaviour
    {
        #region Inspector

        public LayerMask layerMask = ~0;

        public float distance = 5;

        public float mobileMaxTouchSeconds = 0.2f;

        #endregion

        private bool hasAlreadyFired = false;

        private Dictionary<int, float> touchSeconds = new Dictionary<int, float>();

        //[Inject(Id = "dev")]
        //private Text devTextUi = null;

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void Update()
        {
            if (!Application.isMobilePlatform)
                DesktopUpdate();
            else
                MobileUpdate();
        }

        private void DesktopUpdate()
        {
            if (!hasAlreadyFired)
            {
                if (UnityEngine.Input.GetAxis("Fire1") == 1)
                {
                    Raycast();
                    hasAlreadyFired = true;
                }
            }
            else if (UnityEngine.Input.GetAxis("Fire1") != 1)
            {
                hasAlreadyFired = false;
            }
        }

        private void MobileUpdate()
        {
            int relevantFingers = 0;
            int endingFingers = 0;

            //devTextUi.text = $"max: {mobileMaxTouchSeconds}\n";

            for (int i = 0; i < UnityEngine.Input.touchCount; i++)
            {
                var touch = UnityEngine.Input.GetTouch(i);

                if (touch.position.x < Screen.width / 2f)
                {
                    touchSeconds.Remove(touch.fingerId);
                    continue;
                }

                relevantFingers++;

                try
                {
                    touchSeconds[touch.fingerId] += Time.deltaTime;
                }
                catch (KeyNotFoundException)
                {
                    touchSeconds[touch.fingerId] = Time.deltaTime;
                }

                if (touch.phase != TouchPhase.Ended)
                    continue;

                endingFingers++;

                float fingerSeconds = touchSeconds[touch.fingerId];
                touchSeconds.Remove(touch.fingerId);

                if (hasAlreadyFired)
                    continue;

                if (fingerSeconds <= mobileMaxTouchSeconds)
                {
                    Raycast();
                    hasAlreadyFired = true;
                }
            }

            hasAlreadyFired = false;

            //devTextUi.text += $"relevant: {relevantFingers} ending: {endingFingers}\n";
            //devTextUi.text += $"secs: {string.Join(" ", touchSeconds.Select(kvp => kvp.Value.ToString("0.##")))}\n";
        }

        private void Raycast()
        {
            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, distance, layerMask))
            {
                if (hit.collider.TryGetComponent(out IInteractable interactable))
                    interactable.Interact(hit);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + transform.forward * distance);
        }
    }
}