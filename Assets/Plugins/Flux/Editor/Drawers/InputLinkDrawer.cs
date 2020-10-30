using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Flux.Editor
{
    [CustomPropertyDrawer(typeof(InputLink))]
    public class InputLinkDrawer : FluxPropertyDrawer
    {
        public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
        {
            var rootProperty = property.Copy();
            rect.height = EditorGUIUtility.singleLineHeight;

            property.NextVisible(true);
            var reference = property.objectReferenceValue;
            var buttonWidth = 75f;
            
            var labelRect = rect;
            labelRect.width = DrawingUtilities.labelWidth;

            var name = reference == null ? "Null" : ((InputActionReference) reference).name.Split('/').Last();
            rootProperty.isExpanded = EditorGUI.Foldout(labelRect, rootProperty.isExpanded, new GUIContent(name));
            
            var referenceRect = rect;
            referenceRect.x += labelRect.width + DrawingUtilities.horizontalSpacing;
            referenceRect.width -= buttonWidth + labelRect.width + DrawingUtilities.horizontalSpacing * 2f;
            EditorGUI.PropertyField(referenceRect, property, GUIContent.none);
            
            var buttonRect = rect;
            buttonRect.x += labelRect.width + referenceRect.width + DrawingUtilities.horizontalSpacing * 2f;
            buttonRect.width = buttonWidth;

          
            GUI.enabled = reference != null;
            property.NextVisible(false);
            if (GUI.Button(buttonRect, new GUIContent("Add Group")))
            {
                if (property.arraySize == 0) property.InsertArrayElementAtIndex(0);
                else property.InsertArrayElementAtIndex(property.arraySize - 1);
            }
            GUI.enabled = true;
            rect.y += EditorGUIUtility.standardVerticalSpacing * 4f + EditorGUIUtility.singleLineHeight;

            if (!rootProperty.isExpanded || property.arraySize <= 0) return;
            
            var lineRect = rect;
            lineRect.y -= EditorGUIUtility.standardVerticalSpacing * 2f;
            lineRect.height = 1f;
            lineRect.x += 14f;
            lineRect.width -= 14f;
            
            EditorGUI.DrawRect(lineRect, new Color(0.1611765f, 0.1611765f, 0.1611765f));
            for (var i = 0; i < property.arraySize; i++)
            {
                var elementProperty = property.GetArrayElementAtIndex(i);
                elementProperty.NextVisible(true);

                var removalWidth = 65f;
                buttonWidth = 37.5f;
                
                var handlerRect = rect;
                handlerRect.width -= buttonWidth + removalWidth + DrawingUtilities.horizontalSpacing * 2f;
                
                EditorGUI.PropertyField(handlerRect, elementProperty, GUIContent.none);

                buttonRect = rect;
                buttonRect.x += handlerRect.width + DrawingUtilities.horizontalSpacing;
                buttonRect.width = removalWidth;

                if (GUI.Button(buttonRect, new GUIContent("Remove")))
                {
                    property.DeleteArrayElementAtIndex(i);
                    return;
                }
                
                buttonRect = rect;
                buttonRect.x += handlerRect.width + removalWidth + DrawingUtilities.horizontalSpacing * 2f;
                buttonRect.width = buttonWidth; 
                
                reference = elementProperty.objectReferenceValue;
                GUI.enabled = reference != null;
                
                elementProperty.NextVisible(false);
                if (GUI.Button(buttonRect, new GUIContent("Add")))
                {
                    if (elementProperty.arraySize == 0) elementProperty.InsertArrayElementAtIndex(0);
                    else elementProperty.InsertArrayElementAtIndex(elementProperty.arraySize - 1);
                }
                GUI.enabled = true;
                rect.y += EditorGUIUtility.standardVerticalSpacing * 2f + EditorGUIUtility.singleLineHeight;
                
                buttonWidth = EditorGUIUtility.singleLineHeight;
                for (var j = 0; j < elementProperty.arraySize; j++)
                {
                    var subElementProperty = elementProperty.GetArrayElementAtIndex(j);
                    
                    var operationLabelRect = rect;
                    operationLabelRect.width = 50f;

                    var operationLabel = j < 10 ? $"0{j}" : $"{j}";
                    EditorGUI.LabelField(operationLabelRect, new GUIContent(operationLabel));
                    
                    var operationRect = rect;
                    operationRect.x += operationLabelRect.width + DrawingUtilities.horizontalSpacing;
                    operationRect.width -= operationLabelRect.width + buttonWidth + DrawingUtilities.horizontalSpacing * 2f;

                    EditorGUI.PropertyField(operationRect, subElementProperty, GUIContent.none);
                    
                    buttonRect = operationRect;
                    buttonRect.x += operationRect.width + DrawingUtilities.horizontalSpacing - 3f;
                    buttonRect.y -= 3f;
                    buttonRect.width = buttonWidth + 6f;
                    buttonRect.height += 6f;

                    if (GUI.Button(buttonRect, EditorGUIUtility.IconContent("winbtn_win_close@2x"), EditorStyles.label))
                    {
                        elementProperty.DeleteArrayElementAtIndex(j);
                    }
                    
                    lineRect = rect;
                    lineRect.y += EditorGUIUtility.singleLineHeight * 0.5f;
                    lineRect.height = 1f;
                    lineRect.x += operationLabelRect.width - 15f;
                    lineRect.width = 35f;
                    
                    EditorGUI.DrawRect(lineRect, new Color(0.1611765f, 0.1611765f, 0.1611765f));
                    rect.y += EditorGUIUtility.standardVerticalSpacing * 2f + EditorGUIUtility.singleLineHeight;
                }
                
                rect.y += EditorGUIUtility.standardVerticalSpacing * 2f;
                if (i == property.arraySize - 1) continue;

                lineRect = rect;
                lineRect.y -= EditorGUIUtility.standardVerticalSpacing * 2f;
                lineRect.height = 1f;
                lineRect.x += 14f;
                lineRect.width -= 14f;
            
                EditorGUI.DrawRect(lineRect, new Color(0.1611765f, 0.1611765f, 0.1611765f));
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (!property.isExpanded) return EditorGUIUtility.singleLineHeight;
            
            property.NextVisible(true);
            var height = EditorGUIUtility.singleLineHeight;
            
            property.NextVisible(false);
            if (property.arraySize <= 0) return height;
            
            for (var i = 0; i < property.arraySize; i++)
            {
                height += EditorGUIUtility.standardVerticalSpacing * 4f + EditorGUIUtility.singleLineHeight;
                
                var elementProperty = property.GetArrayElementAtIndex(i);
                elementProperty.NextVisible(true);
                
                elementProperty.NextVisible(false);
                for (var j = 0; j < elementProperty.arraySize; j++)
                {
                    height += EditorGUIUtility.standardVerticalSpacing * 2f + EditorGUIUtility.singleLineHeight;
                }
            }

            return height;
        }
    }
}