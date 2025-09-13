using _Project.Scripts.Gameplay.GameFlow;
using _Project.Scripts.Gameplay.Score;
using R3;
using TMPro;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Gameplay.UI
{
    public class HUD : MonoBehaviour, IGameplayInit
    {
        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private TextMeshProUGUI _timeLeftText;
        [SerializeField] private TextMeshProUGUI _cubesLeftText;

        private IScoringSystem _scoringSystem;
        private IGameOverSystem _gameOverSystem;

        [Inject]
        public void Construct(IScoringSystem scoringSystem, IGameOverSystem gameOverSystem)
        {
            _scoringSystem = scoringSystem;
            _gameOverSystem = gameOverSystem;
        }

        public void Initialize()
        {
            _scoringSystem.CurrentScore
                .Subscribe(newScore => _scoreText.SetText("{0}", newScore))
                .AddTo(this);

            _gameOverSystem.TimeLeft
                .Subscribe(secondsLeft =>
                {
                    int clamped = Mathf.Max(0, (int)secondsLeft);
                    int minutes = clamped / 60;
                    int seconds = clamped % 60;
                    _timeLeftText.SetText("{0:00}:{1:00}", minutes, seconds);
                })
                .AddTo(this);

            _gameOverSystem.CubesLeft
                .Subscribe(left => _cubesLeftText.SetText("{0}", left))
                .AddTo(this);
        }
    }
}