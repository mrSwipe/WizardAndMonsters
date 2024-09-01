using Spells;
using Spells.SpellsTypes;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class SpellsInstaller : MonoInstaller<SpellsInstaller>
    {
        [SerializeField] private SpellRed spellRedPrefab;
        [SerializeField] private SpellGreen spellGreenPrefab;
        [SerializeField] private SpellBlue spellBluePrefab;
        
        public override void InstallBindings()
        {
            Container.BindMemoryPool<SpellRed, SpellRed.Pool>()
                .WithInitialSize(20)
                .FromComponentInNewPrefab(spellRedPrefab)
                .UnderTransformGroup("Spells");
            
            Container.BindMemoryPool<SpellGreen, SpellGreen.Pool>()
                .WithInitialSize(20)
                .FromComponentInNewPrefab(spellGreenPrefab)
                .UnderTransformGroup("Spells");
            
            Container.BindMemoryPool<SpellBlue, SpellBlue.Pool>()
                .WithInitialSize(20)
                .FromComponentInNewPrefab(spellBluePrefab)
                .UnderTransformGroup("Spells");
            
            Container.BindInterfacesAndSelfTo<SpellsManager>().AsSingle();
        }
    }
}