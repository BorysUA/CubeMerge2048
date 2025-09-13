using _Project.Scripts.Gameplay.Cube;
using UnityEngine;

namespace _Project.Scripts.Gameplay.Factory
{
    public interface ICubeFactory
    {
        public CubeActor CreateCube(Vector3 configSpawnPosition);
    }
}