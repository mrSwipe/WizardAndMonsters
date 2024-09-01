using Common;
using UnityEngine;
using Zenject;

namespace Spells.SpellsTypes
{
    [RequireComponent (typeof(SpellMovement))]
    [RequireComponent (typeof(Damage))]
    public class SpellBlue : AbstractSpell
    {
        public override SpellType SpellType => SpellType.Blue;
        
        public class Pool : MonoMemoryPool<SpellBlue>
        {
            protected override void Reinitialize(SpellBlue spell)
            {
                spell.ResetToStart();
                base.Reinitialize(spell);
            }
        }
    }
}