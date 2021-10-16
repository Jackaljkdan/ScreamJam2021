using JK.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Horror.Alternatives
{
    [DisallowMultipleComponent]
    public class Section : MonoBehaviour
    {
        #region Inspector

        

        #endregion

        private void Start()
        {
            
        }

        public Alternative[] GetAlternatives()
        {
            return GetComponentsInChildren<Alternative>(includeInactive: true);
        }

        public Selection SelectAlternative(Alternative preselected)
        {
            Alternative[] all = GetAlternatives();
            return ActivateSelectedAlternative(preselected, all);
        }

        public Selection ActivateSelectedAlternative(Alternative selected, Alternative[] all)
        {
            List<Alternative> needReplacement = new List<Alternative>(all.Length - 1);

            foreach (Alternative alternative in all)
            {
                alternative.gameObject.SetActive(alternative == selected);

                if (alternative != selected && alternative.Replacers.Count > 0)
                    needReplacement.Add(alternative);
            }

            return new Selection(this, selected, needReplacement);
        }

        public void SelectAlternativesInAllSections(Alternative preselected = null)
        {
            Section[] all = transform.parent.GetComponentsInChildren<Section>();
            new EnumeratingAlternativesSelector().SelectAlternatives(all, preselected);
        }
    }

    [Serializable]
    public class UnityEventSection : UnityEvent<Section> { }
}