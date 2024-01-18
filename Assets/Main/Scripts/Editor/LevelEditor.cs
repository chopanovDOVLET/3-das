using UnityEditor;
using UnityEngine;
using System.Linq;

[CustomEditor(typeof(Level))]
[CanEditMultipleObjects]
public class LevelEditor : Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        Level level = (Level)target;


        if (level.transform.GetChild(0).GetChild(0) != null)
            level.items = level.transform.GetChild(0).GetChild(0).GetComponentsInChildren<Item>().ToList();


        foreach (var item in level.items)
        {
            item._icon.color = Color.white;
            item.currentLevel = level;
        }

    }


}
