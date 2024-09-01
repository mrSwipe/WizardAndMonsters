namespace Characters.Contracts
{
    public interface IEnemiesManager
    {
        void RecreateEnemy(IEnemy enemy);
        void Terminate();
    }
}