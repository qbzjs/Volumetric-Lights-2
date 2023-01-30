using System;
using UnityEngine;

namespace Dialog
{
    [Serializable]
    public enum SequencingType
    {
        Automatic = 0,
        PressButton = 1
    }

    [Serializable]
    public struct DialogStruct
    {
        [TextArea] public string Text;
        public float TextShowTime;
        public float TextHoldTime;
        public SequencingType SequencingTypeNext;
    }
}
