using UnityEditor;
using UnityEngine;

namespace Flux.Editor
{
    [CustomPropertyDrawer(typeof(Marker))]
    public class MarkerDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(rect, label, property);
            
            var labelRect = new Rect(rect.position, new Vector2(DrawingUtilities.labelWidth, rect.height));
            EditorGUI.LabelField(labelRect, label);
            
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            
            var reference = property.objectReferenceValue as Marker;
            if (reference != null)
            {
                var width = (rect.width - labelRect.width) * 0.5f;
                var fieldRect = new Rect(rect.position + labelRect.width.ToX(), new Vector2(width - 1f, rect.height));
                EditorGUI.PropertyField(fieldRect, property, GUIContent.none);
                
                GUI.enabled = false;
                
                fieldRect.x += width + 1f;
                EditorGUI.TextField(fieldRect, reference.Name);

                GUI.enabled = true;
            }
            else
            {
                var fieldRect = new Rect(rect.position + labelRect.width.ToX(), new Vector2(rect.width - labelRect.width, rect.height));
                EditorGUI.PropertyField(fieldRect, property, GUIContent.none);
            }
            
            EditorGUI.indentLevel = indent;
            EditorGUI.EndProperty();
        }
    }
}