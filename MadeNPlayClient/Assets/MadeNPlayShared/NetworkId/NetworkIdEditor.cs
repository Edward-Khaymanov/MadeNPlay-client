#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace MadeNPlayShared
{
    [CustomEditor(typeof(NetworkId))]
    public class NetworkIdEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            var targetObject = target as NetworkId;
            GUILayout.Label($"Id: {targetObject.Id}");
            
            if (GUILayout.Button("Copy"))
                GUIUtility.systemCopyBuffer = targetObject.Id.ToString();

            if (GUILayout.Button("Generate id"))
                targetObject.Generate();
        }
    }
}
#endif