using System;
using Lunari.Tsuki;
using UnityEngine;
namespace OSD.Gameplay {
    [Serializable]
    public class ColorVisibility : SerializableDictionary<ColorType, float> { }
    [CreateAssetMenu]
    public class ColorBlindnessType : ScriptableObject {
        public string alias;
        public ColorVisibility visibility;
    }
}