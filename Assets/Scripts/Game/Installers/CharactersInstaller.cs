using Characters;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class CharactersInstaller : MonoInstaller<CharactersInstaller>
    {
        [SerializeField] private Player playerPrefab;
        
        [SerializeField] private EnemyRed enemyRedPrefab;
        [SerializeField] private EnemyGreen enemyGreenPrefab;
        [SerializeField] private EnemyBlue enemyBluePrefab;
        
        public override void InstallBindings()
        {
            Container.Bind<Player>().FromInstance(playerPrefab).AsSingle();
            Container.Bind<PlayerInput>().FromComponentOn(playerPrefab.gameObject).AsSingle();
            Container.Bind<PlayerMovement>().AsSingle();
            
            Container.BindMemoryPool<EnemyRed, EnemyRed.Pool>()
                .WithInitialSize(5)
                .FromComponentInNewPrefab(enemyRedPrefab)
                .UnderTransformGroup("Enemies");
            
            Container.BindMemoryPool<EnemyGreen, EnemyGreen.Pool>()
                .WithInitialSize(5)
                .FromComponentInNewPrefab(enemyGreenPrefab)
                .UnderTransformGroup("Enemies");
            
            Container.BindMemoryPool<EnemyBlue, EnemyBlue.Pool>()
                .WithInitialSize(5)
                .FromComponentInNewPrefab(enemyBluePrefab)
                .UnderTransformGroup("Enemies");
            
            Container.BindInterfacesAndSelfTo<EnemiesManager>().AsSingle();
        }
    }
}