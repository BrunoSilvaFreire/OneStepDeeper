using System;
using Lunari.Tsuki;
using Lunari.Tsuki.Entities;
using Lunari.Tsuki.Singletons;
using OSD.Gameplay;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace OSD {
    public partial class GameManager : Singleton<GameManager> {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void EnsureHasGameManager() {
            if (Instance == null) {
                var obj = GameConfiguration.Instance.gameManagerPrefab.Clone();
                DontDestroyOnLoad(obj);
            }
        }
        private LocalPlayer _localPlayer;
        private void Awake() {
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }
        protected override void Start() {
            base.Start();
            TryAttachToNetworkManager();
        }
        private void OnDestroy() {
            TryDetachFromNetworkManager();
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
        }

        private void OnLocalPlayerConnected(ulong clientId) {
            var networkObject = _networkManager.LocalClient.PlayerObject;
            if (!networkObject.TryGetComponent<Entity>(out var entity)) {
                return;
            }
            _localPlayer = GameConfiguration.Instance.defaultPlayerPrefab.Clone();
            _localPlayer.Pawn = entity;
            entity.gameObject.name = "Local Player Character";
        }
        private void OnLocalPlayerDisconnected(ulong clientId) {
            if (_localPlayer != null) {
                _localPlayer.Pawn = null;
                Destroy(_localPlayer);
            }
        }

        private void OnRemotePlayerConnected(ulong clientId) {
            var networkObject = _networkManager.ConnectedClients[clientId].PlayerObject;
            if (!networkObject.TryGetComponent<Entity>(out var entity)) {
                return;
            }
            entity.gameObject.name = $"Remote Player {clientId} Character";

        }
        private void OnRemotePlayerDisconnected(ulong clientId) { }
    }
}