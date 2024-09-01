using Characters.Contracts;
using Characters.Events;
using Common.Contracts;
using Core;
using Core.Events.Contracts;
using Spells.Contracts;
using UI.Contarcts;
using UnityEngine.SceneManagement;
using Zenject;

namespace Common
{
    internal class GameManager : BaseManager, IGameManager
    {
        [Inject] private readonly IEventsManager _eventsManager;
        [Inject] private readonly IEnemiesManager _enemiesManager;
        [Inject] private readonly ISpellsManager _spellsManager;
        [Inject] private readonly IWindowsManager _windowsManager;

        public bool IsGameActive { get; set; }

        public void RestartGame()
        {
            _enemiesManager.Terminate();
            _spellsManager.Terminate();
            _windowsManager.HideWindow();
            _windowsManager.Terminate();
            
            SceneManager.LoadScene("GameScene");
        }
        
        protected override void InitInternal()
        {
            _eventsManager.Subscribe<EventPlayerIsDead>(this, OnPlayerDeath);
            
            _windowsManager.HideWindow();
            IsGameActive = true;
        }

        protected override void TerminateInternal()
        {
        }
        
        private void OnPlayerDeath(EventPlayerIsDead obj)
        {
            IsGameActive = false;
            _windowsManager.ShowWindow();
        }
    }
}