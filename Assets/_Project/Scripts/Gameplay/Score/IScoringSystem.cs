using R3;

namespace _Project.Scripts.Gameplay.Score
{
    public interface IScoringSystem
    {
        public ReadOnlyReactiveProperty<int> CurrentScore { get; }
    }
}