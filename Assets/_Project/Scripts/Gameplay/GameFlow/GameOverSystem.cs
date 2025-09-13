using System;
using _Project.Scripts.Configs;
using _Project.Scripts.Data;
using _Project.Scripts.Gameplay.Merge;
using _Project.Scripts.Gameplay.Repository;
using _Project.Scripts.Infrastructure.AssetsPipeline;
using _Project.Scripts.Infrastructure.ProgressService;
using _Project.Scripts.Infrastructure.TimeCounter;
using R3;

namespace _Project.Scripts.Gameplay.GameFlow
{
    public class GameOverSystem : IGameOverSystem, IGameplayInit, IProgressWriter, IDisposable
    {
        private readonly ICubeRepository _cubeRepository;
        private readonly IStaticDataProvider _staticDataProvider;
        private readonly IMergeSystem _mergeSystem;
        private readonly ITimerFactory _timerFactory;
        private readonly CompositeDisposable _subscriptions = new();
        private readonly Subject<Unit> _gameEnded = new();

        private int _maxCubesAlive;
        private float _noMergeFailSeconds;

        private ITimer _noMergeTimer;

        public Observable<Unit> GameEnded => _gameEnded;
        public ReadOnlyReactiveProperty<int> CubesLeft { get; private set; }
        public ReadOnlyReactiveProperty<float> TimeLeft { get; private set; }

        public GameOverSystem(ICubeRepository cubeRepository, IStaticDataProvider staticDataProvider,
            IMergeSystem mergeSystem, ITimerFactory timerFactory)
        {
            _cubeRepository = cubeRepository;
            _staticDataProvider = staticDataProvider;
            _mergeSystem = mergeSystem;
            _timerFactory = timerFactory;
        }

        public void Initialize()
        {
            ConfigureGameoverRules();
            SetupFailConditions();
            ExposeGameoverState();
        }

        public void WriteProgress(GameStateData gameStateData) =>
            gameStateData.TimeLeft = _noMergeTimer.ElapsedSeconds.CurrentValue;

        public void Dispose()
        {
            _subscriptions.Dispose();
            _timerFactory.DisposeTimer(_noMergeTimer);
        }

        private void EndGame() =>
            _gameEnded.OnNext(Unit.Default);

        private void ConfigureGameoverRules()
        {
            GameplayConfig gameplayConfig = _staticDataProvider.GetGameplayConfig();

            _maxCubesAlive = gameplayConfig.MaxCubesAlive;
            _noMergeFailSeconds = gameplayConfig.NoMergeFailSeconds;

            _noMergeTimer = _timerFactory.Create(autoStart: true);
        }

        private void ExposeGameoverState()
        {
            CubesLeft = _cubeRepository.Count
                .Select(count => Math.Max(0, _maxCubesAlive - count))
                .ToReadOnlyReactiveProperty();

            TimeLeft = _noMergeTimer.ElapsedSeconds
                .Select(seconds => Math.Max(0f, _noMergeFailSeconds - seconds))
                .ToReadOnlyReactiveProperty();
        }

        private void SetupFailConditions()
        {
            _mergeSystem.Merged
                .Subscribe(_ =>
                {
                    _noMergeTimer.Reset();
                    _noMergeTimer.Start();
                })
                .AddTo(_subscriptions);

            _noMergeTimer.ElapsedSeconds
                .Where(seconds => seconds >= _noMergeFailSeconds)
                .Take(1)
                .Subscribe(_ => EndGame())
                .AddTo(_subscriptions);

            _cubeRepository.Count
                .Where(count => count > _maxCubesAlive)
                .Take(1)
                .Subscribe(_ => EndGame())
                .AddTo(_subscriptions);
        }
    }
}