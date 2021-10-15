using JK.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Horror.Alternatives
{
    [DisallowMultipleComponent]
    public class Alternative : MonoBehaviour
    {
        #region Inspector

        [Tooltip("Alternatives in other sections that can't be active when this one is")]
        [SerializeField]
        private List<Alternative> incompatibles = null;
        
        [Tooltip("One of these must be active if this is not")]
        [SerializeField]
        private List<Alternative> replacers = null;

        [SerializeField, HideInInspector]
        private List<Alternative> previousIncompatibles = null;

        [ContextMenu("Set as current alternative")]
        private void InspectorSetAsCurrentAlternative()
        {
            GetSection().SelectAlternativesInAllSections(preselected: this);
        }

        private void OnValidate()
        {
            Undo.SetCurrentUndoGroupName("edit incompatible alternatives");
            int undoGroup = Undo.GetCurrentUndoGroup();

            Undo.RecordObjectForUndo(gameObject, "");

            foreach (var incomp in incompatibles)
            {
                if (incomp == null)
                    continue;

                if (!incomp.incompatibles.Contains(this))
                {
                    Undo.RecordObjectForUndo(incomp.gameObject, "");
                    incomp.incompatibles.Add(this);
                    incomp.previousIncompatibles = new List<Alternative>(incomp.incompatibles);
                }
            }

            foreach (var incomp in previousIncompatibles)
            {
                if (incomp == null)
                    continue;

                if (!incompatibles.Contains(incomp))
                {
                    Undo.RecordObjectForUndo(incomp.gameObject, "");
                    incomp.incompatibles.Remove(this);
                    incomp.previousIncompatibles = new List<Alternative>(incomp.incompatibles);
                }
            }

            previousIncompatibles = new List<Alternative>(incompatibles);

            Undo.CollapseUndoOperations(undoGroup);
        }

        #endregion

        public IReadOnlyList<Alternative> Incompatibles => incompatibles;
        public IReadOnlyList<Alternative> Replacers => replacers;

        //#if UNITY_EDITOR
        //        private void Awake()
        //        {
        //            UnityEditor.Selection.selectionChanged -= OnEditorSelectionChanged;
        //            UnityEditor.Selection.selectionChanged += OnEditorSelectionChanged;
        //        }

        //        private void OnEditorSelectionChanged()
        //        {
        //            if (UnityEditor.Selection.activeGameObject == gameObject)
        //        }

        //        private void OnDestroy()
        //        {
        //            UnityEditor.Selection.selectionChanged -= OnEditorSelectionChanged;
        //        }
        //#endif

        private void Start()
        {
            
        }

        public Section GetSection()
        {
            return GetComponentInParent<Section>();
        }
    }
    
    [Serializable]
    public class UnityEventAlternative : UnityEvent<Alternative> { }
}