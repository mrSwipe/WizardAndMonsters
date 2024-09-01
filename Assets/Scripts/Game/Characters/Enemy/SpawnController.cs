using UnityEngine;

namespace Characters
{
    public class SpawnController : MonoBehaviour
    {
        [SerializeField] private Rigidbody rigidBody;
        [SerializeField] private Collider[] colliders;

        public void SetKinematicAndTriggers(bool v)
        {
            if (colliders is null or {Length: 0})
            {
                return;
            }
            
            rigidBody.isKinematic = v;
            
            foreach (var col in colliders)
            {
                col.isTrigger = v;
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("SpawnEnemies"))
            {
                SetKinematicAndTriggers(false);
            }
        }
    }
}