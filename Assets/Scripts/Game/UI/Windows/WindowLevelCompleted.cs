using Common.Contracts;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class WindowLevelCompleted : MonoBehaviour
    {
        [SerializeField] private Button restartButton;

        [Inject] private readonly IGameManager _gameManager;

        public void OnEnable()
        {
            restartButton.onClick.AddListener(RestartButtonHandler);
        }
        
        public void OnDisable()
        {
            restartButton.onClick.RemoveListener(RestartButtonHandler);
        }

        private void RestartButtonHandler()
        {
            _gameManager.RestartGame();
        }
    }
}