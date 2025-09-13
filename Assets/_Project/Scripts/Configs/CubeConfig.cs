using UnityEngine;

namespace _Project.Scripts.Configs
{
    [CreateAssetMenu(fileName = "CubeConfig", menuName = "Configs/Cube", order = 0)]
    public class CubeConfig : ScriptableObject
    {
        [field: SerializeField] [Min(0f)] public float LaunchImpulse { get; private set; } = 10f;
        [field: SerializeField] [Min(0f)] public float MergeEffectImpulse { get; private set; } = 5f;
    }
}