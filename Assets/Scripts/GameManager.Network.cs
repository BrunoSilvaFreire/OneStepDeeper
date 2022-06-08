using System;
using Lunari.Tsuki;
using Sirenix.OdinInspector;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace OSD {
    public partial class GameManager {
        public NetworkManager networkManagerPrefab;
        [ReadOnly]
        private NetworkManager _networkManager;
        private void TryAttachToNetworkManager() {
            CreateNetworkManager();
        }
        private void TryDetachFromNetworkManager() {
            if (_networkManager != null) {
                return;
            }
            Debug.Log("Detaching from NetworkManager.");
            _networkManager.OnClientConnectedCallback -= OnClientConnected;
            _networkManager.OnClientDisconnectCallback -= OnClientDisconnected;
        }
        private void CreateNetworkManager() {
            if (_networkManager != null) {
                return;
            }
            Debug.Log("Creating NetworkManager from GameManager.");
            _networkManager = networkManagerPrefab.Clone();
            _networkManager.OnClientConnectedCallback += OnClientConnected;
            _networkManager.OnClientDisconnectCallback += OnClientDisconnected;
        }

        private void OnSceneUnloaded(Scene arg0) {
            if (_networkManager == null) {
                Debug.LogWarning("So, um, a scene was unloaded and we can't find the network manager, this might be a problem.");
            }

        }
        private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1) { }
        private void OnClientDisconnected(ulong obj) {
            Debug.Log($"Client {obj} has disconnected");
            if (obj == _networkManager.LocalClientId) {
                OnLocalPlayerDisconnected(obj);
            } else {
                OnRemotePlayerDisconnected(obj);
            }
        }
        private void OnClientConnected(ulong obj) {
            Debug.Log($"Client {obj} has connected");
            if (obj == _networkManager.LocalClientId) {
                OnLocalPlayerConnected(obj);
            } else {
                OnRemotePlayerConnected(obj);
            }
        }
    }
}