using _Project.Scripts.Configs;
using _Project.Scripts.Gameplay.Cube;
using _Project.Scripts.Gameplay.Repository;
using _Project.Scripts.Infrastructure.AssetsPipeline;
using R3;
using UnityEngine;

namespace _Project.Scripts.Gameplay.Merge
{
    public class MergeSystem : IMergeSystem, IGameplayInit
    {
        private readonly IStaticDataProvider _staticDataProvider;
        private readonly ICubeRepository _cubeRepository;
        private readonly Subject<int> _merged = new();

        private GameplayConfig _gameplayConfig;

        public Observable<int> Merged => _merged;

        public MergeSystem(IStaticDataProvider staticDataProvider, ICubeRepository cubeRepository)
        {
            _staticDataProvider = staticDataProvider;
            _cubeRepository = cubeRepository;
        }

        public void Initialize() =>
            _gameplayConfig = _staticDataProvider.GetGameplayConfig();

        public void TryMerge(int fromId, int toId, Collision collision)
        {
            if (!_cubeRepository.TryGetCubeById(fromId, out CubeActor fromActor) ||
                !_cubeRepository.TryGetCubeById(toId, out CubeActor toActor))
                return;

            if (fromActor.CurrentValue.CurrentValue != toActor.CurrentValue.CurrentValue)
                return;

            if (collision.impulse.sqrMagnitude < _gameplayConfig.SqrMinImpulse)
                return;

            fromActor.Upgrade();
            toActor.Kill();

            _merged.OnNext(fromId);
        }
    }
}