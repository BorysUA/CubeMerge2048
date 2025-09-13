using R3;
using UnityEngine;

namespace _Project.Scripts.Gameplay.Cube
{
    [RequireComponent(typeof(Rigidbody))]
    public class CubePhysics : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;

        private readonly Subject<Collision> _collisionEnter = new();

        public Observable<Collision> CollisionEnter => _collisionEnter;

        public void AddImpulse(Vector3 impulse) =>
            _rigidbody.AddForce(impulse, ForceMode.Impulse);

        public PoseSnapshot GetPose() =>
            new(_rigidbody.position, _rigidbody.rotation);

        public void ResetRotation() =>
            transform.rotation = Quaternion.identity;

        public void Teleport(Vector3 position)
        {
            _rigidbody.detectCollisions = false;
            transform.position = position;
            _rigidbody.detectCollisions = true;
        }

        public void SetKinematic(bool isKinematic) =>
            _rigidbody.isKinematic = isKinematic;

        public void MoveToX(float worldX)
        {
            Vector3 position = _rigidbody.position;
            position.x = worldX;
            _rigidbody.position = position;
        }

        public void OnCollisionEnter(Collision collision) =>
            _collisionEnter.OnNext(collision);
    }
}