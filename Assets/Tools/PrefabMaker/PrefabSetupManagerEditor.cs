namespace Script.Editor
{
    using UnityEngine;
    using UnityEditor;
    using Other;

#if UNITY_EDITOR
    
    [CustomEditor(typeof(PrefabSetupManager))]
    public class PrefabSetupManagerEditor : Editor
    {
        private PrefabSetupManager prefabSetupManager;

        private void OnEnable()
        {
            prefabSetupManager = (PrefabSetupManager)target;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            
            DrawDefaultInspector();

            bool isDirty = false;

            //buttons
            if (GUILayout.Button("Save this setup"))
            {
                isDirty = true;
                prefabSetupManager.SaveSetup();
            }

            for (int i = 0; i < prefabSetupManager.SetupList.Count; i++)
            {
                GUILayout.Space(5);
                if (GUILayout.Button($"Load [{prefabSetupManager.SetupList[i].Name}]"))
                {
                    prefabSetupManager.LoadSetup(i);
                    isDirty = true;
                }
                if (GUILayout.Button($"Delete Setup [{prefabSetupManager.SetupList[i].Name}]"))
                {
                    prefabSetupManager.DeleteSetup(i);
                    isDirty = true;
                }
                GUILayout.Space(5);
            }

            if (GUILayout.Button($"Debug"))
            {
                prefabSetupManager.DebugAllChild();
            }

            if (isDirty)
            {
                EditorUtility.SetDirty(prefabSetupManager);
                AssetDatabase.SaveAssetIfDirty(prefabSetupManager);
            }
        }
    }
    
    #endif
}


