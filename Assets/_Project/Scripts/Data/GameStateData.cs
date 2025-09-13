using System;
using System.Collections.Generic;

namespace _Project.Scripts.Data
{
    [Serializable]
    public class GameStateData
    {
        public int Score;
        public float TimeLeft;
        public List<CubeData> Cubes = new();
    }
}