using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Horror.Alternatives
{
    public struct SelectionState
    {
        public HashSet<Alternative> selected;
        public HashSet<Section> doneSections;
        public HashSet<Alternative> incompatibles;

        public SelectionState Clone()
        {
            return new SelectionState()
            {
                selected = new HashSet<Alternative>(selected),
                doneSections = new HashSet<Section>(doneSections),
                incompatibles = new HashSet<Alternative>(incompatibles),
            };
        }
    }

    public class IncompatibleAlternativesException : InvalidOperationException
    {
        public IncompatibleAlternativesException(string msg) : base(msg) { }
    }

    public class NoReplacerException : IncompatibleAlternativesException
    {
        public NoReplacerException(string msg) : base(msg) { }
    }
}