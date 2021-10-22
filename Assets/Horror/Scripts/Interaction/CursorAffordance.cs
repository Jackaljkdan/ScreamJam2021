using Horror.UI;
using JK.Interaction;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace Horror.Interaction
{
    [DisallowMultipleComponent]
    public class CursorAffordance : AffordanceBehaviour
    {
        #region Inspector



        #endregion

        [Inject]
        private InteractableFeedback feedback = null;

        protected override void StartHighlight(RaycastHit hit)
        {
            feedback.Show();
        }

        protected override void StayHighlight(RaycastHit hit)
        {
        }

        protected override void StopHighlight()
        {
            feedback.Hide();
        }
    }
    
    [Serializable]
    public class UnityEventCursorAffordance : UnityEvent<CursorAffordance> { }
}