using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Horror.UI
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Image))]
    public class InteractableFeedback : MonoBehaviour
    {
        #region Inspector

        public float transitionSeconds = 0.25f;

        #endregion

        private Tween tween;

        private Color activeColor;

        private Color InactiveColor
        {
            get
            {
                Color color = activeColor;
                color.a = 0;
                return color;
            }
        }

        private void Start()
        {
            var image = GetComponent<Image>();
            activeColor = image.color;
            image.color = InactiveColor;
        }

        public void Show()
        {
            tween?.Kill();

            tween = GetComponent<Image>().DOColor(activeColor, transitionSeconds);
        }

        public void Hide()
        {
            tween.Kill();

            tween = GetComponent<Image>().DOColor(InactiveColor, transitionSeconds);
        }

    }
    
    [Serializable]
    public class UnityEventInteractableFeedback : UnityEvent<InteractableFeedback> { }
}