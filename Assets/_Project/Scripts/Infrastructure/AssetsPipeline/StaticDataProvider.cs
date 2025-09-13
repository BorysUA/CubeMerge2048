using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Configs;
using _Project.Scripts.Constants;
using _Project.Scripts.Gameplay.Factory;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Scripts.Infrastructure.AssetsPipeline
{
    public class StaticDataProvider : IStaticDataProvider, IWarmUp
    {
        private readonly IAssetProvider _assetProvider;

        private GameplayConfig _gameplayConfig;
        private CubeSpawnerConfig _cubeSpawnerConfig;
        private CubeConfig _cubeConfig;
        private Dictionary<int, Color32> _cubeColors;

        public StaticDataProvider(IAssetProvider assetProvider) =>
            _assetProvider = assetProvider;

        public async UniTask WarmUpAsync()
        {
            _cubeSpawnerConfig =
                await _assetProvider.LoadFromResourcesAsync<CubeSpawnerConfig>(StaticDataPath.CubeSpawner);
            _cubeConfig = await _assetProvider.LoadFromResourcesAsync<CubeConfig>(StaticDataPath.CubeConfig);
            _gameplayConfig =
                await _assetProvider.LoadFromResourcesAsync<GameplayConfig>(StaticDataPath.GameplayConfig);

            ValueColorPaletteConfig colorPaletteConfig =
                await _assetProvider.LoadFromResourcesAsync<ValueColorPaletteConfig>(StaticDataPath.ValueColorPalette);

            _cubeColors = colorPaletteConfig.ValueColors.ToDictionary(x => x.Value, x => x.Color);
        }

        public GameplayConfig GetGameplayConfig() =>
            _gameplayConfig;

        public CubeSpawnerConfig GetCubeSpawnConfig() =>
            _cubeSpawnerConfig;

        public CubeConfig GetCubeConfig() =>
            _cubeConfig;

        public IReadOnlyDictionary<int, Color32> GetCubeColorsPalette()
            => _cubeColors;
    }
}