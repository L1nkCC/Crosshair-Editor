using UnityEngine;
using UnityEditor;

namespace Weapons.Crosshair
{
    public class BuildCrosshairWindow : EditorWindow
    {
        SerializedObject m_serialized;
        SerializedObject m_crosshairData;
        CrosshairData m_data;
        bool m_previewScoped = false;
        Target m_previewTarget = Target.Standard;

        [MenuItem("Window/Weapons/Crosshair Builder")]
        public static void CreateWindow()
        {
            GetWindow<BuildCrosshairWindow>();
        }
        private void OnEnable()
        {
            titleContent = new("Crosshair Builder");
            m_serialized = new SerializedObject(this);
            Init();
        }
        private void Init()
        {
            m_data = ScriptableObject.CreateInstance(typeof(CrosshairData)) as CrosshairData;
            m_crosshairData = new SerializedObject(m_data);
        }
        private void SaveCrosshair()
        {
            AssetDatabase.CreateAsset(m_data, "Crosshairs/" + m_data);
        }
        public void OnGUI()
        {
            if (m_data == null) return;
            EditorGUILayout.BeginHorizontal();
            //left
            EditorGUILayout.BeginVertical();

            EditorGUILayout.EndVertical();

            //right
            EditorGUILayout.BeginVertical();

            EditorGUILayout.PropertyField(m_crosshairData.FindProperty("Hip"));
            EditorGUILayout.PropertyField(m_crosshairData.FindProperty("Scope"));

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();

            m_crosshairData.ApplyModifiedProperties();
            m_serialized.ApplyModifiedProperties();
        }
    }
}