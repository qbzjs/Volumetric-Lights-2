using UnityEditor;
using UnityEngine;
using WaterFlowGPE.Bezier;

namespace WaterFlowGPE.Editor
{
    [CustomEditor(typeof(WaterFlowManager))]
    public class WaterFlowManagerInspector : UnityEditor.Editor
    {
        private WaterFlowManager _waterFlowManager;
        
        public override void OnInspectorGUI()
        {
            _waterFlowManager = target as WaterFlowManager;

            DrawDefaultInspector();
            
            //setup water flow button
            if (GUILayout.Button("Generate Water Flow")) 
            {
                Undo.RecordObject(_waterFlowManager, "Generate Water Flow");
                _waterFlowManager.GenerateWaterFlow();
                EditorUtility.SetDirty(_waterFlowManager);
            }
        }
    }
}