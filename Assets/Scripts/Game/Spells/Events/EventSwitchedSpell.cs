namespace Spells.Events
{
    public readonly struct EventSwitchedSpell
    {
        public SpellType NewSpellType { get; }
        
        public EventSwitchedSpell(SpellType newSpellType)
        {
            NewSpellType = newSpellType;
        }
    }
}