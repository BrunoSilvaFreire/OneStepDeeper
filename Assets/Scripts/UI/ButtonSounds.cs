using System;
using Lunari.Tsuki;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace OSD.UI {
    [RequireComponent(typeof(EventTrigger))]
    public class ButtonSounds : MonoBehaviour {
        public AudioClip hover, click;
        private EventTrigger _trigger;
        public AudioSource source;
        private void Start() {
            _trigger = GetComponent<EventTrigger>();
            AddListener(EventTriggerType.Submit, OnSubmit);
            AddListener(EventTriggerType.PointerEnter, OnHover);
        }
        private void OnSubmit() {
            source.PlayOneShot(click);
        }
        private void OnHover() {
            source.PlayOneShot(hover);
        }
        private void AddListener(EventTriggerType type, UnityAction func) {
            var entry = _trigger.triggers.FirstOrAdd(
                e => e.eventID == type,
                () => new EventTrigger.Entry {
                    eventID = type
                }
            );
            entry.callback.AddListener(_ => func());
        }

    }
}