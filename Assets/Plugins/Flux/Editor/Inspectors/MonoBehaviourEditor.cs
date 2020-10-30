using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Flux.Editor
{
    [CustomEditor(typeof(MonoBehaviour), true)]
    public class MonoBehaviourEditor : UnityEditor.Editor
    {
        private static bool hasBeenBootedUp;
        private static Dictionary<Type, Type> customCollections;

        private Dictionary<string, CollectionDrawer> reorderableLists;

        void OnEnable()
        {
            if (!hasBeenBootedUp) Bootup();
            Initialize();
        }

        protected virtual void Bootup()
        {
            hasBeenBootedUp = true;

            customCollections = new Dictionary<Type, Type>();
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (!typeof(CollectionDrawer).IsAssignableFrom(type)) continue;
                    
                    var customCollectionAttribute = type.GetCustomAttribute<CustomCollectionAttribute>();
                    if (customCollectionAttribute != null) customCollections.Add(customCollectionAttribute.Target, type);
                }
            }
        }
        protected virtual void Initialize()
        {
            reorderableLists = new Dictionary<string, CollectionDrawer>();
            
            var iterator = serializedObject.GetIterator();
            iterator.NextVisible(true);
            
            while (iterator.NextVisible(false))
            {
                if (iterator.propertyType == SerializedPropertyType.String || !iterator.isArray) continue;

                var type = iterator.GetArrayPropertyType();
                var drawer = default(CollectionDrawer);
                
                if (customCollections.TryGetValue(type, out var drawerType)) drawer = (CollectionDrawer)Activator.CreateInstance(drawerType, serializedObject, iterator);
                else drawer = new CollectionDrawer(serializedObject, iterator);
                
                reorderableLists.Add(iterator.name, drawer);
            }
        }
        
        public sealed override void OnInspectorGUI()
        {
            serializedObject.UpdateIfRequiredOrScript();
            
            var iterator = serializedObject.GetIterator();
            iterator.NextVisible(true);

            GUI.enabled = false;
            EditorGUILayout.PropertyField(iterator);
            GUI.enabled = true;

            Draw(iterator);

            serializedObject.ApplyModifiedProperties();
        }

        protected virtual void Draw(SerializedProperty iterator) { while (iterator.NextVisible(false)) DrawProperty(iterator); }
        protected virtual void DrawProperty(SerializedProperty property)
        {
            if (IsCollection(property)) DrawCollection(property);
            else EditorGUILayout.PropertyField(property);
        }

        protected bool IsCollection(SerializedProperty property) => reorderableLists.Keys.Contains(property.name);
        protected void DrawCollection(SerializedProperty property) => reorderableLists[property.name].Draw();
    }
}