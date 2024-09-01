using System;
using UnityEngine;
using Random = System.Random;

namespace Environment
{
    public class SpawnSettings : MonoBehaviour
    {
        [Serializable]
        public class SpawnRangeData
        {
            [SerializeField] private float xMin;
            [SerializeField] private float xMax;
            [SerializeField] private float zMin;
            [SerializeField] private float zMax;
            
            public float XMin => xMin;
            public float XMax => xMax;
            public float ZMin => zMin;
            public float ZMax => zMax;
        }
        
        [SerializeField] private SpawnRangeData spawnRangeDataTop;
        [SerializeField] private SpawnRangeData spawnRangeDataBottom;
        [SerializeField] private SpawnRangeData spawnRangeDataLeft;
        [SerializeField] private SpawnRangeData spawnRangeDataRight;
        
        private readonly Random _rnd = new();

        private SpawnRangeData[] _spawnRanges;
        
        public Vector3 GetRandomSpawnPosition()
        {
            var range = _spawnRanges[_rnd.RandomRange(0, 3)];

            var rndX = _rnd.RandomRange(range.XMin, range.XMax);
            var rndZ = _rnd.RandomRange(range.ZMin, range.ZMax);
            
            return new Vector3(rndX, 0, rndZ);
        }

        private void Awake()
        {
            Debug.Assert(spawnRangeDataTop != null, "Error! spawnRangeDataTop is null");
            Debug.Assert(spawnRangeDataTop != null, "Error! spawnRangeDataBottom is null");
            Debug.Assert(spawnRangeDataTop != null, "Error! spawnRangeDataLeft is null");
            Debug.Assert(spawnRangeDataTop != null, "Error! spawnRangeDataRight is null");
            
            _spawnRanges = new[] { spawnRangeDataTop, spawnRangeDataBottom, spawnRangeDataLeft, spawnRangeDataRight };
        }
    }
}