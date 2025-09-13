using System;
using UnityEngine;

namespace _Project.Scripts.Configs
{
    [Serializable]
    public struct ValueChance
    {
        public int Value;
        [Min(0)] public int Weight;
    }
}