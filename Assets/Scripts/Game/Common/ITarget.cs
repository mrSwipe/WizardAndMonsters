using UnityEngine;

namespace Common
{
    public interface ITarget
    {
        bool IsAlive { get; }
        Vector3 Position { get; }
    }
}