using Common.Contracts;
using UnityEngine;
using Zenject;

namespace Characters
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private CharacterController characterController;
        
        [SerializeField] private float movementSpeed;
        [SerializeField] private float rotationSpeed;

        [Inject] private IGameManager _gameManager;

        private float _verticalInput;
        private float _horizontalInput;
        private float _verticalVelocity;
        private Vector3 _movement;
        private Vector3 _motion;

        private void Update()
        {
            if (!_gameManager.IsGameActive) return;
                
            _verticalInput = Input.GetAxis("Vertical");
            if (characterController.isGrounded)
            {
                _verticalVelocity = 0f;
            }
            else
            {
                _verticalVelocity -= 1f;
            }
            
            _movement = new Vector3(0f, _verticalVelocity, _verticalInput);
            _horizontalInput = Input.GetAxis("Horizontal");
            
            Move(_movement);
            Rotate(_horizontalInput);
        }

        private void Move(Vector3 movement)
        {
            _motion = transform.TransformDirection(movement) * movementSpeed;
            
            characterController.Move(_motion);
        }

        private void Rotate(float angle)
        {
            transform.Rotate(0f, angle * rotationSpeed, 0f);
        }
    }
}