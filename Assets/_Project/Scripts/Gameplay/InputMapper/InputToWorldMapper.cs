using _Project.Scripts.Configs;
using _Project.Scripts.Infrastructure.AssetsPipeline;
using UnityEngine;

namespace _Project.Scripts.Gameplay.InputMapper
{
    public class InputToWorldMapper : IInputToWorldMapper, IGameplayInit
    {
        private readonly IStaticDataProvider _staticDataProvider;

        private GameplayConfig _gameplayConfig;

        public InputToWorldMapper(IStaticDataProvider staticDataProvider) =>
            _staticDataProvider = staticDataProvider;

        public void Initialize() =>
            _gameplayConfig = _staticDataProvider.GetGameplayConfig();

        public float MapScreenToWorldX(Vector2 screenPosition)
        {
            float viewportX = Mathf.Clamp01(screenPosition.x / Screen.width);
            float factor = Mathf.InverseLerp(1f - _gameplayConfig.SideMarginNormalized,
                _gameplayConfig.SideMarginNormalized, viewportX);

            return Mathf.Lerp(_gameplayConfig.MinWorldX, _gameplayConfig.MaxWorldX, factor);
        }
    }
}