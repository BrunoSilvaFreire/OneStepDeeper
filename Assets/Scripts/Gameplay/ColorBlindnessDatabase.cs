using System;
using System.Collections.Generic;
using Lunari.Tsuki;
using Lunari.Tsuki.Singletons;
using UnityEngine;
namespace OSD.Gameplay {
    [Serializable]
    public class ColorBlindnessDictionary : SerializableDictionary<ColorBlindnessType, ColorBlindnessEntry> { }
    [CreateAssetMenu]
    public class ColorBlindnessDatabase : ScriptableSingleton<ColorBlindnessDatabase> {
        public ColorBlindnessDictionary entries;
    }
}