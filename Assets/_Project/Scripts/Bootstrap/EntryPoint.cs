using _Project.Scripts.Constants;
using _Project.Scripts.Infrastructure.SceneLoader;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Bootstrap
{
    public class EntryPoint : IInitializable
    {
        private readonly ISceneLoader _sceneLoader;

        public EntryPoint(ISceneLoader sceneLoader) =>
            _sceneLoader = sceneLoader;

        public void Initialize()
        {
            SetupGame();
            _sceneLoader.LoadScene(SceneName.Gameplay);
        }

        private void SetupGame()
        {
            Application.targetFrameRate = 60;
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
        }
    }
}