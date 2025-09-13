using System.Collections.Generic;
using _Project.Scripts.Constants;
using R3;
using TMPro;
using UnityEngine;

namespace _Project.Scripts.Gameplay.Cube
{
    public class CubeView : MonoBehaviour
    {
        private static readonly int ColorIdBase = Shader.PropertyToID("_BaseColor");

        [SerializeField] private Renderer _renderer;
        [SerializeField] private TextMeshPro[] _cubeFacesText = new TextMeshPro[6];

        private IReadOnlyDictionary<int, Color32> _colorsPalette;
        private MaterialPropertyBlock _materialProperty;
        private CubeActor _actor;

        public int Id => _actor.Id;

        private void Awake()
        {
            _materialProperty = new MaterialPropertyBlock();
            _renderer.GetPropertyBlock(_materialProperty);
        }

        public void Bind(CubeActor actor, IReadOnlyDictionary<int, Color32> colorsPalette)
        {
            _actor = actor;
            _colorsPalette = colorsPalette;

            actor.CurrentValue
                .Skip(1)
                .Subscribe(UpdateCubeText)
                .AddTo(this);

            actor.Activated
                .Subscribe(_ => Activate())
                .AddTo(this);

            actor.Deactivated
                .Subscribe(_ => Deactivate())
                .AddTo(this);

            actor.Upgraded
                .Subscribe(_ => PlayVFX())
                .AddTo(this);
        }

        private void PlayVFX()
        {
        }

        private void UpdateCubeText(int value)
        {
            foreach (TextMeshPro text in _cubeFacesText)
                text.SetText("{0}", value);

            _renderer.GetPropertyBlock(_materialProperty);

            if (!_colorsPalette.TryGetValue(value, out Color32 color))
                color = Color32Defaults.Magenta;

            _materialProperty.SetColor(ColorIdBase, color);
            _renderer.SetPropertyBlock(_materialProperty);
        }

        private void Activate() =>
            gameObject.SetActive(true);

        private void Deactivate() =>
            gameObject.SetActive(false);
    }
}