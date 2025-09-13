using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Configs
{
    [CreateAssetMenu(fileName = "ValueColorPaletteConfig", menuName = "Configs/ValueColorPalette", order = 3)]
    public class ValueColorPaletteConfig : ScriptableObject
    {
        [SerializeField] private List<ValueColorPair> _valueColors = new();
        public IReadOnlyList<ValueColorPair> ValueColors => _valueColors;
    }
}