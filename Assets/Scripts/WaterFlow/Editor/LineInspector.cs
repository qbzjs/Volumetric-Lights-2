using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Line))]
public class LineInspector : Editor
{
    private void OnSceneGUI()
    {
        Line line = target as Line;
        Transform handleTransform = line.transform;
        Quaternion handleRotation = Tools.pivotRotation == PivotRotation.Local ? handleTransform.rotation : Quaternion.identity;
        Vector3 point0 = handleTransform.TransformPoint(line.Point0);
        Vector3 point1 = handleTransform.TransformPoint(line.Point1);
        
        //handles
        Handles.color = Color.white;
        if (line != null)
        {
            Handles.DrawLine(point0, point1);
            
            //point 0
            EditorGUI.BeginChangeCheck();
            point0 = Handles.DoPositionHandle(point0, handleRotation);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(line, "Move Point");
                EditorUtility.SetDirty(line);
                line.Point0 = handleTransform.InverseTransformPoint(point0);
            }
            Handles.Label(point0, "point 0");
            
            //point 1
            EditorGUI.BeginChangeCheck();
            point1 = Handles.DoPositionHandle(point1, handleRotation);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(line, "Move Point");
                EditorUtility.SetDirty(line);
                line.Point1 = handleTransform.InverseTransformPoint(point1);
            }
            Handles.Label(point1, "point 1");

        }
    }
}
