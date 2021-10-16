using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Horror.Alternatives
{
    public struct Selection
    {
        public readonly Section section;
        public readonly Alternative selected;
        public readonly IReadOnlyList<Alternative> needReplacement;

        public Selection(Section section, Alternative selected, IReadOnlyList<Alternative> needReplacement)
        {
            this.section = section;
            this.selected = selected;
            this.needReplacement = needReplacement;
        }
    }

    [Serializable]
    public class UnityEventSelection : UnityEvent<Selection> { }
}