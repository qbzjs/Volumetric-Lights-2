using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SceneTransition.Editor
{
    
    using UnityEditor;

#if UNITY_EDITOR
    [CustomEditor(typeof(TransitionManager))]
    public class MenuManagerEditor : UnityEditor.Editor
    {
        private TransitionManager transitionManagerScript;

        private void OnEnable()
        {
            transitionManagerScript = (TransitionManager)target;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            
            DrawDefaultInspector();

            EditorUtility.SetDirty(transitionManagerScript);
            
            #region Warnings

            //references check
            Animator fadeImage = transitionManagerScript.FadeImageAnimator;
            Animator sizeImage = transitionManagerScript.SizeImageAnimator;
            List<Animator> animatorList = new List<Animator>() { fadeImage, sizeImage };

            if (animatorList.Any(x => x == null))
            {
                EditorGUILayout.HelpBox("References missing", MessageType.Warning, true);
            }

            #endregion

            serializedObject.ApplyModifiedProperties();
        }
    }

#endif
}