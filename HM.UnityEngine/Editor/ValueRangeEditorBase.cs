#nullable enable
using UnityEngine;
using HM.UnityEngine._FakeUnityAPI;

namespace HM.UnityEngine.Editor
{
    public abstract class ValueRangeEditorBase : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var minValue = property.FindPropertyRelative("minimum");
            var maxValue = property.FindPropertyRelative("maximum");
            float labelWidth = EditorGUIUtility.labelWidth;
            var labelRect = new Rect(position.x, position.y, labelWidth, position.height);
            var inputWidth = (position.width - labelWidth) / 2;
            var minValueRect = new Rect(position.x + labelWidth, position.y, inputWidth - 10, position.height);
            var maxValueRect = new Rect(position.x + labelWidth + inputWidth + 10, position.y, inputWidth - 10, position.height);

            EditorGUIUtility.labelWidth = 30;
            EditorGUI.LabelField(labelRect, label);
            EditorGUI.PropertyField(minValueRect, minValue, new GUIContent("min"));
            EditorGUI.PropertyField(maxValueRect, maxValue, new GUIContent("max"));

        }
    }
}
