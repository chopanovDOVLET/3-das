using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Item))]
[CanEditMultipleObjects]
public class ItemEditor : Editor
{
    bool isMain = false;

    public override void OnInspectorGUI()
    {

        base.OnInspectorGUI();

        Item _item = (Item)target;

        isMain = _item.isMainGumSide;

        _item._icon.sprite = _item.controller.ApplyItemIcon(_item);

        if (_item.currentLevel != null && _item.currentLevel.autoSellect)
            HelpSideSellect(_item);

        if (_item.currentLevel != null && _item.currentLevel.colorHelp)
            ItemColor(_item);

        if (_item._obstacle == ItemData.EObstacle.Gum)
        {
            EditorGUILayout.LabelField("Obstacle", EditorStyles.boldLabel);
            _item.gumSide = (Item)EditorGUILayout.ObjectField("Gum Side", _item.gumSide, typeof(Item));

            isMain = EditorGUILayout.Toggle("isMainGumSide", isMain);
            _item.isMainGumSide = isMain;

            if (_item.isMainGumSide)
            {
                _item.itemSides[3].items[0]._obstacle = ItemData.EObstacle.Gum;

                _item.gumSide = _item.itemSides[3].items[0];
                _item.gumSide.isMainGumSide = false;
            }
            else
            {
                _item.gumSide = _item.itemSides[2].items[0];
            }
        }

        if (GUILayout.Button("Reset"))
        {
            _item.gumSide = null;
            _item.isMainGumSide = false;
        }
    }

    void HelpSideSellect(Item _item)
    {

        for (int i = 0; i < _item.itemSides.Length; i++)
        {
            for (int j = 0; j < _item.itemSides[i].items.Count; j++)
            {
                if (_item.itemSides[i].items[j] == null)
                    _item.itemSides[i].items.Remove(_item.itemSides[i].items[j]);
            }
        }

        if (_item.itemSides[0].items.Count != 0)
        {
            for (int i = 0; i < _item.itemSides[0].items.Count; i++)
            {
                if (!_item.itemSides[0].items[i].itemSides[1].items.Contains(_item))
                    _item.itemSides[0].items[i].itemSides[1].items.Add(_item);
            }
        }

        if (_item.itemSides[1] != null)
        {
            for (int i = 0; i < _item.itemSides[1].items.Count; i++)
            {
                if (!_item.itemSides[1].items[i].itemSides[0].items.Contains(_item))
                    _item.itemSides[1].items[i].itemSides[0].items.Add(_item);
            }
        }

        if (_item.itemSides[2] != null)
        {
            for (int i = 0; i < _item.itemSides[2].items.Count; i++)
            {
                if (!_item.itemSides[2].items[i].itemSides[3].items.Contains(_item))
                    _item.itemSides[2].items[i].itemSides[3].items.Add(_item);
            }
        }

        if (_item.itemSides[3] != null)
        {
            for (int i = 0; i < _item.itemSides[3].items.Count; i++)
            {
                if (!_item.itemSides[3].items[i].itemSides[2].items.Contains(_item))
                    _item.itemSides[3].items[i].itemSides[2].items.Add(_item);
            }
        }

        if (_item.itemSides[4] != null)
        {
            for (int i = 0; i < _item.itemSides[4].items.Count; i++)
            {
                if (!_item.itemSides[4].items[i].itemSides[5].items.Contains(_item))
                    _item.itemSides[4].items[i].itemSides[5].items.Add(_item);
            }
        }

        if (_item.itemSides[5] != null)
        {
            for (int i = 0; i < _item.itemSides[5].items.Count; i++)
            {
                if (!_item.itemSides[5].items[i].itemSides[4].items.Contains(_item))
                    _item.itemSides[5].items[i].itemSides[4].items.Add(_item);
            }
        }
    }

    void ItemColor(Item _item)
    {

        if (_item.transform.parent == null)
            return;

        Item[] items = _item.transform.parent.GetComponentsInChildren<Item>();

        foreach (var item in items)
        {
            Color b = Color.black;

            b.a = 0.5f;

            item._icon.color = b;
        }

        _item._icon.color = Color.green;

        for (int i = 0; i < _item.itemSides.Length; i++)
        {
            if (_item.itemSides[i].items.Count != 0)
            {
                for (int j = 0; j < _item.itemSides[i].items.Count; j++)
                {
                    _item.itemSides[i].items[j]._icon.color = Color.white;
                }
            }
        }
    }
}