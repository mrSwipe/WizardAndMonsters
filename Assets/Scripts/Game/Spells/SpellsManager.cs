using System;
using System.Collections.Generic;
using Characters;
using Common;
using Spells.Contracts;
using Core;
using Core.Events.Contracts;
using Spells.Events;
using Spells.SpellsTypes;
using Zenject;

namespace Spells
{
    internal class SpellsManager : BaseManager, ISpellsManager
    {
        private const float LifeTimeSec = 5f;
        
        [Inject] private readonly IEventsManager _eventsManager;
        [Inject] private readonly Player _player;

        [Inject] private readonly SpellRed.Pool _spellRedPool;
        [Inject] private readonly SpellGreen.Pool _spellGreenPool;
        [Inject] private readonly SpellBlue.Pool _spellBluePool;

        private SpellType _currentSpellType;
        private List<ISpell> _spellsList;
        
        public void SwitchedPrevSpell()
        {
            _currentSpellType = _currentSpellType.GetPrevSpellType();
            _eventsManager.Fire(new EventSwitchedSpell(_currentSpellType));
        }

        public void SwitchedNextSpell()
        {
            _currentSpellType = _currentSpellType.GetNextSpellType();
            _eventsManager.Fire(new EventSwitchedSpell(_currentSpellType));
        }

        public void FireCurrentSpell()
        {
            var spell = CreateSpell(_currentSpellType);
            _spellsList.Add(spell);
        }
        
        public void RemoveSpell(ISpell spell)
        {
            if (_spellsList.Remove(spell))
            {
                DespawnSpell(spell);
            }
        }
        
        protected override void InitInternal()
        {
            _spellsList = new List<ISpell>(60);
            _currentSpellType = SpellType.Red;
            _eventsManager.Fire(new EventSwitchedSpell(_currentSpellType));
        }

        protected override void TerminateInternal()
        {
            ClearSpells();
        }

        private void ClearSpells()
        {
            foreach (var spell in _spellsList)
            {
                DespawnSpell(spell);
            }
            _spellsList.Clear();
        }
        
        private ISpell CreateSpell(SpellType spellType)
        {
            ISpell spell = spellType switch
            {
                SpellType.Red => _spellRedPool.Spawn(),
                SpellType.Green => _spellGreenPool.Spawn(),
                SpellType.Blue => _spellBluePool.Spawn(),
                _ => throw new ArgumentOutOfRangeException(nameof(spellType), spellType,
                    $"Error! Unknown spell type {spellType}")
            };
            
            spell.Fire(_player.SpellRefPoint, LifeTimeSec);
            
            return spell;
        }

        private void DespawnSpell(ISpell spell)
        {
            switch (spell.SpellType)
            {
                case SpellType.Red: 
                    _spellRedPool.Despawn(spell as SpellRed);
                    break;
                case SpellType.Green: 
                    _spellGreenPool.Despawn(spell as SpellGreen);
                    break;
                case SpellType.Blue: 
                    _spellBluePool.Despawn(spell as SpellBlue);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            } 
        }
    }
}