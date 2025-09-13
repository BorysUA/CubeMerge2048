using System.Collections.Generic;
using _Project.Scripts.Configs;
using _Project.Scripts.Constants;
using _Project.Scripts.Gameplay.Cube;
using _Project.Scripts.Infrastructure.AssetsPipeline;
using _Project.Scripts.Infrastructure.Pool;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Gameplay.Factory
{
    public class CubeFactory : ICubeFactory, IWarmUp
    {
        private const string RootName = "CubesRoot";

        private readonly IAssetProvider _assetProvider;
        private readonly IInstantiator _instantiator;
        private readonly IStaticDataProvider _staticDataProvider;

        private readonly ObjectPool<CubeActor, Vector3> _cubePool = new();

        private GameObject _cubePrefab;
        private Transform _cubesRoot;

        public CubeFactory(IAssetProvider assetProvider, IInstantiator instantiator,
            IStaticDataProvider staticDataProvider)
        {
            _assetProvider = assetProvider;
            _instantiator = instantiator;
            _staticDataProvider = staticDataProvider;
        }

        public async UniTask WarmUpAsync()
        {
            CreateRoot();
            _cubePrefab = await _assetProvider.LoadFromResourcesAsync<GameObject>(AssetPath.CubePrefab);
        }

        public CubeActor CreateCube(Vector3 spawnPosition)
        {
            if (_cubePool.TryGet(spawnPosition, out CubeActor cubeActor))
                return cubeActor;

            CubeConfig cubeConfig = _staticDataProvider.GetCubeConfig();

            CubeView cubeView = _instantiator.InstantiatePrefabForComponent<CubeView>(_cubePrefab, spawnPosition,
                Quaternion.identity, _cubesRoot);
            CubePhysics cubePhysics = cubeView.GetComponent<CubePhysics>();

            cubeActor = _instantiator.Instantiate<CubeActor>();
            cubeActor.Setup(cubeConfig, cubePhysics);

            IReadOnlyDictionary<int, Color32> colorsPalette = _staticDataProvider.GetCubeColorsPalette();
            cubeView.Bind(cubeActor, colorsPalette);

            _cubePool.Add(cubeActor);

            return cubeActor;
        }

        private void CreateRoot() =>
            _cubesRoot = new GameObject(RootName).transform;
    }
}