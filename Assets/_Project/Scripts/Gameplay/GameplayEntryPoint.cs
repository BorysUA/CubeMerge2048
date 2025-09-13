using System;
using System.Collections.Generic;
using _Project.Scripts.Gameplay.Factory;
using _Project.Scripts.Gameplay.GameFlow;
using _Project.Scripts.Gameplay.InputHandlers;
using _Project.Scripts.Infrastructure.InputService;
using Cysharp.Threading.Tasks;
using R3;
using Zenject;

namespace _Project.Scripts.Gameplay
{
    public class GameplayEntryPoint : IInitializable, IDisposable
    {
        private readonly IInputService _inputService;
        private readonly CubeLauncher _cubeLauncher;
        private readonly List<IWarmUp> _warmUpServices;
        private readonly List<IGameplayInit> _gameplayInits;
        private readonly IGameOverSystem _gameOverSystem;
        private readonly CompositeDisposable _subscriptions = new();

        public GameplayEntryPoint(IInputService inputService, CubeLauncher cubeLauncher,
            List<IWarmUp> warmUpServices, List<IGameplayInit> gameplayInits, IGameOverSystem gameOverSystem)
        {
            _inputService = inputService;
            _cubeLauncher = cubeLauncher;
            _warmUpServices = warmUpServices;
            _gameplayInits = gameplayInits;
            _gameOverSystem = gameOverSystem;
        }

        public void Initialize() =>
            InternalInitialize().Forget();

        public void Dispose() =>
            _subscriptions.Dispose();

        private async UniTaskVoid InternalInitialize()
        {
            await WarmUpServices();
            InitializeServices();

            StartGameplay();
            SubscribeToGameOver();
        }

        private void StartGameplay() =>
            _inputService.Subscribe(_cubeLauncher);

        private void InitializeServices()
        {
            foreach (IGameplayInit service in _gameplayInits)
                service.Initialize();
        }

        private async UniTask WarmUpServices()
        {
            List<UniTask> tasks = new List<UniTask>();

            foreach (IWarmUp service in _warmUpServices)
            {
                UniTask task = service.WarmUpAsync();
                tasks.Add(task);
            }

            await UniTask.WhenAll(tasks);
        }

        private void SubscribeToGameOver()
        {
            _gameOverSystem.GameEnded
                .Subscribe(_ => _inputService.Unsubscribe(_cubeLauncher))
                .AddTo(_subscriptions);
        }
    }
}