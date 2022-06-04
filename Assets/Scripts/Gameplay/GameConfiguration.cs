using Lunari.Tsuki.Entities;
using Lunari.Tsuki.Singletons;
using Unity.Netcode;
using UnityEngine;
namespace OSD.Gameplay {
    [CreateAssetMenu]
    public class GameConfiguration : ScriptableSingleton<GameConfiguration> {
        public Entity defaultPawnPrefab;
        public LocalPlayer defaultPlayerPrefab;
        public GameManager gameManagerPrefab;
    }
}