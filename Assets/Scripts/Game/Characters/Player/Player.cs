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
    public class Player : MonoBehaviour, IDamagable, ITarget
    {
        [SerializeField] private Transform spellRefPoint;
        [SerializeField] private HealthWithProtection healthWithProtection;
        [SerializeField] private PlayerMovement movement;
        [SerializeField] private TMP_Text heathText;
        [SerializeField] private TMP_Text protectionText;
        [SerializeField] private TMP_Text spellTypeText;
        
        [Inject] private readonly IEventsManager _eventsManager;
        [Inject] private readonly IGameManager _gameManager;

        public bool IsAlive => healthWithProtection.HealthValue > 0;
        public Vector3 Position => transform.position;
        public Transform SpellRefPoint => spellRefPoint;
        
        public void ApplyDamage(int damageValue)
        {
            healthWithProtection.ApplyDamage(damageValue);
        }
        
        private void Awake()
        {
            healthWithProtection.OnChangeValue += OnChangeHealth;
            healthWithProtection.OnTerminate += OnTerminate;
            _eventsManager.Subscribe<EventSwitchedSpell>(this, OnSwitchedSpell);
            
            protectionText.text = $"{healthWithProtection.ProtectionValue}";
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