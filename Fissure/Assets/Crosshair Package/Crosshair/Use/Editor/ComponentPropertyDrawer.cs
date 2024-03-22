using UnityEditor;
using UnityEngine;

namespace Weapons.Crosshair
{
    /// Author: L1nkCC
    /// Created: 12/5/2023
    /// Last Edited: 12/5/2023
    /// 
    /// <summary>
    /// Drawer for Crosshair Components. It will show the texture and allow editing of the texture and color
    /// </summary>
    [CustomPropertyDrawer(typeof(Component))]
    public class ComponentPropertyDrawer : PropertyDrawer
    {
        public static float LABEL_WIDTH = 130f;
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            GUILayout.BeginHorizontal();

            //Draw label
            if (!label.Equals(GUIContent.none))
                EditorGUILayout.LabelField(label, GUIContent.none, GUILayout.Width(LABEL_WIDTH));

            //Editable properties
            EditorGUILayout.PropertyField(property.FindPropertyRelative("Texture"), GUIContent.none);
            EditorGUILayout.PropertyField(property.FindPropertyRelative("Color"), GUIContent.none);

            GUILayout.EndHorizontal();

            EditorGUI.EndProperty();
        }
        //remove top space
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) { return 0; }
    }
}
