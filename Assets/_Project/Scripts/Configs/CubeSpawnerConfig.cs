using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Configs
{
    [CreateAssetMenu(fileName = "CubeSpawnerConfig", menuName = "Configs/CubeSpawner", order = 1)]
    public class CubeSpawnerConfig : ScriptableObject
    {
        [SerializeField] private List<ValueChance> _weightedValueChance = new();
        [field: SerializeField] public Vector3 SpawnPosition { get; private set; } = new(0f, 0.5f, -4f);
        [field: SerializeField] [Min(0f)] public float SpawnDelay { get; private set; } = 1f;
        public IReadOnlyList<ValueChance> WeightedValueChance => _weightedValueChance;
    }
}