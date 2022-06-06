using System;
using System.Collections;
using Lunari.Tsuki;
using UnityEngine;
namespace OSD.UI {
    public class MenuController : MonoBehaviour {
        public View[] views;
        [SerializeField]
        private Slot<View> _currentView;
        public int initialView;
        private Coroutine _coroutine;
        public float transitionDelay;
        private void Start() {
            _currentView = new Slot<View>().OnChanged(OnChanged);
            for (var i = 0; i < views.Length; i++) {
                var view = views[i];
                if (i == initialView) {
                    continue;
                }
                view.Hide();
            }
            _currentView.Value = views[initialView];
        }
        private IEnumerator Swap(View old, View now) {
            var hadOld = old != null;
            if (hadOld) {
                old.Hide();
            }
            var hasNew = now != null;
            if (!hasNew) {
                yield break;
            }
            if (hadOld) {
                yield return new WaitForSeconds(transitionDelay);
            }
            now.Show();
        }
        private void OnChanged(View old, View now) {
            Coroutines.ReplaceCoroutine(ref _coroutine, this, Swap(old, now));
        }
        public void SetSelected(View view) {
            _currentView.Value = view;
        }

    }
}