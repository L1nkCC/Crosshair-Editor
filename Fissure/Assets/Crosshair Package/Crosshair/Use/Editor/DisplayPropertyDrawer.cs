using UnityEditor;
using UnityEngine;

namespace Weapons.Crosshair
{
    /// Author: L1nkCC
    /// Created: 11/2/2023
    /// Last Edited: 11/2/2023
    /// 
    /// <summary>
    /// Drawer for Crosshair Display
    /// </summary>
    public class DisplayPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            if(!label.Equals(GUIContent.none))
                EditorGUILayout.LabelField(label, EditorStyles.boldLabel);


            

            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.Space(50f);

            EditorGUILayout.BeginVertical();
            GUILayout.FlexibleSpace();
            Rect previewRect = EditorGUILayout.GetControlRect(GUILayout.Height(100), GUILayout.Width(100));
            EditorGUI.DrawRect(previewRect, Color.black);
            DrawPreview(property, previewRect);
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndVertical();

            //Draw Components
            EditorGUILayout.BeginVertical();
            DrawDot(property);
            DrawInner(property);
            DrawExpanding(property);
            DrawNonInheritedComponents(property);
            EditorGUILayout.EndVertical();

            EditorGUILayout.EndHorizontal();


            EditorGUI.EndProperty();
        }

        private void DrawDot(SerializedProperty property)
        {
            EditorGUILayout.PropertyField(property.FindPropertyRelative("Dot"));
        }
        private void DrawInner(SerializedProperty property)
        {
            EditorGUILayout.PropertyField(property.FindPropertyRelative("Inner"));
        }
        private void DrawExpanding(SerializedProperty property)
        {
            EditorGUILayout.PropertyField(property.FindPropertyRelative("Expanding"));
            EditorGUI.indentLevel += 2;
            EditorGUILayout.PropertyField(property.FindPropertyRelative("ShrinkSpeed"));
            EditorGUILayout.PropertyField(property.FindPropertyRelative("MaxScale"));
            EditorGUI.indentLevel -= 2;
        }
        protected virtual void DrawNonInheritedComponents(SerializedProperty property) { }

        protected virtual void DrawPreview(SerializedProperty property, Rect previewRect)
        {
            DrawComponentPreview(property.FindPropertyRelative("Dot"),previewRect);
            DrawComponentPreview(property.FindPropertyRelative("Inner"),previewRect);
            DrawComponentPreview(property.FindPropertyRelative("Expanding"),previewRect);
        }
        protected void DrawComponentPreview(SerializedProperty component, Rect previewRect)
        {
            SerializedProperty colorProperty = component.FindPropertyRelative("Color");
            Texture tex = component.FindPropertyRelative("Texture").objectReferenceValue as Texture;
            if (tex != null && colorProperty != null)
            {
                GUI.DrawTexture(previewRect, tex, ScaleMode.ScaleAndCrop, true, 0f, colorProperty.colorValue, 0f, 0f);
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) { return 0; }
    }

    [CustomPropertyDrawer(typeof(HipDisplay))]
    public class HipDisplayPropertyDrawer : DisplayPropertyDrawer
    {
        protected override void DrawNonInheritedComponents(SerializedProperty property)
        {
            base.DrawNonInheritedComponents(property);
            EditorGUILayout.PropertyField(property.FindPropertyRelative("Reload"));
        }
    }
    [CustomPropertyDrawer(typeof(ScopeDisplay))]
    public class ScopeDisplayPropertyDrawer : DisplayPropertyDrawer
    {
        protected override void DrawNonInheritedComponents(SerializedProperty property)
        {
            base.DrawNonInheritedComponents(property);
            EditorGUILayout.PropertyField(property.FindPropertyRelative("Scope"));
        }

    }

}
