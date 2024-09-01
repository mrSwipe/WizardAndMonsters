using UI;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class UIInstaller : MonoInstaller<UIInstaller>
    {
        [SerializeField] private WindowLevelCompleted windowLevelCompleted;
        
        public override void InstallBindings()
        {
            Container.Bind<WindowLevelCompleted>().FromInstance(windowLevelCompleted);
            Container.BindInterfacesAndSelfTo<WindowsManager>().AsSingle();
        }
    }
}