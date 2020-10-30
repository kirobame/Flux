using System;
using UnityEditor;
using UnityEditor.Graphs;
using UnityEditorInternal;
using UnityEngine;

namespace Flux.Editor
{
    public class CollectionDrawer
    {
        public CollectionDrawer(SerializedObject serializedObject, SerializedProperty property)
        {
            var propertyCopy = property.Copy();
            reorderableList = new ReorderableList(serializedObject, propertyCopy, true, true, false, false);

            reorderableList.drawHeaderCallback += DrawHeader;
            reorderableList.drawElementCallback += DrawElement;
            reorderableList.drawElementBackgroundCallback += DrawElementBackground;
            
            reorderableList.elementHeightCallback += GetElementHeight;
            
            reorderableList.draggable = property.isExpanded;
            
            var type = reorderableList.serializedProperty.GetArrayPropertyType();
            if (DrawingUtilities.TryGetDrawerFor(type, out drawer)) hasDrawer = true;
        }
        
        protected ReorderableList reorderableList;
        
        private bool hasDrawer;
        private FluxPropertyDrawer drawer;

        public virtual void Draw()
        {
            EditorGUILayout.Space(EditorGUIUtility.standardVerticalSpacing);
            reorderableList.DoLayoutList();
            EditorGUILayout.Space(-EditorGUIUtility.singleLineHeight);
        }

        private void DrawHeader(Rect rect)
        {
            rect.x += 10f;
            rect.width -= 10f;
            
            var arrayProperty = reorderableList.serializedProperty;
            arrayProperty.isExpanded = EditorGUI.Foldout(rect, arrayProperty.isExpanded, new GUIContent(arrayProperty.displayName));

            reorderableList.draggable = arrayProperty.isExpanded;
            
            var buttonSize = EditorGUIUtility.singleLineHeight + 2;
            var addRect = new Rect(rect.position + (rect.width - 34f).ToX(), buttonSize * Vector2.one);
            DrawAddButton(addRect);
        
            var removeRect = new Rect(addRect.position + buttonSize.ToX(), buttonSize * Vector2.one);
            DrawRemoveButton(removeRect);
        }

        protected virtual void DrawAddButton(Rect rect)
        {
            var icon = EditorGUIUtility.IconContent("d_Toolbar Plus@2x");
            if (GUI.Button(rect, icon, EditorStyles.toolbarButton)) AddElement();
        }
        protected virtual void AddElement()
        {
            var arrayProperty = reorderableList.serializedProperty;
                
            if (arrayProperty.arraySize > 0) arrayProperty.InsertArrayElementAtIndex(arrayProperty.arraySize - 1);
            else arrayProperty.InsertArrayElementAtIndex(0);
        }
        
        protected virtual void DrawRemoveButton(Rect rect)
        {
            var icon = EditorGUIUtility.IconContent("d_Toolbar Minus@2x");
            var isSelectionValid = reorderableList.index >= 0 && reorderableList.index < reorderableList.serializedProperty.arraySize;

            if (GUI.Button(rect, icon, EditorStyles.toolbarButton) && isSelectionValid) RemoveElement();
        }
        protected virtual void RemoveElement()
        {
            var arrayProperty = reorderableList.serializedProperty;
            arrayProperty.DeleteArrayElementAtIndex(reorderableList.index);
        }

        protected virtual void DrawElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            if (!reorderableList.serializedProperty.isExpanded) return;
            
            rect.y += EditorGUIUtility.standardVerticalSpacing * 2f;
            rect.height -= EditorGUIUtility.standardVerticalSpacing * 2;
            var elementProperty = reorderableList.serializedProperty.GetArrayElementAtIndex(index);
            
            if (hasDrawer)
            {
                EditorGUI.indentLevel++;
                
                drawer.withHeader = false;
                drawer.OnGUI(rect, elementProperty, GUIContent.none);

                EditorGUI.indentLevel--;
                return;
            }
            
            EditorGUI.indentLevel++;
            if (elementProperty.propertyType == SerializedPropertyType.Generic)
            {
                var copy = elementProperty.Copy();
                var count = 0;
            
                var prolong = copy.NextVisible(true);
                if (prolong)
                {
                    while (prolong)
                    {
                        prolong = copy.NextVisible(false);

                        var lastOccurence = copy.propertyPath.LastIndexOf('.');
                        if (lastOccurence != -1)
                        {
                            var parent = copy.propertyPath.Substring(0, lastOccurence);
                            if (parent != elementProperty.propertyPath) prolong = false;
                        }
                        else prolong = false;
                    
                        count++;
                    }
                }
                
                elementProperty.NextVisible(true);
                EditorGUI.PropertyField(rect, elementProperty, true);
                
                for (var i = 1; i < count; i++)
                {
                    elementProperty.NextVisible(false);
                    
                    rect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing * 2;
                    EditorGUI.PropertyField(rect, elementProperty, true);
                }
            }
            else EditorGUI.PropertyField(rect, elementProperty, GUIContent.none, true);
            EditorGUI.indentLevel--;
        }
        protected virtual void DrawElementBackground(Rect rect, int index, bool isActive, bool isFocused)
        {
            var color = Color.black;

            if (reorderableList.serializedProperty.arraySize == 0) color = new Color(0.254902f, 0.254902f, 0.254902f);
            else if (isActive && isFocused) color = new Color(0.172549f, 0.3647059f, 0.5294118f);
            else color = index % 2 == 0 ? new Color(0.254902f, 0.254902f, 0.254902f) : new Color(0.2264151f, 0.2264151f, 0.2264151f);
            
            EditorGUI.DrawRect(rect, color);
        }
        
        protected virtual float GetElementHeight(int index)
        {
            if (!reorderableList.serializedProperty.isExpanded) return 0f;
            
            var elementProperty = reorderableList.serializedProperty.GetArrayElementAtIndex(index);
            if (hasDrawer) return drawer.GetPropertyHeight(elementProperty, GUIContent.none) + EditorGUIUtility.standardVerticalSpacing * 4f;
            
            var height = EditorGUIUtility.standardVerticalSpacing * 4f;
            if (elementProperty.propertyType == SerializedPropertyType.Generic)
            {
                var count = 0;
                var copy = elementProperty.Copy();
                
                var prolong = copy.NextVisible(true);
                if (prolong)
                {
                 
                    height += EditorGUI.GetPropertyHeight(copy);
                    while (true)
                    {
                        if (!copy.NextVisible(false)) break;
  
                        var lastOccurence = copy.propertyPath.LastIndexOf('.');
                        if (lastOccurence != -1)
                        {
                            var parent = copy.propertyPath.Substring(0, lastOccurence);
                            if (parent != elementProperty.propertyPath) break;
                        }
                        else break;

                        height += EditorGUI.GetPropertyHeight(copy) + EditorGUIUtility.standardVerticalSpacing;
                        count++;
                    }
                }

                if (count == 1) height -= EditorGUIUtility.singleLineHeight;
                else if (count > 1) height += EditorGUIUtility.standardVerticalSpacing;
            }
            else height += EditorGUI.GetPropertyHeight(elementProperty);
            return height;
        }
    }
}