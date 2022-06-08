using System;
using System.Collections.Generic;
using Lunari.Tsuki.Entities;
using UnityEngine;
using UnityEngine.Events;
namespace OSD.World {
    public class Area : MonoBehaviour {

        public HashSet<Entity> Entities {
            get;
            private set;
        }

        public UnityEvent onEntitiesChanged;
        private void Awake() {
            Entities = new HashSet<Entity>();
        }
        private void OnTriggerEnter2D(Collider2D col) {
            if (!col.FindEntity(out var entity)) {
                return;
            }
            Entities.Add(entity);
            onEntitiesChanged.Invoke();
        }
        private void OnTriggerExit2D(Collider2D other) {
            if (!other.FindEntity(out var entity)) {
                return;
            }
            Entities.Remove(entity);
            onEntitiesChanged.Invoke();
        }

        public bool Contains(Entity pawn) {
            return Entities.Contains(pawn);
        }
    }
}