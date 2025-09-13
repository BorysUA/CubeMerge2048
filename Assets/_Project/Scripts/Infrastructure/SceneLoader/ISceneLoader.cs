using System;

namespace _Project.Scripts.Infrastructure.SceneLoader
{
    public interface ISceneLoader
    {
        void LoadScene(string sceneName, Action completed = null);
    }
}