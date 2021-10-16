using JK.Utils;
using NUnit.Framework.Constraints;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Horror.Alternatives
{
    [Serializable]
    public class EnumeratingAlternativesSelector
    {
        public void SelectAlternatives(IList<Section> sections, Alternative preselected = null)
        {
            SelectionState state = new SelectionState()
            {
                selected = new HashSet<Alternative>(),
                doneSections = new HashSet<Section>(),
                incompatibles = new HashSet<Alternative>(),
            };

            if (preselected != null)
                Select(preselected.GetSection(), preselected, state);

            RecursiveSelection(0, sections, state);
        }

        private Selection Select(Section section, Alternative alternative, SelectionState state)
        {
            Selection selection = section.SelectAlternative(alternative);
            state.selected.Add(selection.selected);
            state.doneSections.Add(selection.section);

            foreach (var incompatible in selection.selected.Incompatibles)
                state.incompatibles.Add(incompatible);

            return selection;
        }

        private void RecursiveSelection(int currentIndex, IList<Section> sections, SelectionState state)
        {
            Section currentSection = sections[currentIndex];

            if (state.doneSections.Contains(currentSection))
            {
                ContinueRecursion(currentIndex, sections, state);
                return;
            }

            var alternatives = currentSection.GetAlternatives();
            ListUtils.ShuffleInPlace(alternatives);

            foreach (var alternative in alternatives)
            {
                if (state.incompatibles.Contains(alternative))
                    continue;

                SelectionState clonedState = state.Clone();

                Select(currentSection, alternative, clonedState);

                try
                {
                    ContinueRecursion(currentIndex, sections, clonedState);
                    return;
                }
                catch (IncompatibleAlternativesException)
                {
                    continue;
                }
            }

            throw new IncompatibleAlternativesException($"{currentSection.name} has no compatible alternatives");
        }

        private void ContinueRecursion(int currentIndex, IList<Section> sections, SelectionState state)
        {
            int nextIndex = currentIndex + 1;

            if (nextIndex < sections.Count)
                RecursiveSelection(nextIndex, sections, state);
            else
                CheckFinalState(sections, state);
        }

        private void CheckFinalState(IList<Section> sections, SelectionState state)
        {
            foreach (var section in sections)
            {
                foreach (var alternative in section.GetAlternatives())
                {
                    if (alternative.Replacers.Count == 0)
                        continue;

                    if (state.selected.Contains(alternative))
                        continue;

                    if (!IsAnySelected(alternative.Replacers, state))
                        throw new NoReplacerException($"{alternative.name} has no replacer selected");
                }
            }
        }

        private bool IsAnySelected(IReadOnlyList<Alternative> alternatives, SelectionState state)
        {
            foreach (var alt in alternatives)
                if (state.selected.Contains(alt))
                    return true;

            return false;
        }
    }
    
    [Serializable]
    public class UnityEventEnumeratingAlternativesSelector : UnityEvent<EnumeratingAlternativesSelector> { }
}