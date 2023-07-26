using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
[CustomPropertyDrawer(typeof(Optional<>))]
public class OptionalPropertyDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        var valueProperty = property.FindPropertyRelative("val");
        return EditorGUI.GetPropertyHeight(valueProperty);
    }
    public override void OnGUI(
        Rect position,
        SerializedProperty property,
        GUIContent label)
    {
        var valueProperty = property.FindPropertyRelative("val");
        var enabledProperty = property.FindPropertyRelative("enabled");

        position.width -= 24;
        EditorGUI.BeginDisabledGroup(!enabledProperty.boolValue);
        EditorGUI.PropertyField(position, valueProperty, label, true);
        EditorGUI.EndDisabledGroup();

        position.x += position.width + 24;
        position.width = position.height;
        position.x -= position.width;
        EditorGUI.PropertyField (position, enabledProperty, GUIContent.none);
    }
}
