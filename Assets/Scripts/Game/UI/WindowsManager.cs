using Core;
using UI.Contarcts;
using Zenject;

namespace UI
{
    internal class WindowsManager : BaseManager, IWindowsManager
    {
        [Inject] private readonly WindowLevelCompleted _windowLevelCompleted;

        public void ShowWindow()
        {
            _windowLevelCompleted.gameObject.SetActive(true);
        }

        public void HideWindow()
        {
            _windowLevelCompleted.gameObject.SetActive(false);
        }
        
        protected override void InitInternal()
        {
            HideWindow();
        }

        protected override void TerminateInternal()
        {
        }
    }
}

