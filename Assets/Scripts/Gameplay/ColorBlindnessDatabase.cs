using System.Collections.Generic;
using Lunari.Tsuki.Singletons;
using UnityEngine;
namespace OSD.Gameplay {
    [CreateAssetMenu]
    public class ColorBlindnessDatabase : ScriptableSingleton<ColorBlindnessDatabase> {
        public List<ColorBlindnessType> colorBlindnessTypes;
    }
}