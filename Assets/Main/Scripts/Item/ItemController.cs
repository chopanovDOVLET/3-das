using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using MoreMountains.NiceVibrations;
using ExtensionMethods;
using Random = UnityEngine.Random;

public class ItemController : MonoBehaviour
{

    public static ItemController instance;

    [Header("Item Type Data")]
    public List<ItemTypes> itemTypes;

    [Header("Active Items")]
    public List<Item> items = new List<Item>();

    public int level;
    public bool customLevel;

    public List<Level> levels = new List<Level>();

    public Transform ItemsParent;

    public Level currnetLevel;
    public List<Item> lastCollectedItems;

    public int currentLvl;
    public int levelIndex;

    public ParticleSystem itemExplode;
    public SpriteRenderer itemCollectGlow;

    void Awake()
    {
        instance = this;
        Application.targetFrameRate = 60;
        Input.multiTouchEnabled = false;
    }

    private void Start()
    {
        DOTween.SetTweensCapacity(3125, 50);
        
        if (customLevel)
            currentLvl = level;
        else
        { 
            currentLvl = PlayerPrefs.GetInt("Level", 0);
            levelIndex = PlayerPrefs.GetInt("LevelIndex", 0);
        }

        currnetLevel = Instantiate(levels[currentLvl], levels[currentLvl].transform.position, levels[currentLvl].transform.rotation, ItemsParent);
        currnetLevel.InitializeItems();

        BusterController.instance.InitializeBusters(currentLvl);
    }

    #region Item
    public Sprite ApplyItem(Item _item)
    {
        foreach (var item in itemTypes)
        {
            if (item.type == _item._type)
            {
                items.Add(_item);
                return item.icon;
            }
        }
        return null;
    }

    public Sprite ApplyItemIcon(Item _item)
    {
        foreach (var item in itemTypes)
        {
            if (item.type == _item._type)
            {
                return item.icon;
            }
        }
        return null;
    }

    public void CollectItem(List<Item> _item)
    {

        lastCollectedItems = new List<Item>();

        if (!SettingButton.Instance.vibrationOn) MMVibrationManager.Haptic(HapticTypes.LightImpact);

        foreach (var item2 in _item)
        {
            items.Remove(item2);
            lastCollectedItems.Add(item2);

            item2.sortingGroup.sortingOrder = 6;

            foreach (var box in currnetLevel.itemBox)
            {

                /* if(box.underItem == item2)
                 {
                     for (int i = 0; i < item2.itemSides[1].items.Count; i++)
                     {
                         CheckItemFrontWithRay(item2.itemSides[1].items[i]);
                     }
                 }*/

                box.UnBox(item2);
            }
        }

        foreach (var item in items)
        {
            item.CheckOnCollect();
            item.CheckState();
        }

        foreach (var box in currnetLevel.itemBox)
        {
            box.CheckState();
        }

        BusterController.instance.RemoveItemsFromReItems(_item);

        ItemCollector.instance.Collect(_item);
    }

    public void CheckAllItems()
    {
        foreach (var item in items)
        {
            item.CheckState();
        }
    }

    public void SaveItemsInStart()
    {
        foreach (var item in items)
        {
            item.SaveStartPos();
        }
    }

    public void EnableAllItems()
    {
        foreach (var item in items)
        {
            item.EnableItem();
        }
    }

    public void DisableAllItems()
    {
        foreach (var item in items)
        {
            item.DisableItem();
        }
    }

    #endregion

    #region UI
    public void Lose()
    {
        foreach (var item in items)
        {
            item.DisableItem();

        }
        UIController.instance.OpenLosePanel();
    }

    public void Win()
    {
        levelIndex++;
        if (PlayerPrefs.GetInt("isCompletedAllLevels", 0) == 1)
            currentLvl = Random.Range(50, 100);
        else
        {
            currentLvl++;
            if (currentLvl == levels.Count)
            {
                PlayerPrefs.SetInt("isCompletedAllLevels", 1);
                currentLvl = Random.Range(50, 100);
            }
        }
        UIController.instance.OpenWinPanel();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    #endregion

    public void NewLevel()
    {
        Destroy(currnetLevel.gameObject);

        ItemCollector.instance.matchEnded = false;

        ItemCollector.instance.currentCollectCount = 0;

        items = new List<Item>();
        lastCollectedItems = new List<Item>();
        ItemCollector.instance.collectedItems = new List<Item>();

        PlayerPrefs.SetInt("Level", currentLvl);
        PlayerPrefs.SetInt("LevelIndex", levelIndex);

        currnetLevel = Instantiate(levels[currentLvl], levels[currentLvl].transform.position, levels[currentLvl].transform.rotation, ItemsParent);
        currnetLevel.InitializeItems();

        BusterController.instance.InitializeBusters(currentLvl);
    }

    public void RestartLevel()
    {
        Destroy(currnetLevel.gameObject);

        ItemCollector.instance.matchEnded = false;

        ItemCollector.instance.currentCollectCount = 0;

        items = new List<Item>();
        lastCollectedItems = new List<Item>();
        ItemCollector.instance.collectedItems = new List<Item>();

        currnetLevel = Instantiate(levels[currentLvl], levels[currentLvl].transform.position, levels[currentLvl].transform.rotation, ItemsParent);
        currnetLevel.InitializeItems();
    }

    public void CheckWinLose()
    {
        if (items.Count + ItemCollector.instance.collectedItems.Count == 0)
        {
            Win();
        }
    }

    public Level GetCurrentLevel()
    {
        return currnetLevel;
    }

    public void CheckItemFrontWithRay(Item item)
    {

        for (int i = 0; i < item.itemSides[0].items.Count; i++)
        {
            if (item.itemSides[0].items.IsAllItemsEqual(ItemData.ItemState.Collected))
            {
                List<Item> list = new List<Item>();
                list.AddRange(item.CheckRadius());

                for (int k = 0; k < list.Count; k++)
                {
                    if (!item.itemSides[0].items.Contains(list[k]))
                        item.itemSides[0].items.Add(list[k]);
                }

                item.CheckState();
            }
        }
    }

}

[Serializable]
public class ItemTypes
{
    public ItemData.Type type;
    public Sprite icon;
}

namespace ExtensionMethods
{
    public static class ItemExtension
    {
        public static bool IsAllItemsEqual(this List<Item> items, ItemData.ItemState itemState)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i]._state != itemState)
                    return false;
            }
            return true;
        }

        public static bool IsAllItemsNotEqual(this List<Item> items, ItemData.ItemState itemState)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i]._state == itemState)
                    return false;
            }
            return true;
        }

        public static bool AtLeastOneEqual(this List<Item> items, ItemData.ItemState itemState)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i]._state == itemState)
                    return true;
            }
            return false;
        }

        public static bool AtLeastOneNotEqual(this List<Item> items, ItemData.ItemState itemState)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i]._state != itemState)
                    return true;
            }
            return false;
        }
    }
}