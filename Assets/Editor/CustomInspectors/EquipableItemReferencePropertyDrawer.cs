using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(EquipableReference))]
public class ItemReferencePropertyDrawer : PropertyDrawer
{   
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        //base.OnGUI(position, property, label);
        
        EditorGUI.BeginProperty(position, label, property);

        position.height = 18;

        property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, GUIContent.none);
        Rect fieldRect = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        int oldIndentLevel = EditorGUI.indentLevel;
        EditorGUI.indentLevel += 1;

        SerializedProperty itemDataProperty = property.FindPropertyRelative("itemData");
        
        if (property.isExpanded)
        {
            position.y += 20;
            
            EditorGUI.PropertyField(position, itemDataProperty);

            if (itemDataProperty.objectReferenceValue != null)
            {
                SerializedProperty itemTypeProperty = new SerializedObject(itemDataProperty.objectReferenceValue).FindProperty("itemType");

                if (itemTypeProperty.enumValueIndex != (int)ItemType.Pickup)
                {
                    position.y += 20;

                    EditorGUI.LabelField(position, "Random Rarity?");
                    SerializedProperty defineRarityProperty = property.FindPropertyRelative("randomRarity");

                    Rect rarityToggleRect = new Rect(fieldRect.x + -15, fieldRect.y + 40, 48, 18);

                    defineRarityProperty.boolValue = EditorGUI.Toggle(rarityToggleRect, defineRarityProperty.boolValue);

                    if (false == defineRarityProperty.boolValue)
                    {
                        fieldRect.y += 40;
                        SerializedProperty rarityProperty = property.FindPropertyRelative("_definedRarity");
                        rarityProperty.enumValueIndex = (int)(ItemRarity)EditorGUI.EnumPopup(fieldRect, (ItemRarity)rarityProperty.enumValueIndex);
                    }


                    if (itemTypeProperty.enumValueIndex == (int)ItemType.Utility)
                    {
                        position.y += 20;
                        EditorGUI.PropertyField(position, property.FindPropertyRelative("itemCount"));
                    }
                }                
            }            
        }
             
        EditorGUI.indentLevel = oldIndentLevel;
        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        int numberOfExtraLines = 0;

        if (property.isExpanded)
        {
            numberOfExtraLines += 1;

            SerializedProperty itemDataProperty = property.FindPropertyRelative("itemData");
            

            if (itemDataProperty.objectReferenceValue != null)
            {
                SerializedProperty itemTypeProperty = new SerializedObject(itemDataProperty.objectReferenceValue).FindProperty("itemType");

                if(itemTypeProperty.enumValueIndex != (int)ItemType.Pickup)
                {
                    numberOfExtraLines += 1;

                    if (itemTypeProperty.enumValueIndex == (int)ItemType.Utility)
                    {
                        numberOfExtraLines += 1;
                    }
                }                
            }
        }


        return base.GetPropertyHeight(property, label) + 20 * numberOfExtraLines;
    }
}
