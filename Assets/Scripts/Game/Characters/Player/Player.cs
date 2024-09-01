using Characters.Events;
using Common;
using Common.Contracts;
using Core.Events.Contracts;
using Spells.Events;
using TMPro;
using UnityEngine;
using Zenject;

namespace Characters
{
    [RequireComponent (typeof(CharacterController))]
    [RequireComponent (typeof(PlayerMovement))]
    [RequireComponent (typeof(HealthWithProtection))]
    public class Player : MonoBehaviour
    {
        [SerializeField] private Transform spellRefPoint;
        [SerializeField] private HealthWithProtection healthWithProtection;
        [SerializeField] private PlayerMovement movement;
        [SerializeField] private TMP_Text heathText;
        [SerializeField] private TMP_Text protectionText;
        [SerializeField] private TMP_Text spellTypeText;
        
        [Inject] private readonly IEventsManager _eventsManager;
        [Inject] private readonly IGameManager _gameManager;
        
        public HealthWithProtection HealthWithProtection => healthWithProtection;
        
        public Transform SpellRefPoint => spellRefPoint;
        
        private void Awake()
        {
            healthWithProtection.OnChangeValue += OnChangeHealth;
            healthWithProtection.OnTerminate += OnTerminate;
            _eventsManager.Subscribe<EventSwitchedSpell>(this, OnSwitchedSpell);
            
            protectionText.text = $"{HealthWithProtection.ProtectionValue}";
        }

        private void OnDestroy()
        {
            healthWithProtection.OnChangeValue -= OnChangeHealth;
            healthWithProtection.OnTerminate -= OnTerminate;
            _eventsManager.Unsubscribe<EventSwitchedSpell>(OnSwitchedSpell);
        }

        private void OnChangeHealth(int v)
        {
            heathText.text = $"{v}";
        }

        private void OnSwitchedSpell(EventSwitchedSpell e)
        {
            spellTypeText.text = $"{e.NewSpellType}Spell";
        }
        
        private void OnTerminate()
        {
            _eventsManager.Fire(new EventPlayerIsDead());
        }
    }
}