using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Rendering;

namespace Flux.Editor
{
    public class UnityEventAltDrawer : UnityEditorInternal.UnityEventDrawer
    {
        private ReorderableList reorderableList;
        
        protected override void SetupReorderableList(ReorderableList reorderableList)
        {
            base.SetupReorderableList(reorderableList);
            this.reorderableList = reorderableList;

            reorderableList.drawElementBackgroundCallback += DrawEventBackground;
            
            reorderableList.draggable = reorderableList.serializedProperty.isExpanded;
            
            reorderableList.displayAdd = false;
            reorderableList.displayRemove = false;
        }

        protected override void DrawEventHeader(Rect rect)
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
            if (GUI.Button(rect, icon, EditorStyles.toolbarButton))
            {
                var arrayProperty = reorderableList.serializedProperty;
                
                if (arrayProperty.arraySize > 0) arrayProperty.InsertArrayElementAtIndex(arrayProperty.arraySize - 1);
                else arrayProperty.InsertArrayElementAtIndex(0);
            }
        }
        protected virtual void DrawRemoveButton(Rect rect)
        {
            var icon = EditorGUIUtility.IconContent("d_Toolbar Minus@2x");
            var isSelectionValid = reorderableList.index >= 0 && reorderableList.index < reorderableList.serializedProperty.arraySize;
            
            if (GUI.Button(rect, icon, EditorStyles.toolbarButton) && isSelectionValid)
            {
                var arrayProperty = reorderableList.serializedProperty;
                arrayProperty.DeleteArrayElementAtIndex(reorderableList.index);
            }
        }

        protected override void DrawEvent(Rect rect, int index, bool isActive, bool isFocused)
        {
            if (!reorderableList.serializedProperty.isExpanded) return;
            base.DrawEvent(rect, index, isActive, isFocused);
        }
        protected virtual void DrawEventBackground(Rect rect, int index, bool isActive, bool isFocused)
        {
            var color = Color.black;

            if (reorderableList.serializedProperty.arraySize == 0) color = new Color(0.254902f, 0.254902f, 0.254902f);
            else if (isActive && isFocused) color = new Color(0.172549f, 0.3647059f, 0.5294118f);
            else color = index % 2 == 0 ? new Color(0.254902f, 0.254902f, 0.254902f) : new Color(0.2264151f, 0.2264151f, 0.2264151f);
            
            EditorGUI.DrawRect(rect, color);
        }
        
        public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
        {
            rect.y += EditorGUIUtility.standardVerticalSpacing;
            if (reorderableList.serializedProperty.isExpanded)
            {
                base.OnGUI(rect, property, label);
                EditorGUILayout.Space(-EditorGUIUtility.singleLineHeight);
            }
            else
            {
                rect.x += 6f;
                rect.width -= 12f;
                rect.y += 1f;
                rect.height -= EditorGUIUtility.singleLineHeight / 3f;

                Handles.BeginGUI();
                
                var frameRect = rect;
                frameRect.x -= 5f;
                frameRect.width += 11f;
                
                var outlineColor = new Color(0.1411765f,0.1411765f,0.1411765f);
                Handles.DrawSolidRectangleWithOutline(frameRect, new Color(0.2078432f, 0.2078432f, 0.2078432f), outlineColor);
                
                DrawEventHeader(rect);
                
                frameRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                frameRect.height /= 3f;
                Handles.DrawSolidRectangleWithOutline(frameRect, new Color(0.254902f,0.254902f,0.254902f), outlineColor);
                
                var compensationRect = rect;
                compensationRect.x += rect.xMax - 58f;
                compensationRect.width = EditorGUIUtility.singleLineHeight * 2f + 3f;
                compensationRect.y = rect.yMax - 3f;
                compensationRect.height = 1f;
                EditorGUI.DrawRect(compensationRect, new Color(0.2352941f,0.2352941f,0.2352941f));
                
                Handles.EndGUI();
                
                EditorGUILayout.Space(EditorGUIUtility.standardVerticalSpacing);
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (reorderableList == null) return base.GetPropertyHeight(property, label);
            else if (!reorderableList.serializedProperty.isExpanded)
            {
                var lineHeight = EditorGUIUtility.singleLineHeight;
                return lineHeight + EditorGUIUtility.standardVerticalSpacing * 2 + lineHeight / 3f;
            }
            else return base.GetPropertyHeight(property, label);
        }
    }
}