using Core.Events;
using Core.Helpers;
using Zenject;

namespace Installers
{
    public class CoreInstaller : MonoInstaller<CoreInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<Ticker>().AsSingle();
            Container.BindInterfacesAndSelfTo<EventsManager>().AsSingle();
        }
    }
}