namespace Common.Contracts
{
    public interface IGameManager
    {
        bool IsGameActive { get; set; }
        void RestartGame();
    }
}