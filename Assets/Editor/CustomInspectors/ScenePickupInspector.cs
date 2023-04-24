using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ScenePickup))]
public class ScenePickupInspector : Editor
{
    private SpriteRenderer renderer;

    private ScenePickup sceneItem;

    private SerializedProperty itemDataProperty;

    private void OnEnable()
    {
        sceneItem = (ScenePickup)target;

        renderer = sceneItem.GetComponent<SpriteRenderer>();

        itemDataProperty = serializedObject.FindProperty("itemData");
    }

    public override void OnInspectorGUI()
    {
        GUI.enabled = false;
        EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour((ScenePickup)target), typeof(ScenePickup), false);
        GUI.enabled = true;

        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(itemDataProperty);
        if (EditorGUI.EndChangeCheck())
        {
            SerializedObject itemDataObject = new SerializedObject(itemDataProperty.objectReferenceValue);
            renderer.sprite = (Sprite)itemDataObject.FindProperty("itemSprite").objectReferenceValue;
            sceneItem.gameObject.name = "Pickup (" + itemDataObject.FindProperty("itemName").stringValue + ")";
        }

        serializedObject.ApplyModifiedProperties();
    }
}
