using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Project.Scripts.Infrastructure.SceneLoader
{
    public class SceneLoader : ISceneLoader
    {
        public void LoadScene(string sceneName, Action completed = null)
        {
            AsyncOperation loadSceneOperation = SceneManager.LoadSceneAsync(sceneName);
            loadSceneOperation!.allowSceneActivation = true;
            loadSceneOperation.completed += _ => { completed?.Invoke(); };
        }
    }
}