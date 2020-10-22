using System;
using UnityEngine;

namespace LevelGenerator
{
    [Serializable]
    public struct AdditionalAsset
    {
        public GameObject prefab;
        public int amount;
    }
}