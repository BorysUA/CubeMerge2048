using System.Collections.Generic;
using _Project.Scripts.Configs;
using UnityEngine;

namespace _Project.Scripts.Infrastructure.AssetsPipeline
{
    public interface IStaticDataProvider
    {
        public CubeSpawnerConfig GetCubeSpawnConfig();
        public CubeConfig GetCubeConfig();
        GameplayConfig GetGameplayConfig();
        IReadOnlyDictionary<int, Color32> GetCubeColorsPalette();
    }
}