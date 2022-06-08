﻿using System;
using Lunari.Tsuki;
using Lunari.Tsuki.Entities;
using OSD.Network;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Tilemaps;
namespace OSD.Gameplay {
    [RequireComponent(typeof(TilemapRenderer), typeof(TilemapCollider2D))]
    public class TilemapColorBlindness : MonoBehaviour {
        public ColorBlindnessType colorBlindness;
        private Slot<ColorBlindness> current;
        private NetworkVariable<ColorBlindnessType>.OnValueChangedDelegate _listener;
        private new TilemapRenderer renderer;
        private new TilemapCollider2D collider;
        private void Start() {
            renderer = GetComponent<TilemapRenderer>();
            collider = GetComponent<TilemapCollider2D>();
            var gm = GameManager.Instance;
            gm.localPlayerConnected.AddListener(OnLocalPlayerConnected);
            gm.localPlayerDisconnected.AddListener(OnLocalPlayerDisconnected);
            current = new Slot<ColorBlindness>().OnChangedNotNull(v =>
            {
                current.ListenAndInvoke(v.currentType, UpdateTo);
            });
            
            var lp = LocalPlayer.Instance;
            if (lp != null) {
                OnLocalPlayerConnected(lp.Pawn);
            }
            Hide();
        }
        
        private void OnDestroy() {
            current.Clear();
        }
        
        private void OnLocalPlayerDisconnected(Entity arg0) {
            current.Clear();
        }
        
        private void OnLocalPlayerConnected(Entity arg0) {
            if (arg0.Access(out ColorBlindness cb)) {
                current.Value = cb;
            }
        }

        private void UpdateTo(ColorBlindnessType currentTypeValue) {
            if (currentTypeValue == ColorBlindnessType.None || currentTypeValue == colorBlindness) {
                Hide();
            } else {
                Show();
            }
        }
        public void Show() {
            renderer.enabled = true;
            collider.enabled = true;
        }
        public void Hide() {
            renderer.enabled = false;
            collider.enabled = false;
        }
    }
}