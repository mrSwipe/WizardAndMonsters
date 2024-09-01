using Common;
using Environment;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class GameInstaller : MonoInstaller<GameInstaller>
    {
        [SerializeField] private SpawnSettings spawnSettings;
        
        public override void InstallBindings()
        {
            Container.Bind<SpawnSettings>().FromInstance(spawnSettings).AsSingle();
            Container.BindInterfacesAndSelfTo<GameManager>().AsSingle();
        }
    }
}