using System;
using UnityEngine;

namespace _Project.Scripts.Data
{
    [Serializable]
    public class CubeData
    {
        public Vector3 Position;
        public Quaternion Rotation;

        public CubeData(Vector3 position, Quaternion rotation)
        {
            Position = position;
            Rotation = rotation;
        }
    }
}