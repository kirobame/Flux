using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace Flux.Editor
{
    public static class DrawingUtilities
    {
        public static Vector2 standardVerticalPadding => new Vector2(3f,3f);
        public static Vector2 standardHorizontalPadding => new Vector2(3f,3f);
        public static Vector4 standardPadding => new Vector4(standardHorizontalPadding.x, standardHorizontalPadding.y, standardVerticalPadding.x, standardVerticalPadding.y);

        public static float horizontalSpacing => EditorGUIUtility.standardVerticalSpacing * 2f;
        
        public static float labelWidth => EditorGUIUtility.labelWidth + 2f;
        public static float fieldWidth => EditorGUIUtility.currentViewWidth - EditorGUIUtility.labelWidth - 25f;

        private static HashSet<(Type type, FluxPropertyDrawer instance)> drawers;

        [InitializeOnLoadMethod]
        private static void LoadDrawers()
        {
            drawers = new HashSet<(Type type, FluxPropertyDrawer drawer)>();
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (!typeof(FluxPropertyDrawer).IsAssignableFrom(type) || type == typeof(FluxPropertyDrawer)) continue;
                    
                    var attribute = type.GetCustomAttribute<CustomPropertyDrawer>();
                    var field = attribute.GetType().GetField("m_Type", BindingFlags.Instance | BindingFlags.NonPublic);

                    drawers.Add(((Type) field.GetValue(attribute), (FluxPropertyDrawer) Activator.CreateInstance(type)));
                }
            }
        }

        public static bool TryGetDrawerFor(Type type, out FluxPropertyDrawer drawerInstance)
        {
            foreach (var drawer in drawers)
            {
                if (!drawer.type.IsAssignableFrom(type)) continue;

                drawerInstance = drawer.instance;
                return true;
            }

            drawerInstance = null;
            return false;
        }
    }
}