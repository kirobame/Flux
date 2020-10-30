using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Flux.Editor
{
    [CustomPropertyDrawer(typeof(EnumAddress))]
    public class EnumAddressDrawer : PropertyDrawer
    {
        private static bool hasBeenBootedUp;
        
        private static Type[] types;
        private static string[] typeOptions;
            
        private bool hasBeenInitialized;
        
        private int typeSelection;
        private int enumSelection;
        
        public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
        {
            if (!hasBeenBootedUp) BootUp();
            if (!hasBeenInitialized) Initialize(property);
            
            EditorGUI.BeginProperty(rect, label, property);
            
            var labelRect = new Rect(rect.position, new Vector2(DrawingUtilities.labelWidth, rect.height));      
            EditorGUI.LabelField(labelRect, label);
            
            EditorGUI.BeginChangeCheck();
            
            var typeRect = new Rect(labelRect.position + DrawingUtilities.labelWidth.ToX(), new Vector2(DrawingUtilities.fieldWidth * 0.5f, rect.height));
            typeSelection = EditorGUI.Popup(typeRect, typeSelection, typeOptions);
            
            property.NextVisible(true);
            property.stringValue = types[typeSelection].AssemblyQualifiedName;
            
            if (EditorGUI.EndChangeCheck()) enumSelection = 0;
            
            var selectedType = types[typeSelection];
            var availableValues = Enum.GetValues(selectedType).Cast<Enum>().ToArray();
            var options = availableValues.Length > 0 ? Enum.GetNames(selectedType) : new string[] {"Null"};

            property.NextVisible(false);
            var enumRect = new Rect(typeRect.position + typeRect.width.ToX(), typeRect.size);

            if (options[enumSelection] == "Null") GUI.enabled = false;
            
            enumSelection = EditorGUI.Popup(enumRect, enumSelection, options);
            property.intValue = Convert.ToInt32(availableValues[enumSelection]);
            GUI.enabled = true;
            
            EditorGUI.EndProperty();
        }

        private void BootUp()
        {
            hasBeenBootedUp = true;
                
            types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => type.GetCustomAttribute<EnumAddressAttribute>() != null)
                .ToArray();

            typeOptions = types.Select(type => type.GetNiceName()).ToArray();
        }
        private void Initialize(SerializedProperty property)
        {
            hasBeenInitialized = true;

            var initializationProperty = property.Copy();
            initializationProperty.NextVisible(true);
                
            var type = Type.GetType(initializationProperty.stringValue);
            if (type != null)
            {
                typeSelection = types.IndexOf(type);
                var values = Enum.GetValues(type).Cast<Enum>().Select(Convert.ToInt32).ToArray();
                    
                initializationProperty.NextVisible(false);
                var index = values.IndexOf(initializationProperty.intValue);

                if (index != -1) enumSelection = values[index];
            }
        }
    }
}