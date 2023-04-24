using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ItemManagerSO))]
public class TestScriptableEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var script = (ItemManagerSO)target;

        if (GUILayout.Button("Generate", GUILayout.Height(40)))
        {
            script.GenerateDatabase();
        }

    }
}