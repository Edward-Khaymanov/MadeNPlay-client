#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEditor;
using UnityEngine;

namespace MadeNPlayShared
{
    public class NetworkIdGenerator
    {
        [MenuItem("Window/NetworkId/Generate Network Ids")]
        public static void GenerateNetworkIds()
        {
            var networkIds = GetAllAssetsOfType<NetworkId>();
            GenerateId(networkIds);
            SavePrefabs(networkIds);
        }

        [MenuItem("Window/NetworkId/Validate")]
        public static void Validate()
        {
            var networkObjects = GetAllAssetsOfType<NetworkObject>();

            var nonIdObjects = networkObjects
                .Where(x => x.GetComponent<NetworkId>() == null)
                .ToList();

            AddIdComponent(nonIdObjects);

            var emptyIdObjects = networkObjects
                .Select(x => x.GetComponent<NetworkId>())
                .Where(x => x.Id == 0)
                .ToList();

            GenerateId(emptyIdObjects);
            SavePrefabs(emptyIdObjects);
        }

        private static void AddIdComponent(IList<NetworkObject> targetObjects)
        {
            foreach (var target in targetObjects)
            {
                target.gameObject.AddComponent<NetworkId>();
            }

            Debug.Log($"{nameof(NetworkId)} component has been added to {targetObjects.Count} objects");
        }

        private static void GenerateId(IList<NetworkId> networkIds)
        {
            foreach (var id in networkIds)
            {
                id.Generate();
            }

            Debug.Log($"{networkIds.Count} id's was generated");
        }

        private static List<T> GetAllAssetsOfType<T>() where T : Object
        {
            var result = new List<T>();
            var allTypeObjects = Resources.FindObjectsOfTypeAll<T>();

            foreach (var typeObject in allTypeObjects)
            {
                var path = AssetDatabase.GetAssetPath(typeObject);
                if (string.IsNullOrWhiteSpace(path))
                    continue;

                var asset = AssetDatabase.LoadAssetAtPath<T>(path);
                result.Add(asset);
            }

            return result;
        }

        private static void SavePrefabs<T>(IEnumerable<T> prefabs) where T : Component
        {
            foreach (var prefab in prefabs)
            {
                PrefabUtility.SavePrefabAsset(prefab.gameObject);
            }
        }
    }
}
#endif