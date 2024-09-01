namespace Spells.Contracts
{
    public interface ISpellsManager
    {
        void SwitchedPrevSpell();
        void SwitchedNextSpell();
        void FireCurrentSpell();
        void RemoveSpell(ISpell spell);
        void Terminate();
    }
}