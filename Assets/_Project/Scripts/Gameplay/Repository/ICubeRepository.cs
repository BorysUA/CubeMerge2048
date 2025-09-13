using _Project.Scripts.Gameplay.Cube;
using R3;

namespace _Project.Scripts.Gameplay.Repository
{
    public interface ICubeRepository
    {
        bool TryGetCubeById(int id, out CubeActor actor);
        void Register(CubeActor cubeActor);
        void Unregister(int id);
        ReadOnlyReactiveProperty<int> Count { get; }
    }
}