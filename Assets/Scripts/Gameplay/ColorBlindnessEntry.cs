using System;
using Lunari.Tsuki;
using UnityEngine;
namespace OSD.Gameplay {
    [CreateAssetMenu]
    public class ColorBlindnessEntry : ScriptableObject {
        public string alias;
        public Color characterColor;
        public Material material;
    }
}