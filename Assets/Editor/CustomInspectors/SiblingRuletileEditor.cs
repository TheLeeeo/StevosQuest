using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SiblingRuleTile))]
[CanEditMultipleObjects]
public class SiblingRuletileEditor : RuleTileEditor
{
    public Texture2D T_empty;

    public override void RuleOnGUI(Rect rect, Vector3Int position, int neighbor)
    {
        switch (neighbor)
        {
            case 3:
                GUI.DrawTexture(rect, T_empty);
                return;
        }

        base.RuleOnGUI(rect, position, neighbor);
    }
}
