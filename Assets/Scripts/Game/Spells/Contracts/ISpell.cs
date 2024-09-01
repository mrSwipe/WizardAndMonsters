using UnityEngine;

namespace Spells.Contracts
{
    public interface ISpell
    {
        SpellType SpellType { get; }
        
        void Fire(Transform firePoint, float lifeTimeSec);
    }
}