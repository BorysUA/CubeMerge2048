using System;
using _Project.Scripts.Configs;
using _Project.Scripts.Data;
using _Project.Scripts.Gameplay.Merge;
using _Project.Scripts.Infrastructure.Pool;
using _Project.Scripts.Infrastructure.ProgressService;
using R3;
using UnityEngine;

namespace _Project.Scripts.Gameplay.Cube
{
    public class CubeActor : IResettablePoolItem<Vector3>, IProgressWriter, IDisposable
    {
        private const int UpgradeMultiplier = 2;

        private readonly IMergeSystem _mergeSystem;
        private readonly CompositeDisposable _subscriptions = new();

        private readonly Subject<Unit> _deactivated = new();
        private readonly Subject<Unit> _activated = new();
        private readonly Subject<Unit> _upgraded = new();
        private readonly ReactiveProperty<int> _currentValue = new();

        private CubePhysics _cubePhysics;
        private CubeConfig _cubeConfig;
        private bool _isLaunched;

        public int Id { get; private set; }
        public Observable<Unit> Upgraded => _upgraded;
        public Observable<Unit> Deactivated => _deactivated;
        public Observable<Unit> Activated => _activated;

        public ReadOnlyReactiveProperty<int> CurrentValue => _currentValue;

        public CubeActor(IMergeSystem mergeSystem) =>
            _mergeSystem = mergeSystem;

        public void Initialize(int id, int value)
        {
            Id = id;
            _currentValue.Value = value;
        }

        public void Setup(CubeConfig cubeConfig, CubePhysics cubePhysics)
        {
            _cubeConfig = cubeConfig;
            _cubePhysics = cubePhysics;

            _cubePhysics.CollisionEnter
                .Subscribe(OnCollisionFromPhysics)
                .AddTo(_subscriptions);
        }

        public void Activate(Vector3 position)
        {
            _cubePhysics.Teleport(position);
            _activated.OnNext(Unit.Default);
        }

        public void Reset()
        {
            _isLaunched = false;
            _cubePhysics.ResetRotation();
            _cubePhysics.SetKinematic(true);
        }

        public void MoveToX(float worldX)
        {
            if (_isLaunched)
                return;

            _cubePhysics.MoveToX(worldX);
        }

        public void Upgrade()
        {
            _currentValue.Value *= UpgradeMultiplier;
            _cubePhysics.AddImpulse(Vector3.up * _cubeConfig.MergeEffectImpulse);
            _upgraded.OnNext(Unit.Default);
        }

        public void Launch()
        {
            if (_isLaunched)
                return;

            _isLaunched = true;
            _cubePhysics.SetKinematic(false);
            _cubePhysics.AddImpulse(Vector3.forward * _cubeConfig.LaunchImpulse);
        }

        public void Kill()
        {
            _deactivated.OnNext(Unit.Default);
        }

        public void WriteProgress(GameStateData gameStateData)
        {
            if (!_isLaunched)
                return;

            PoseSnapshot poseSnapshot = _cubePhysics.GetPose();
            gameStateData.Cubes.Add(new CubeData(poseSnapshot.Position, poseSnapshot.Rotation));
        }

        public void Dispose() =>
            _subscriptions.Dispose();

        private void OnCollisionFromPhysics(Collision collision)
        {
            if (!collision.collider.TryGetComponent(out ICubeIdentity other))
                return;

            if (Id > other.Id)
                _mergeSystem.TryMerge(Id, other.Id, collision);
        }
    }
}