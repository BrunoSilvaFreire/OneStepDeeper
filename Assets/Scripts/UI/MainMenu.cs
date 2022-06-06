using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace OSD.UI {
    public class MainMenu : MonoBehaviour {
        public int firstScene;
        public TMP_InputField joinAddress;
        public TMP_InputField hostPort;
        public void Host() {
            if (!ushort.TryParse(hostPort.text, out var port)) {
                return;
            }
            GameManager.Instance.Host(port, firstScene);
        }
        public void Join() {
            if (!TryGetAddress(out var addr, out var p)) {
                return;
            }
            GameManager.Instance.Client(addr, p, firstScene);
        }
        private bool TryGetAddress(out string addr, out ushort p) {
            addr = default;
            p = default;
            var raw = joinAddress.text;
            var components = raw.Split(':');
            if (components.Length != 2) {
                return false;
            }
            addr = components[0];
            var port = components[1];
            return ushort.TryParse(port, out p);
        }
    }
}