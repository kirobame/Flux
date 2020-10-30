using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Flux.Editor
{
    public class ReferenceTrackingEditorWindow : EditorWindow
    {
        private static Color outlineColor => new Color(0.1611765f, 0.1611765f, 0.1611765f);
        
        [MenuItem("Tools/Flux/Reference Tracking")]
        private static void ShowWindow()
        {
            var window = CreateWindow<ReferenceTrackingEditorWindow>("Reference Tracking");
            
            window.Show();
            window.minSize = new Vector2(300f, 300f);
        }

        private List<(Type type, TrackReferencingAttribute attribute)> registry;
        
        private Dictionary<Type, TrackedReference[]> trackedReferences = new Dictionary<Type, TrackedReference[]>();
        private Dictionary<Type, bool> foldouts = new Dictionary<Type, bool>();

        private Vector2 scroll;

        void OnEnable()
        {
            registry = new List<(Type type, TrackReferencingAttribute attribute)>();
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in assembly.GetTypes())
                {
                    var attribute = type.GetCustomAttribute<TrackReferencingAttribute>();
                    if (attribute != null) registry.Add((type, attribute));
                }
            }
        }

        void OnGUI()
        {
            if (GUILayout.Button("Track"))
            {
                trackedReferences.Clear();
                foldouts.Clear();

                foreach (var item in registry)
                {
                    trackedReferences.Add(item.type, item.attribute.Track(item.type));
                    foldouts.Add(item.type, false);
                }
            }

            if (!trackedReferences.Any()) return;
            
            var lineRect = GUILayoutUtility.GetLastRect();
            lineRect.y += lineRect.height + 2f;
            lineRect.height = 1f;
            lineRect.x -= 6f;
            lineRect.width += 12f;
            
            EditorGUI.DrawRect(lineRect, outlineColor);
            
            scroll = GUILayout.BeginScrollView(scroll, false, true, GUIStyle.none, GUI.skin.verticalScrollbar);
            foreach (var keyValuePair in trackedReferences)
            {
                var name= keyValuePair.Key.GetNiceName();
                foldouts[keyValuePair.Key] = EditorGUILayout.Foldout(foldouts[keyValuePair.Key], new GUIContent(name));

                if (!foldouts[keyValuePair.Key])
                {
                    DrawSeparation(position.width - 13f);
                    continue;
                }
                
                EditorGUI.indentLevel++;

                var index = 0;
                foreach (var trackedReference in keyValuePair.Value)
                {
                    var shortName = trackedReference.Address.Replace(name, string.Empty);
                    shortName = shortName.Replace(".", string.Empty);

                    var color = index % 2 == 1 ? new Color(0.2196079f,0.2196079f,0.2196079f) : new Color(0.1981132f,0.1981132f,0.1981132f);
                    
                    var height = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing * 4f;
                    var rect = GUILayoutUtility.GetRect(position.width, height);

                    rect.width -= 13f;
                    EditorGUI.DrawRect(rect, color);
                    DrawParenting(rect);

                    rect.y += EditorGUIUtility.standardVerticalSpacing * 2f;
                    rect.height -= EditorGUIUtility.standardVerticalSpacing * 4f;

                    if (trackedReference.IsReferenced)
                    {
                        var content = EditorGUIUtility.IconContent("greenLight");
                        content.text = shortName;

                        var labelRect = rect;
                        labelRect.width = 100f;
                        EditorGUI.LabelField(labelRect, content);

                        var referenceRect = labelRect;
                        referenceRect.x += labelRect.width;
                        referenceRect.width = rect.width - labelRect.width - DrawingUtilities.horizontalSpacing;

                        GUI.enabled = false;
                        EditorGUI.ObjectField(referenceRect, trackedReference.Referencer, typeof(Referencer), true);
                        GUI.enabled = true;
                    }
                    else
                    {
                        var content = EditorGUIUtility.IconContent("redLight");
                        content.text = shortName;

                        EditorGUI.LabelField(rect, content);
                    }

                    if (index == keyValuePair.Value.Length - 1)
                    {
                        GUILayout.Space(6f);

                        rect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing * 2f;
                        rect.x = 0f;
                        rect.width = position.width - 13f;
                        rect.height = 6f;
                        
                        EditorGUI.DrawRect(rect, new Color(0.1981132f,0.1981132f,0.1981132f));
                        DrawParenting(rect);
                    }
                    
                    index++;
                }
                
                EditorGUI.indentLevel--;
                DrawSeparation(position.width - 13f);
            }
            GUILayout.EndScrollView();
        }

        private void DrawSeparation(float width)
        {
            EditorGUILayout.Separator();
            
            var lineRect = GUILayoutUtility.GetLastRect();
            lineRect.y += EditorGUIUtility.standardVerticalSpacing - 3f;
            lineRect.height = 1f;
            lineRect.width = width;
                
            EditorGUI.DrawRect(lineRect, outlineColor);
        }
        private void DrawParenting(Rect rect)
        {
            var parentingRect = rect;
            parentingRect.x += 6f;
            parentingRect.width = 1f;
            EditorGUI.DrawRect(parentingRect, outlineColor);
        }
    }
}