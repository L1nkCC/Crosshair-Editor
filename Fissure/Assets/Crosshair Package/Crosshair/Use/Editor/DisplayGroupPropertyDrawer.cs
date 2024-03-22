using UnityEditor;
using UnityEngine;

namespace Weapons.Crosshair
{
    [CustomPropertyDrawer(typeof(DisplayGroup<>))]
    public class DisplayGroupPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            if (!label.Equals(GUIContent.none))
                EditorGUILayout.LabelField(label, EditorStyles.boldLabel);

            EditorGUI.indentLevel++;

            for(int i = 0; i < property.FindPropertyRelative("m_displays").arraySize; i++)
            {
                EditorGUILayout.PropertyField(property.FindPropertyRelative("m_displays").GetArrayElementAtIndex(i), new GUIContent(System.Enum.GetName(typeof(Target), i)));
            }
            EditorGUI.indentLevel--;
            EditorGUI.EndProperty();
        }
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) { return 0; }
    }
}
