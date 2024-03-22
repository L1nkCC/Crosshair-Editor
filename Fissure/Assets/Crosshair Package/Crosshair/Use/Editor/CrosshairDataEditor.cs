using UnityEditor;
using UnityEngine;

namespace Weapons.Crosshair
{
    /// Author: L1nkCC
    /// Created: 12/8/2023
    /// Last Edited: 12/8/2023
    /// 
    /// <summary>
    /// Allow for easy editing of CrosshairData
    /// </summary>
    [CustomEditor(typeof(CrosshairData))]
    [CanEditMultipleObjects]
    public class CrosshairDataEditor : Editor
    {
        SerializedProperty s_CanScope;
        SerializedProperty s_Hip;
        SerializedProperty s_Scope;
        bool m_hipFoldout, m_scopeFoldout;
        /// <summary>
        /// Initialize Serialized Properties and Foldout stati
        /// </summary>
        private void OnEnable()
        {
            s_CanScope = serializedObject.FindProperty("CanScope");
            s_Hip = serializedObject.FindProperty("Hip");
            s_Scope = serializedObject.FindProperty("Scope");
        }

        /// <summary>
        /// Draw and update all GUI
        /// </summary>
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUI.indentLevel++;

            m_hipFoldout = EditorGUILayout.Foldout(m_hipFoldout, "Hip");
            if (m_hipFoldout)
            {
                EditorGUILayout.PropertyField(s_Hip, GUIContent.none);
            }

            EditorGUILayout.BeginHorizontal();
            
            EditorGUILayout.PropertyField (s_CanScope, GUIContent.none, GUILayout.Width(50));
            EditorGUI.BeginDisabledGroup(!s_CanScope.boolValue);
            m_scopeFoldout = EditorGUILayout.Foldout(m_scopeFoldout, "Scope");
            EditorGUI.EndDisabledGroup();

            EditorGUILayout.EndHorizontal();
            if (m_scopeFoldout)
            {
                EditorGUILayout.PropertyField(s_Scope, GUIContent.none);
            }

            EditorGUI.indentLevel--;

            serializedObject.ApplyModifiedProperties();
        }
    }
}
