using _Project.Scripts.Gameplay.Factory;
using _Project.Scripts.Gameplay.GameFlow;
using _Project.Scripts.Gameplay.InputHandlers;
using _Project.Scripts.Gameplay.InputMapper;
using _Project.Scripts.Gameplay.Merge;
using _Project.Scripts.Gameplay.Repository;
using _Project.Scripts.Gameplay.Score;
using _Project.Scripts.Gameplay.Spawner;
using _Project.Scripts.Gameplay.UI;
using _Project.Scripts.Infrastructure.AssetsPipeline;
using _Project.Scripts.Infrastructure.InputService;
using _Project.Scripts.Infrastructure.LogService;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Gameplay
{
    public class GameplayInstaller : MonoInstaller
    {
        [SerializeField] private HUD _hud;

        public override void InstallBindings()
        {
            BindEntryPoint();
            BindInput();
            BindServices();
            BindFactories();
            BindInputHandlers();
            BindRepositories();
            BindSystems();
            BindSpawners();
            BindUI();
        }

        private void BindSpawners()
        {
            Container.Bind<CubeSpawner>().AsSingle();
        }

        private void BindSystems()
        {
            Container.BindInterfacesTo<ScoringSystem>().AsSingle();
            Container.BindInterfacesTo<GameOverSystem>().AsSingle();
        }

        private void BindUI()
        {
            Container.BindInterfacesTo<HUD>().FromInstance(_hud).AsSingle();
        }

        private void BindInputHandlers() =>
            Container.BindInterfacesAndSelfTo<CubeLauncher>().AsSingle();

        private void BindRepositories() =>
            Container.BindInterfacesTo<CubeRepository>().AsSingle();

        private void BindInput()
        {
            Container.BindInterfacesTo<InputService>().AsSingle();
            Container.Bind<TapDetector>().AsSingle();
            Container.BindInterfacesTo<InputToWorldMapper>().AsSingle();
        }

        private void BindEntryPoint() =>
            Container.BindInterfacesAndSelfTo<GameplayEntryPoint>().AsSingle();

        private void BindServices()
        {
            Container.BindInterfacesTo<AssetProvider>().AsSingle();
            Container.BindInterfacesTo<StaticDataProvider>().AsSingle();
            Container.BindInterfacesTo<LogService>().AsSingle();
            Container.BindInterfacesTo<MergeSystem>().AsSingle();
        }

        private void BindFactories()
        {
            Container.BindInterfacesTo<CubeFactory>().AsSingle();
        }
    }
}