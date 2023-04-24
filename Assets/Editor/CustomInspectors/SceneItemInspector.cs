using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SceneItem))]
public class SceneItemInspector : Editor
{
    private SpriteRenderer renderer;

    private SceneItem sceneItem;

    private SerializedProperty itemDataProperty;
    private SerializedProperty itemCountProperty;
    private SerializedProperty useRandomRarityProperty;
    private SerializedProperty rarityProperty;

    private int itemType = -1;

    private void OnEnable()
    {
        sceneItem = (SceneItem)target;

        renderer = sceneItem.GetComponent<SpriteRenderer>();

        itemDataProperty = serializedObject.FindProperty("itemData");
        itemCountProperty = serializedObject.FindProperty("itemCount");
        useRandomRarityProperty = serializedObject.FindProperty("randomRarity");
        rarityProperty = serializedObject.FindProperty("_definedRarity");

        if(itemDataProperty.objectReferenceValue != null)
        {
            SerializedProperty itemTypeProperty = new SerializedObject(itemDataProperty.objectReferenceValue).FindProperty("itemType");
            itemType = itemTypeProperty.enumValueIndex;
        }
    }

    public override void OnInspectorGUI()
    {        
        GUI.enabled = false;
        EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour((SceneItem)target), typeof(SceneItem), false);
        GUI.enabled = true;

        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(itemDataProperty);

        if (EditorGUI.EndChangeCheck())
        {            
            SerializedObject itemDataObject = new SerializedObject(itemDataProperty.objectReferenceValue);
            SerializedProperty itemTypeProperty = itemDataObject.FindProperty("itemType");
            renderer.sprite = (Sprite)itemDataObject.FindProperty("itemSprite").objectReferenceValue;
            itemType = itemTypeProperty.enumValueIndex;

            sceneItem.gameObject.name = itemTypeProperty.enumDisplayNames[itemType] + " (" + itemDataObject.FindProperty("itemName").stringValue + ")";
        }

        if (itemType == (int)ItemType.Utility)
        {
            EditorGUILayout.PropertyField(itemCountProperty);
        }

        EditorGUILayout.PropertyField(useRandomRarityProperty);

        if(false == useRandomRarityProperty.boolValue)
        {
            EditorGUILayout.PropertyField(rarityProperty);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
