using Common;
using UnityEngine;

namespace Characters
{
    [RequireComponent (typeof(Rigidbody))]
    public class EnemyMovement : MonoBehaviour
    {
        [SerializeField] private Rigidbody rigidBody;
        
        [SerializeField] private float spawnSpeedValue;
        [SerializeField] private float movementSpeed;
        [SerializeField] private float rotationSpeed;
        
        private ITarget _target;
        private Vector3 _position;
        private Vector3 _relativePos;
        private Quaternion _rotation;
        
        
        public void Construct(ITarget target)
        {
            _target = target;
        }

        public void Terminate()
        {
            _target = null;
        }
        
        private void FixedUpdate()
        {
            if (!_target.IsAlive) return;
            
            _position = transform.position;

            if (rigidBody.isKinematic)
            {
                _relativePos = Vector3.zero - _position;
                rigidBody.MovePosition(transform.position + _relativePos * (spawnSpeedValue * Time.deltaTime));
            }
            else
            {
                _relativePos = _target.Position - _position;
                rigidBody.velocity = transform.forward * movementSpeed;
            }
            
            _rotation = Quaternion.LookRotation(_relativePos);
            rigidBody.MoveRotation(Quaternion.RotateTowards(transform.rotation, _rotation, rotationSpeed));
        }
    }
}