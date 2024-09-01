using UnityEngine;

namespace Spells
{
    public class SpellMovement : MonoBehaviour
    {
        [SerializeField] private Rigidbody rigidBody;
        [SerializeField] private float speedValue;
        
        public bool IsMoving { get; private set; }
        
        public void Fire(Transform firePoint)
        {
            transform.position = firePoint.position;
            transform.rotation = firePoint.rotation;
            
            rigidBody.AddForce(firePoint.forward * speedValue, ForceMode.Impulse);
            
            IsMoving = true;
        }

        public void Terminate()
        {
            rigidBody.velocity = Vector3.zero;
            rigidBody.angularVelocity = Vector3.zero; 
            IsMoving = false;
        }
    }
}