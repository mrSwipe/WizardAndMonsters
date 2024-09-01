using Common;
using UnityEngine;

namespace Characters.Contracts
{
    public interface IEnemy
    {
        int StartHealth { get; }
        
        float PosY { get; }
        
        void Construct(ITarget player, Vector3 pos);
    }
}