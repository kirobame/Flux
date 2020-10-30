using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Flux.Editor
{
    public static class PropertyUtilities
    {
        public static Type GetPropertyType(this SerializedProperty property)
        {
            var parts = property.propertyPath.Split('.');
            var rootType = property.serializedObject.targetObject.GetType();
            
            foreach (var part in parts)
            {
                var bindings = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.Instance;
                var fields = new Dictionary<string, FieldInfo>();
            
                var current = rootType;
                while (current != null)
                {
                    foreach (var field in current.GetFields(bindings)) fields.Add(field.Name, field);
                    current = current.BaseType;
                }

                rootType = fields[part].FieldType;
            }

            return rootType;
        }
        public static Type GetArrayPropertyType(this SerializedProperty property)
        {
            var type = property.GetPropertyType();
            
            if (type.IsArray) return type.GetElementType();
            else return type.GetGenericArguments().First();
        }
    }
}