namespace Spells.Contracts
{
    public interface ISpellsManager
    {
        void Init();
        void SwitchedPrevSpell();
        void SwitchedNextSpell();
        void FireCurrentSpell();
        void Terminate();
    }
}