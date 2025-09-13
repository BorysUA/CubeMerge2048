using UnityEngine;

namespace _Project.Scripts.Gameplay.Cube
{
    public readonly struct PoseSnapshot
    {
        public readonly Vector3 Position;
        public readonly Quaternion Rotation;

        public PoseSnapshot(Vector3 position, Quaternion rotation)
        {
            Position = position;
            Rotation = rotation;
        }
    }
}