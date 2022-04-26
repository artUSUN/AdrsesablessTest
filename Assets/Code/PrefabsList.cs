using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Code
{
    [CreateAssetMenu(fileName = "PrefabsList", menuName = "PrefabsList", order = 0)]
    public class PrefabsList : ScriptableObject
    {
        [SerializeField] private List<PrefabData> prefabData;

        public List<PrefabData> Data => prefabData;
    }

    [Serializable]
    public class PrefabData
    {
        public string PrefabName;
        public AssetReference PrefabReference;
    }
}