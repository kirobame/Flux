using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Flux
{
    public static class Extensions
    {
        public static string GetNiceName(this Enum value) => $"{value.GetType().GetNiceName()}.{value}";
        public static string GetNiceName(this Type type) => type.FullName.Replace('+', '.');

        public static int IndexOf<T>(this IEnumerable<T> collection, T value)
        {
            var index = 0;
            foreach (var item in collection)
            {
                if (item.Equals(value)) return index;
                index++;
            }

            return -1;
        }
        
        public static Vector2 ToX(this float value) => new Vector2(value, 0f);
        public static Vector2 ToY(this float value) => new Vector2(0f, value);

        public static Color SetAlpha(this Color color, float alpha)
        {
            color.a = alpha;
            return color;
        }

        public static Rect Pad(this Rect rect, Vector4 padding)
        {
            rect = PadHorizontally(rect, new Vector2(padding.x, padding.y));
            rect = PadVertically(rect, new Vector2(padding.z, padding.w));
            return rect;
        }
        public static Rect PadHorizontally(this Rect rect, Vector2 padding)
        {
            rect.x += padding.x;
            rect.width -= padding.y * 2f;
            return rect;
        }
        public static Rect PadVertically(this Rect rect, Vector2 padding)
        {
            rect.y += padding.x;
            rect.height -= padding.y * 2f;
            return rect;
        }
    }
}