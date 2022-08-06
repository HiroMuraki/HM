#nullable enable
using UnityEditor;
using UnityEngine;
using System;
using System.Reflection;

namespace HM.UnityEngine
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(OneWayBinder), true)]
    public class OneWayBinderEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            var gameObjectOrigin = serializedObject.FindProperty("gameObjectOrigin");
            var bindingComponentType = serializedObject.FindProperty("bindingComponentType");
            var bindingComponentProperty = serializedObject.FindProperty("bindingComponentProperty");

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.ObjectField(gameObjectOrigin, GUIContent.none, GUILayout.MaxWidth(200));
            string bindingPath;
            if (string.IsNullOrEmpty(bindingComponentType.stringValue) || string.IsNullOrEmpty(bindingComponentProperty.stringValue))
            {
                bindingPath = string.Empty;
            }
            else
            {
                bindingPath = bindingComponentType.stringValue + "." + bindingComponentProperty.stringValue;
            }
            if (EditorGUILayout.DropdownButton(new GUIContent(bindingPath), FocusType.Passive))
            {
                if (gameObjectOrigin.objectReferenceValue is GameObject gameObject)
                {
                    var gm = new GenericMenu();
                    foreach (var component in gameObject.GetComponents<INotifyValueChanged>())
                    {
                        var componentType = component.GetType();
                        foreach (var prop in componentType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
                        {
                            gm.AddItem(new GUIContent(componentType.Name + "/" + prop.Name), false, () =>
                            {
                                bindingComponentType.stringValue = componentType.Name;
                                bindingComponentProperty.stringValue = prop.Name;
                                serializedObject.ApplyModifiedProperties();
                            });
                        }
                    }
                    gm.ShowAsContext();
                }
            };
            EditorGUILayout.EndHorizontal();

            serializedObject.ApplyModifiedProperties();
        }
    }
}
