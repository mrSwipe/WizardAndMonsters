using UnityEngine;

namespace Common
{
    public class Damage : MonoBehaviour
    {
        [SerializeField] private int damageValue;

        public int DamageValue => damageValue;
    }
}