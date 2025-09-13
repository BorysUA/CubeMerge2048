using UnityEngine;

namespace _Project.Scripts.Infrastructure.InputService
{
    public class InputHandler
    {
        public virtual void OnTouchStarted(Vector2 inputPoint)
        {
        }

        public virtual void OnTouchMoved(Vector2 position)
        {
        }

        public virtual void OnTouchEnded()
        {
        }
    }
}