using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Members.JJW._01.Code
{
    public class AfterimageTrailJJW : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer source;
        [SerializeField] private SpriteRenderer[] additionalSources;
        [SerializeField] private Material afterimageMaterial;

        [Header("잔상 설정")]
        [SerializeField, Min(0.1f)] private float emissionDuration = 2f;
        [SerializeField, Min(0.01f)] private float spawnInterval = 0.05f;
        [SerializeField, Min(0.01f), InspectorName("Fade Duration")] private float lifetime = 0.3f;
        [SerializeField, Min(1)] private int poolSize = 30;
        [SerializeField, Min(0f)] private float minDistance = 0.08f;
        [SerializeField, InspectorName("Trail Gradient")] private Gradient colorOverLifetime = new Gradient();

        public float EmissionDuration => Mathf.Max(0.1f, emissionDuration);
        public SpriteRenderer SourceRenderer => source;

        private class Afterimage
        {
            public SpriteRenderer[] Renderers;
        }

        private SpriteRenderer[] _sources;
        private Afterimage[] _images;
        private Transform _poolRoot;
        private float _emissionRemaining;
        private float _spawnTimer;
        private int _nextIndex;
        private float _activeDuration;
        private float _activeSpawnInterval;
        private float _fadeDuration;
        private Vector3 _lastPosition;
        private bool _hasLastPosition;
        private bool _isPlaying;

        private void Awake()
        {
            List<SpriteRenderer> sources = new List<SpriteRenderer>();
            if (source != null) sources.Add(source);
            if (additionalSources != null)
            {
                foreach (SpriteRenderer part in additionalSources)
                {
                    if (part != null && !sources.Contains(part)) sources.Add(part);
                }
            }
            _sources = sources.ToArray();

            _poolRoot = new GameObject("AfterimagePoolJJW").transform;
            SceneManager.MoveGameObjectToScene(_poolRoot.gameObject, gameObject.scene);
            EnsurePoolCapacity(GetRequiredPoolSize());
        }

        private int GetRequiredPoolSize()
        {
            float duration = Mathf.Max(0.1f, emissionDuration);
            float interval = Mathf.Max(0.01f, spawnInterval);
            return Mathf.Max(poolSize, Mathf.CeilToInt(duration / interval) + 1);
        }

        private void EnsurePoolCapacity(int count)
        {
            int previousCount = _images == null ? 0 : _images.Length;
            if (count <= previousCount) return;

            System.Array.Resize(ref _images, count);
            for (int i = previousCount; i < count; i++)
            {
                Afterimage image = new Afterimage { Renderers = new SpriteRenderer[_sources.Length] };
                _images[i] = image;
                for (int j = 0; j < _sources.Length; j++)
                {
                    GameObject part = new GameObject($"Afterimage_{i}_{j}");
                    part.transform.SetParent(_poolRoot, false);
                    image.Renderers[j] = part.AddComponent<SpriteRenderer>();
                    image.Renderers[j].enabled = false;
                }
            }
        }

        public void PlayTrail()
        {
            if (!isActiveAndEnabled || source == null || _poolRoot == null) return;

            StopTrail();
            EnsurePoolCapacity(GetRequiredPoolSize());
            _activeDuration = Mathf.Max(0.1f, emissionDuration);
            _activeSpawnInterval = Mathf.Max(0.01f, spawnInterval);
            _fadeDuration = Mathf.Max(0.01f, lifetime);
            _emissionRemaining = _activeDuration;
            _spawnTimer = 0f;
            _isPlaying = true;
        }

        private void LateUpdate()
        {
            if (!_isPlaying || _images == null || _poolRoot == null) return;

            float deltaTime = Time.deltaTime;
            if (deltaTime <= 0f) return;

            float previousRemaining = _emissionRemaining;
            _emissionRemaining -= deltaTime;
            if (previousRemaining > 0f)
            {
                TrySpawnAfterimage(deltaTime);
                if (_emissionRemaining > 0f) return;
            }

            float progress = Mathf.Clamp01(-_emissionRemaining / _fadeDuration);
            float alpha = colorOverLifetime.Evaluate(progress).a;
            for (int i = 0; i < _nextIndex; i++)
            {
                foreach (SpriteRenderer part in _images[i].Renderers)
                {
                    if (part == null || !part.enabled) continue;
                    Color color = part.color;
                    color.a = alpha;
                    part.color = color;
                }
            }

            if (progress >= 1f) StopTrail();
        }

        private void TrySpawnAfterimage(float deltaTime)
        {
            _spawnTimer -= deltaTime;
            if (_spawnTimer > 0f || source == null) return;
            if (!source.enabled || !source.gameObject.activeInHierarchy) return;

            Vector3 position = source.transform.position;
            if (_hasLastPosition && (position - _lastPosition).sqrMagnitude < minDistance * minDistance)
                return;

            SpawnAfterimage();
            _lastPosition = position;
            _hasLastPosition = true;
            _spawnTimer = _activeSpawnInterval;
        }

        private void SpawnAfterimage()
        {
            if (_nextIndex >= _images.Length)
                EnsurePoolCapacity(Mathf.Max(_nextIndex + 1, _images.Length * 2));

            Afterimage image = _images[_nextIndex];
            float colorProgress = Mathf.Clamp01(1f - _emissionRemaining / _activeDuration);
            Color spawnColor = colorOverLifetime.Evaluate(colorProgress);
            spawnColor.a = colorOverLifetime.Evaluate(0f).a;

            for (int i = 0; i < _sources.Length; i++)
            {
                SpriteRenderer original = _sources[i];
                SpriteRenderer copy = image.Renderers[i];
                if (copy == null) continue;
                copy.enabled = original != null && original.enabled
                    && original.gameObject.activeInHierarchy && original.sprite != null;
                if (!copy.enabled) continue;

                copy.sprite = original.sprite;
                copy.sharedMaterial = afterimageMaterial != null ? afterimageMaterial : original.sharedMaterial;
                copy.flipX = original.flipX;
                copy.flipY = original.flipY;
                copy.drawMode = original.drawMode;
                copy.size = original.size;
                copy.sortingLayerID = original.sortingLayerID;
                copy.sortingOrder = original.sortingOrder - 1;
                copy.gameObject.layer = original.gameObject.layer;
                copy.transform.SetPositionAndRotation(original.transform.position, original.transform.rotation);
                copy.transform.localScale = original.transform.lossyScale;
                copy.color = spawnColor;
            }

            _nextIndex++;
        }

        public void StopTrail()
        {
            _isPlaying = false;
            _emissionRemaining = 0f;
            _hasLastPosition = false;
            if (_images == null) return;

            for (int i = 0; i < _nextIndex; i++)
            {
                foreach (SpriteRenderer part in _images[i].Renderers)
                {
                    if (part != null) part.enabled = false;
                }
            }
            _nextIndex = 0;
        }

        private void OnDisable()
        {
            StopTrail();
        }

        private void OnDestroy()
        {
            if (_poolRoot != null) Destroy(_poolRoot.gameObject);
        }
    }
}
