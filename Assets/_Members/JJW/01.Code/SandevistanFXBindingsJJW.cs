using MoreMountains.Feedbacks;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

namespace _Members.JJW._01.Code
{
    [DefaultExecutionOrder(-1000)]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(MMF_Player))]
    public class SandevistanFXBindingsJJW : MonoBehaviour
    {
        [SerializeField] private MMF_Player feedbacks;
        [SerializeField] private SandevistanAbility ability;
        [SerializeField] private AfterimageTrailJJW trail;
        [SerializeField] private SpriteRenderer body;
        [SerializeField] private SpriteRenderer backdrop;

        private void Awake()
        {
            ConnectTargets();
        }

        private void OnEnable()
        {
            ConnectTargets();
        }

        private void OnTransformParentChanged()
        {
            if (!Application.isPlaying) return;

            if (feedbacks != null && feedbacks.IsPlaying)
            {
                feedbacks.StopFeedbacks();
                feedbacks.RestoreInitialValues();
            }
            StopEffects();
            ConnectTargets();
            if (feedbacks != null && ability != null && body != null && backdrop != null)
                feedbacks.Initialization();
        }

        public bool ConnectTargets()
        {
            MMF_Player nextFeedbacks = GetComponent<MMF_Player>();
            SandevistanAbility nextAbility = GetComponentInParent<SandevistanAbility>(true);
            AfterimageTrailJJW nextTrail = nextAbility != null
                ? nextAbility.GetComponent<AfterimageTrailJJW>()
                : null;
            SpriteRenderer nextBody = nextTrail != null ? nextTrail.SourceRenderer : null;
            SpriteRenderer nextBackdrop = FindBackdrop();

            bool changed = feedbacks != nextFeedbacks || ability != nextAbility
                || trail != nextTrail || body != nextBody || backdrop != nextBackdrop;
            feedbacks = nextFeedbacks;
            ability = nextAbility;
            trail = nextTrail;
            body = nextBody;
            backdrop = nextBackdrop;

            if (feedbacks == null || feedbacks.FeedbacksList == null) return changed;

            foreach (MMF_Feedback feedback in feedbacks.FeedbacksList)
            {
                if (feedback is MMF_SpriteRenderer spriteFeedback
                    && spriteFeedback.BoundSpriteRenderer != body)
                {
                    spriteFeedback.BoundSpriteRenderer = body;
                    changed = true;
                }
                else if (feedback is MMF_MapColorJJW mapFeedback
                    && mapFeedback.TargetRenderer != backdrop)
                {
                    mapFeedback.TargetRenderer = backdrop;
                    changed = true;
                }
            }
            return changed;
        }

        private SpriteRenderer FindBackdrop()
        {
            if (backdrop != null && backdrop.gameObject.scene == gameObject.scene
                && HasEffectMaterial(backdrop))
                return backdrop;

            if (!gameObject.scene.IsValid()) return null;

            foreach (GameObject root in gameObject.scene.GetRootGameObjects())
            {
                foreach (SpriteRenderer candidate in root.GetComponentsInChildren<SpriteRenderer>(true))
                {
                    if (candidate.name == "SandevistanBackdropJJW" && HasEffectMaterial(candidate))
                        return candidate;
                }
            }
            return null;
        }

        private static bool HasEffectMaterial(SpriteRenderer renderer)
        {
            return renderer.sharedMaterial != null
                && renderer.sharedMaterial.HasProperty("_EffectAmount");
        }

        public void PlayEffects()
        {
            if (!isActiveAndEnabled) return;
            if (ability == null || trail == null)
            {
                Debug.LogWarning(".", this);
                return;
            }

            ability.Activate();
            trail.PlayTrail();
        }

        public void StopEffects()
        {
            if (ability != null) ability.Deactivate();
            if (trail != null) trail.StopTrail();
        }

        private void OnDisable()
        {
            if (Application.isPlaying) StopEffects();
        }
    }

#if UNITY_EDITOR
    [InitializeOnLoad]
    internal static class SandevistanFXBindingsJJWEditor
    {
        private static bool _refreshQueued;

        static SandevistanFXBindingsJJWEditor()
        {
            EditorApplication.hierarchyChanged += QueueRefresh;
            Undo.undoRedoPerformed += QueueRefresh;
            EditorApplication.playModeStateChanged += OnPlayModeChanged;
            QueueRefresh();
        }

        private static void OnPlayModeChanged(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.EnteredEditMode) QueueRefresh();
        }

        private static void QueueRefresh()
        {
            if (_refreshQueued) return;
            _refreshQueued = true;
            EditorApplication.delayCall += Refresh;
        }

        private static void Refresh()
        {
            _refreshQueued = false;
            if (EditorApplication.isPlayingOrWillChangePlaymode) return;
            if (EditorApplication.isCompiling || EditorApplication.isUpdating)
            {
                QueueRefresh();
                return;
            }

            foreach (SandevistanFXBindingsJJW bindings in Object.FindObjectsByType<SandevistanFXBindingsJJW>(
                         FindObjectsInactive.Include, FindObjectsSortMode.None))
            {
                if (EditorUtility.IsPersistent(bindings) || !bindings.gameObject.scene.IsValid()
                    || !bindings.gameObject.scene.isLoaded
                    || EditorSceneManager.IsPreviewScene(bindings.gameObject.scene))
                    continue;
                if (!bindings.ConnectTargets()) continue;

                MMF_Player player = bindings.GetComponent<MMF_Player>();
                EditorUtility.SetDirty(bindings);
                if (PrefabUtility.IsPartOfPrefabInstance(bindings))
                    PrefabUtility.RecordPrefabInstancePropertyModifications(bindings);
                if (player != null)
                {
                    EditorUtility.SetDirty(player);
                    if (PrefabUtility.IsPartOfPrefabInstance(player))
                        PrefabUtility.RecordPrefabInstancePropertyModifications(player);
                }
                EditorSceneManager.MarkSceneDirty(bindings.gameObject.scene);
            }
        }
    }
#endif
}
