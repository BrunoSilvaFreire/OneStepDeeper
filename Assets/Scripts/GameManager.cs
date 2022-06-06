using System;
using System.Collections;
using Lunari.Tsuki;
using Lunari.Tsuki.Entities;
using Lunari.Tsuki.Singletons;
using OSD.Gameplay;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace OSD {
    public partial class GameManager : Singleton<GameManager> {
        public EntityEvent localPlayerConnected;
        public EntityEvent localPlayerDisconnected;

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
            localPlayerConnected.Invoke(entity);
        }
        private void OnLocalPlayerDisconnected(ulong clientId) {
            if (_localPlayer != null) {
                localPlayerDisconnected.Invoke(_localPlayer.Pawn);
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
        private IEnumerator HostRoutine(ushort port, int scene) {
            yield return SafeLoadScene(scene);
            var nm = NetworkManager.Singleton;
            var transport = nm.GetComponent<UnityTransport>();
            transport.ConnectionData.Port = port;
            nm.StartHost();
        }
        private static IEnumerator SafeLoadScene(int scene) {
            AsyncOperation op = null;
            if (SceneManager.GetActiveScene().buildIndex != scene) {
                op = SceneManager.LoadSceneAsync(scene);
            }

            if (op != null) {
                yield return op;
            }
            yield return null; // Wait one frame
        }
        public void Host(ushort port, int scene) {

            StartCoroutine(HostRoutine(port, scene));
        }
        public void Client(string addr, ushort port, int scene) {
            StartCoroutine(ClientRoutine(addr, port, scene));
        }
        private IEnumerator ClientRoutine(string addr, ushort port, int scene) {
            yield return SafeLoadScene(scene);
            var nm = NetworkManager.Singleton;
            var transport = nm.GetComponent<UnityTransport>();
            transport.ConnectionData.Address = addr;
            transport.ConnectionData.Port = port;
            nm.StartClient();
        }
    }
}