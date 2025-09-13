using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using _Project.Scripts.Gameplay.Cube;
using ObservableCollections;
using R3;

namespace _Project.Scripts.Gameplay.Repository
{
    public class CubeRepository : ICubeRepository
    {
        private readonly ObservableDictionary<int, CubeActor> _cubeActor = new();
        private readonly WeakReference<CubeRepository> _selfWeak;

        public ReadOnlyReactiveProperty<int> Count { get; }

        public CubeRepository()
        {
            _selfWeak = new WeakReference<CubeRepository>(this);
            Count = _cubeActor.ObserveCountChanged().ToReadOnlyReactiveProperty();
        }

        public bool TryGetCubeById(int id, out CubeActor actor) =>
            _cubeActor.TryGetValue(id, out actor);

        public void Register(CubeActor cubeActor)
        {
            int id = cubeActor.Id;

            if (_cubeActor.TryAdd(id, cubeActor))
                BindAutoUnregister(cubeActor, id);
        }

        public void Unregister(int id) =>
            _cubeActor.Remove(id);

        private void BindAutoUnregister(CubeActor cubeActor, int id)
        {
            WeakReference<CubeRepository> weak = _selfWeak;

            cubeActor.Deactivated
                .Take(1)
                .Subscribe(_ =>
                {
                    if (weak.TryGetTarget(out CubeRepository repository))
                        repository.Unregister(id);
                });
        }
    }
}