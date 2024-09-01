using System;
using System.Collections.Generic;
using Core.Events.Contracts;
using Spells.Events;
using UnityEngine;
using Zenject;

namespace Spells
{
    public class SpellTypeIndicator : MonoBehaviour
    {
        [Inject] private readonly IEventsManager _eventsManager;
        
        [SerializeField] private MeshRenderer meshRenderer;
        [SerializeField] private Material weaponRedMaterial;
        [SerializeField] private Material weaponGreenMaterial;
        [SerializeField] private Material weaponBlueMaterial;

        private List<Material> _weaponRedMaterials;
        private List<Material> _weaponGreenMaterials;
        private List<Material> _weaponBlueMaterials;
        
        private void Awake()
        {
            _weaponRedMaterials = new List<Material> { weaponRedMaterial };
            _weaponGreenMaterials = new List<Material> { weaponGreenMaterial };
            _weaponBlueMaterials = new List<Material> { weaponBlueMaterial };
            
            _eventsManager.Subscribe<EventSwitchedSpell>(this, OnSwitchedSpell);
            ChangeIndicator(SpellType.Red);
        }

        private void OnDestroy()
        {
            _eventsManager.Unsubscribe<EventSwitchedSpell>(OnSwitchedSpell);
        }

        private void OnSwitchedSpell(EventSwitchedSpell e)
        {
            ChangeIndicator(e.NewSpellType);
        }

        private void ChangeIndicator(SpellType spellType)
        {
            var materials = spellType switch
            {
                SpellType.Red => _weaponRedMaterials,
                SpellType.Green => _weaponGreenMaterials,
                SpellType.Blue => _weaponBlueMaterials,
                _ => throw new ArgumentOutOfRangeException(nameof(spellType), spellType, "Error! Unknown spell type")
            };
            
            meshRenderer.SetMaterials(materials);
        }
    }
}