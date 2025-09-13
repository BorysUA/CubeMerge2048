using System;
using System.Collections;
using _Project.Scripts.Configs;
using _Project.Scripts.Gameplay.Cube;
using _Project.Scripts.Gameplay.InputMapper;
using _Project.Scripts.Gameplay.Spawner;
using _Project.Scripts.Infrastructure.AssetsPipeline;
using _Project.Scripts.Infrastructure.CoroutineProvider;
using _Project.Scripts.Infrastructure.InputService;
using UnityEngine;

namespace _Project.Scripts.Gameplay.InputHandlers
{
    public class CubeLauncher : PlayerInputHandler, IGameplayInit, IDisposable
    {
        private readonly ICoroutineProvider _coroutineProvider;
        private readonly CubeSpawner _cubeSpawner;
        private readonly IStaticDataProvider _staticDataProvider;
        private readonly IInputToWorldMapper _inputToWorldMapper;

        private CubeSpawnerConfig _config;
        private CubeActor _currentCubeActor;

        private Coroutine _cooldownRoutine;
        private WaitForSeconds _waitRespawnDelay;

        private LaunchPhase _phase = LaunchPhase.Idle;

        public CubeLauncher(ICoroutineProvider coroutineProvider,
            IStaticDataProvider staticDataProvider, IInputToWorldMapper inputToWorldMapper, CubeSpawner cubeSpawner)
        {
            _coroutineProvider = coroutineProvider;
            _staticDataProvider = staticDataProvider;
            _inputToWorldMapper = inputToWorldMapper;
            _cubeSpawner = cubeSpawner;
        }

        public void Initialize()
        {
            _config = _staticDataProvider.GetCubeSpawnConfig();
            _waitRespawnDelay = new WaitForSeconds(_config.SpawnDelay);
            SpawnCube();
        }

        public override void OnTouchMoved(Vector2 position)
        {
            if (_phase != LaunchPhase.Holding)
                return;

            float worldX = _inputToWorldMapper.MapScreenToWorldX(position);
            _currentCubeActor.MoveToX(worldX);
        }

        public override void OnTouchEnded()
        {
            if (_phase == LaunchPhase.Holding)
                ConfirmLaunch();
        }

        public void Dispose() =>
            _coroutineProvider.TerminateCoroutine(_cooldownRoutine);

        private void SpawnCube()
        {
            _currentCubeActor = _cubeSpawner.Spawn(_config.SpawnPosition, _config.WeightedValueChance);
            _phase = LaunchPhase.Holding;
        }

        private void EnterCooldown()
        {
            _phase = LaunchPhase.Cooldown;
            _currentCubeActor = null;

            _cooldownRoutine = _coroutineProvider.ExecuteCoroutine(CooldownThenRespawn());
        }

        private IEnumerator CooldownThenRespawn()
        {
            yield return _waitRespawnDelay;
            _cooldownRoutine = null;
            SpawnCube();
        }

        private void ConfirmLaunch()
        {
            if (_phase != LaunchPhase.Holding)
                return;

            _currentCubeActor.Launch();
            EnterCooldown();
        }
    }
}