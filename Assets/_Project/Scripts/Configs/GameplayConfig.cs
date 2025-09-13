using UnityEngine;

namespace _Project.Scripts.Configs
{
    [CreateAssetMenu(fileName = "GameplayConfig", menuName = "Configs/GameplayConfig", order = 2)]
    public class GameplayConfig : ScriptableObject
    {
        [field: Header("Spawn Area")]
        [field: SerializeField]
        [Range(0f, 1f)]
        public float SideMarginNormalized { get; private set; } = 0.8f;

        [field: SerializeField] public float MinWorldX { get; private set; } = -2f;
        [field: SerializeField] public float MaxWorldX { get; private set; } = 2f;

        [field: Header("Merge Logic")]
        [field: SerializeField]
        public float SqrMinImpulse { get; private set; } = 1f;

        [field: Header("Game Over Conditions")]
        [field: SerializeField]
        public int MaxCubesAlive { get; private set; } = 30;

        [field: SerializeField] public float NoMergeFailSeconds { get; private set; } = 10f;
    }
}