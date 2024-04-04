using System;
using System.Collections.Generic;
using UnityEngine;

namespace Services
{
    //TODO: переделать на адрессаблы
    public class AssetsService : MonoBehaviour
    {
        private static AssetsService _instance;

        public static AssetsService Instance
        {
            get
            {
                if (_instance == null) _instance = FindObjectOfType<AssetsService>();
                return _instance;
            }
        }
        
        [SerializeField]
        private List<Asset> assets = new();

        public GameObject GetAsset(string assetName)
        {
            foreach (var asset in assets)
            {
                if (asset.name != assetName) continue;
                return asset.gameObject;
            }

            return null;
        }
        
        public T GetAsset<T>() where T : Component
        {
            var assetName = typeof(T).Name;
            return GetAsset<T>(assetName);
        }

        public T GetAsset<T>(string assetName) where T : Component
        {
            var asset = GetAsset(assetName);
            return asset.GetComponent<T>();
        }

        [Serializable]
        public class Asset
        {
            public string name;
            public GameObject gameObject;
        }
    }
}