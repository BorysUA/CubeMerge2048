using System;
using _Project.Scripts.Data;
using _Project.Scripts.Gameplay.Cube;
using _Project.Scripts.Gameplay.Merge;
using _Project.Scripts.Gameplay.Repository;
using _Project.Scripts.Infrastructure.ProgressService;
using R3;
using Zenject;

namespace _Project.Scripts.Gameplay.Score
{
    public class ScoringSystem : IScoringSystem, IGameplayInit, IDisposable, IProgressWriter
    {
        private const int ScoreDivider = 2;

        private readonly IMergeSystem _mergeSystem;
        private readonly ICubeRepository _cubeRepository;
        private readonly CompositeDisposable _subscriptions = new();
        private readonly ReactiveProperty<int> _score = new(0);

        public ReadOnlyReactiveProperty<int> CurrentScore => _score;

        public ScoringSystem(IMergeSystem mergeSystem, ICubeRepository cubeRepository)
        {
            _mergeSystem = mergeSystem;
            _cubeRepository = cubeRepository;
        }

        public void Initialize()
        {
            _mergeSystem.Merged
                .Subscribe(UpdateScore)
                .AddTo(_subscriptions);
        }

        public void WriteProgress(GameStateData gameStateData) =>
            gameStateData.Score = _score.CurrentValue;

        public void Dispose() =>
            _subscriptions?.Dispose();

        private void UpdateScore(int id)
        {
            if (_cubeRepository.TryGetCubeById(id, out CubeActor actor))
                _score.Value += actor.CurrentValue.CurrentValue / ScoreDivider;
        }
    }
}