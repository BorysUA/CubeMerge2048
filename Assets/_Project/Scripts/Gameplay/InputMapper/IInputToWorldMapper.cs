using UnityEngine;

namespace _Project.Scripts.Gameplay.InputMapper
{
    public interface IInputToWorldMapper
    {
        public float MapScreenToWorldX(Vector2 screenPosition);
    }
}