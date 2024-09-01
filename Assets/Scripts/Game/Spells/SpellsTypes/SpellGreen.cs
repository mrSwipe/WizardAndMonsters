using Common;
using UnityEngine;
using Zenject;

namespace Spells.SpellsTypes
{
    [RequireComponent (typeof(SpellMovement))]
    [RequireComponent (typeof(Damage))]
    public class SpellGreen : AbstractSpell
    {
        public override SpellType SpellType => SpellType.Green;
        
        public class Pool : MonoMemoryPool<SpellGreen>
        {
            protected override void Reinitialize(SpellGreen spell)
            {
                spell.ResetToStart();
                base.Reinitialize(spell);
            }
        }
    }
}