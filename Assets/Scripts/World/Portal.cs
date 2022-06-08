using System;
using System.Collections;
using System.Linq;
using Lunari.Entities.Network;
using Lunari.Tsuki;
using Lunari.Tsuki.Entities;
using Lunari.Tsuki.Misc;
using Sirenix.OdinInspector;
using Unity.Netcode;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace OSD.World {
    public class Portal : NetworkTrait {
        public Area area;
        public string numPlayersParam = "NumPlayers";
        public string presentParam = "Present";
        public Animator animator;
        public Animator p1Anim;
        public Animator p2Anim;
        public Cooldown postHasBothCooldown;
        public string nextLevel;
        private Coroutine _completion;

        public override void Configure(TraitDescriptor descriptor) {
            descriptor.RequiresAnimatorParameter(animator, numPlayersParam, AnimatorControllerParameterType.Int);
            descriptor.RequiresAnimatorParameter(p1Anim, presentParam, AnimatorControllerParameterType.Bool);
            descriptor.RequiresAnimatorParameter(p2Anim, presentParam, AnimatorControllerParameterType.Bool);
        }
        private void Start() {
            area.onEntitiesChanged.AddListener(OnChanged);
        }
        public IEnumerator CompletionRoutine() {
            yield return postHasBothCooldown.UseAndWait();
            GameManager.Instance.GoToLevel(nextLevel);
        }
        private bool HasFirstPlayer() {
            var m = NetworkManager.Singleton;
            return m.IsClient ? HasRemotePlayer() : HasLocalPlayer();
        }

        private bool HasSecondPlayer() {
            var m = NetworkManager.Singleton;
            return m.IsClient ? HasLocalPlayer() : HasRemotePlayer();
        }
        private bool HasLocalPlayer() {
            return area.Contains(LocalPlayer.Instance.Pawn);
        }

        private bool HasRemotePlayer() {
            return area.Entities.Any(e => e != LocalPlayer.Instance.Pawn);
        }

        private void OnChanged() {
            var hasFirst = HasFirstPlayer();
            var hasSecond = HasSecondPlayer();
            var numPlayers = (hasFirst ? 1 : 0) + (hasSecond ? 1 : 0);
            animator.SetInteger(numPlayersParam, numPlayers);
            p1Anim.SetBool(presentParam, hasFirst);
            p2Anim.SetBool(presentParam, hasSecond);
            if (IsOwner && hasFirst && hasSecond) {
                GoToNextLevel();
            }
        }
        [ShowInInspector]
        public void GoToNextLevel() {
            Coroutines.ReplaceCoroutine(ref _completion, this, CompletionRoutine());
        }
    }
#if UNITY_EDITOR
    [CustomEditor(typeof(Portal))]
    public class PortalEditor : Editor {
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
            if (GUILayout.Button("Force Next Level")) {
                ((Portal) target).GoToNextLevel();
            }
        }
    }
#endif
}