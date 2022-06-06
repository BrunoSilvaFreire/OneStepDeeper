using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace OSD.UI {
    public abstract class View : MonoBehaviour {
        public const string ViewGroup = "View Stuff";

        [SerializeField]
        [HideInInspector]
        private bool shown;


        [ShowInInspector]
        [BoxGroup(ViewGroup)]
        public bool Shown {
            get => shown;
            set {
                if (value == shown) {
                    return;
                }
#if UNITY_EDITOR
                if (!EditorApplication.isPlaying) {
                    if (value) {
                        Show(true);
                    } else {
                        Hide(true);
                    }

                    return;
                }
#endif
                if (value) {
                    Show();
                } else {
                    Hide();
                }
            }
        }

        public void Show(bool immediate = false) {
            shown = true;
            if (immediate) {
                ImmediateReveal();
            } else {
                Reveal();
            }
        }

        public void Hide(bool immediate = false) {
            shown = false;
            if (immediate) {
                ImmediateConceal();
            } else {
                Conceal();
            }
        }
        public void SetShown(bool shown, bool immediate) {
            if (shown) {
                Show(immediate);
            } else {
                Hide(immediate);
            }
        }

        protected abstract void Conceal();
        protected abstract void Reveal();
        protected abstract void ImmediateConceal();
        protected abstract void ImmediateReveal();
        public abstract bool IsFullyShown();
    }
}