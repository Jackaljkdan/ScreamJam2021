using JK.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Horror.Alternatives
{
    [Serializable]
    public class TooComplexAlternativesSelector
    {
        public static void SelectAlternatives(IList<Section> sections, Alternative preselected = null)
        {
            SelectionState state = new SelectionState()
            {
                selected = new HashSet<Alternative>(),
                doneSections = new HashSet<Section>(),
                incompatibles = new HashSet<Alternative>(),
            };

            if (preselected == null)
            {
                RecursiveSelection(0, sections, state);
                return;
            }

            Section section = preselected.GetSection();
            Selection selection = section.SelectAlternative(preselected);
            ProcessSelection(selection, state);

            RecursiveSelection(0, sections, state);
        }

        private static void RecursiveSelection(int currentIndex, IList<Section> sections, SelectionState state)
        {
            Section currentSection = sections[currentIndex];

            if (state.doneSections.Contains(currentSection))
            {
                ContinueRecursiveSelection(currentIndex, sections, state);
                return;
            }

            TryAllAlternatives(currentIndex, currentSection, sections, state);
        }

        private static void TryAllAlternatives(int currentIndex, Section currentSection, IList<Section> sections, SelectionState state)
        {
            Alternative[] all = currentSection.GetAlternatives();
            ListUtils.ShuffleInPlace(all);

            foreach (Alternative alternative in all)
            {
                if (state.incompatibles.Contains(alternative))
                    continue;

                SelectionState clonedState = state.Clone();

                Selection selection = currentSection.ActivateSelectedAlternative(alternative, all);
                ProcessSelection(selection, clonedState);

                try
                {
                    TryAllReplacers(currentIndex, selection, sections, clonedState);
                    return;
                }
                catch (IncompatibleAlternativesException)
                {
                    continue;
                }
            }

            throw new IncompatibleAlternativesException($"{currentSection.name}: all alternatives are incompatible");
        }

        private static void ProcessSelection(Selection selection, SelectionState state)
        {
            state.selected.Add(selection.selected);
            state.doneSections.Add(selection.section);

            foreach (var incompatible in selection.selected.Incompatibles)
                state.incompatibles.Add(incompatible);
        }

        private static void TryAllReplacers(int currentIndex, Selection selection, IList<Section> sections, SelectionState state)
        {
            foreach (var alternative in selection.needReplacement)
            {
                foreach (var candidate in alternative.Replacers)
                    if (state.selected.Contains(candidate))
                        continue;

                List<Alternative> replacers = new List<Alternative>(alternative.Replacers);
                ListUtils.ShuffleInPlace(replacers);

                foreach (var candidate in replacers)
                {
                    if (state.incompatibles.Contains(candidate))
                        continue;

                    Section section = candidate.GetSection();

                    if (state.doneSections.Contains(section))
                        continue;

                    SelectionState clonedState = state.Clone();

                    Selection nestedSelection = section.SelectAlternative(candidate);
                    ProcessSelection(nestedSelection, clonedState);

                    try
                    {
                        TryAllReplacers(currentIndex, nestedSelection, sections, clonedState);

                    }
                    catch (IncompatibleAlternativesException)
                    {
                        continue;
                    }
                }

                throw new NoReplacerException($"no compatible replacer for {alternative.name}");
            }

            ContinueRecursiveSelection(currentIndex, sections, state);
        }

        private static void ContinueRecursiveSelection(int currentIndex, IList<Section> sections, SelectionState state)
        {
            int nextIndex = currentIndex + 1;

            if (nextIndex < sections.Count)
                RecursiveSelection(nextIndex, sections, state);
        }
    }
}