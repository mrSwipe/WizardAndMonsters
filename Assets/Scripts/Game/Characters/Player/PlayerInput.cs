using Common.Contracts;
using Spells.Contracts;
using UnityEngine;
using Zenject;

namespace Characters
{
    public class PlayerInput : MonoBehaviour
    {
        [Inject] private readonly ISpellsManager _spellsManager;
        [Inject] private readonly IGameManager _gameManager;

        private void Update()
        {
            if (!_gameManager.IsGameActive)
            {
                return;
            }
            
            if (Input.GetKeyDown(KeyCode.Q))
            {
                _spellsManager.SwitchedPrevSpell();
            }
            
            if (Input.GetKeyDown(KeyCode.W))
            {
                _spellsManager.SwitchedNextSpell();
            }
            
            if (Input.GetKeyDown(KeyCode.X))
            {
                _spellsManager.FireCurrentSpell();
            }
        }
    }
}