using UnityEditor;
using UnityEngine;

namespace Flux.Editor
{
    [CustomEditor(typeof(Referencer), true)]
    public class ReferencerEditor : MonoBehaviourEditor
    {
        protected override void Draw(SerializedProperty iterator)
        {
            iterator.NextVisible(false);
            DrawCollection(iterator);

            iterator.NextVisible(false);
            EditorGUILayout.PropertyField(iterator);
            
            var enumValue = (Referencer.Mode)iterator.enumValueIndex;
            iterator.NextVisible(false);

            if (enumValue == Referencer.Mode.Local) EditorGUILayout.PropertyField(iterator);
            base.Draw(iterator);
        }
    }
}