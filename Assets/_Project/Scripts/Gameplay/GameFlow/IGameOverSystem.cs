using R3;

namespace _Project.Scripts.Gameplay.GameFlow
{
    public interface IGameOverSystem
    {
        Observable<Unit> GameEnded { get; }
        ReadOnlyReactiveProperty<float> TimeLeft { get; }
        ReadOnlyReactiveProperty<int> CubesLeft { get; }
    }
}