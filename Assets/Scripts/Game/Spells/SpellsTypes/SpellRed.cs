using Common;
using UnityEngine;
using Zenject;

namespace Spells.SpellsTypes
{
    [RequireComponent (typeof(SpellMovement))]
    [RequireComponent (typeof(Damage))]
    public class SpellRed : AbstractSpell
    {
        public override SpellType SpellType => SpellType.Red;
        
        public class Pool : MonoMemoryPool<SpellRed>
        {
            protected override void Reinitialize(SpellRed spell)
            {
                spell.ResetToStart();
                base.Reinitialize(spell);
            }
        }
    }
}