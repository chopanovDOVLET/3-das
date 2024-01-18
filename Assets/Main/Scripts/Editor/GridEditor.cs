using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(ItemGrid))]
public class GridEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        ItemGrid Dscript = (ItemGrid)target;

        /* Dscript.xPos = EditorGUILayout.Slider("xPos", Dscript.xPos, -5f, 5f);
         Dscript.xDistanse = EditorGUILayout.Slider("xDistanse", Dscript.xDistanse, -5f, 5f);
         Dscript.zPos = EditorGUILayout.Slider("zPos", Dscript.zPos, -5f, 5f);
         Dscript.zDistanse = EditorGUILayout.Slider("zDistanse", Dscript.zDistanse, -5f, 5f);
         Dscript.yPos = EditorGUILayout.Slider("yPos", Dscript.yPos, -5f, 5f);
         Dscript.yDistanse = EditorGUILayout.Slider("yDistanse", Dscript.yDistanse, -5f, 5f);*/

        if (Dscript.enabled)
            Dscript.generate();
    }
}
