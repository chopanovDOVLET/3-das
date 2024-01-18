using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Level : MonoBehaviour
{
    public List<Item> items = new List<Item>();

    public List<PlaceData> places = new List<PlaceData>();
    public RectTransform mixPoint;
    public List<ItemBox> itemBox;

    private List<Item> sort = new List<Item>(), sort1 = new List<Item>();

    public bool colorHelp, autoSellect;

    public void InitializeItems()
    {
        transform.localPosition = Vector3.zero;

        UIController.instance.levelTxt.text = (ItemController.instance.currentLvl + 1).ToString();

        foreach (var item in items)
        {
            item.InitializeItem();
        }

        foreach (var box in itemBox)
        {
            box.InitializeBox();
        }
    }

    public void FindPlaces(List<Item> activeItems)
    {
        foreach (var item in activeItems)
        {
            PlaceData place = new PlaceData();
            place.position = item.startPos;

            for (int i = 0; i < place.itemSide.sides.Length; i++)
            {
                place.itemSide.sides[i] = new List<int>();

                place.itemSide.isSideCollected[i] = new List<bool>();

                for (int j = 0; j < item.itemSides[i].items.Count; j++)
                {
                    if (activeItems.Contains(item.itemSides[i].items[j]))
                    {
                        place.itemSide.sides[i].Add(activeItems.IndexOf(item.itemSides[i].items[j]));

                        place.itemSide.isSideCollected[i].Add(false);

                        if (BusterController.instance.reItems.Contains(item))
                        {
                            place.isReItem = true;

                            BusterController.instance.reItemsCopy.Clear();
                            BusterController.instance.reItemsCopy = BusterController.instance.reItems.ToList();

                            place.reItemIndex = BusterController.instance.reItemsCopy.IndexOf(item);
                        }
                    }
                    else if (ItemCollector.instance.collectedItems.Contains(item.itemSides[i].items[j]))
                    {
                        place.itemSide.isSideCollected[i].Add(true);
                        place.itemSide.sides[i].Add(ItemCollector.instance.collectedItems.IndexOf(item.itemSides[i].items[j]));                     
                    }
                }
            }

            place.Eobstacle = item._obstacle;

            place.obstacle = item.obstacle;
            place.gumSide = activeItems.IndexOf(item.gumSide);

            place.isMainGumSide = item.isMainGumSide;

            place.sortingOrder = item.sortingGroup.sortingOrder;

            foreach (var box in itemBox)
            {
                if (item == box.underItem)
                {
                    place.isUnderItem = true;
                    place.boxIndex = itemBox.IndexOf(box);
                }

            }

            if (place.obstacle != null)
                place.obstacle.Hide();

            place.frontBox = item.boxFront;
            place.backBox = item.boxBack;

            item.obstacle = null;
            item.isMainGumSide = false;

            item.SwitchState(item.mainState);
            place.originalItem = item;

            places.Add(place);
        }

        Item[] l = new Item[activeItems.Count];
        activeItems.CopyTo(l);

        sort.AddRange(l);

        for (int i = 0; i < activeItems.Count; i++)
        {
        newWave:

            int rd = Random.Range(0, sort.Count);

            if (sort[rd] != activeItems[i])
            {
                sort1.Add(sort[rd]);
                sort.Remove(sort[rd]);
            }
            else if (sort.Count == 1)
            {
                sort1.Add(sort[0]);
                int rd2 = Random.Range(0, sort.Count - 1);
                Item it = sort1[rd2];

                sort1[rd2] = sort1[sort1.Count - 1];
                sort1[sort1.Count - 1] = it;
            }
            else goto newWave;
        }


        foreach (var item in ItemCollector.instance.collectedItems)
        {
            item.ResetProperties();
        }

        for (int i = 0; i < sort1.Count; i++)
        {
            sort1[i].ResetProperties();

            sort1[i].startPos = places[i].position;

            for (int j = 0; j < places[i].itemSide.sides.Length; j++)
            {
                for (int k = 0; k < places[i].itemSide.sides[j].Count; k++)
                {
                    if (!places[i].itemSide.isSideCollected[j][k])
                    {
                        sort1[i].itemSides[j].items.Add(sort1[places[i].itemSide.sides[j][k]]);
                    }
                    else if (places[i].itemSide.isSideCollected[j][k])
                    {
                        // Debug.Log("i=" + i + "   j=" + j + "  k=" + k + "  ||  " + places[i].itemSide.sides[j][k] + " E");

                        sort1[i].itemSides[j].items.Add(ItemCollector.instance.collectedItems[places[i].itemSide.sides[j][k]]);
                        ItemCollector.instance.collectedItems[places[i].itemSide.sides[j][k]].itemSides[1].items.Add(sort1[i]);
                    }
                }
            }

            sort1[i]._obstacle = places[i].Eobstacle;

            if (places[i].isReItem)
                BusterController.instance.reItems[places[i].reItemIndex] = sort1[i];

            if (places[i].isUnderItem)
                itemBox[places[i].boxIndex].underItem = sort1[i];

            sort1[i].boxFront = places[i].frontBox;

            sort1[i].boxBack = places[i].backBox;

            if (sort1[i].boxBack != null)
                sort1[i].boxBack.AddFrontItem(sort1[i]);

            if (sort1[i].boxFront != null)
                sort1[i].boxFront.AddBackItem(sort1[i]);

            sort1[i].sortingGroup.sortingOrder = places[i].sortingOrder;

            if (sort1[i]._obstacle == ItemData.EObstacle.Gum)
            {
                sort1[i].isMainGumSide = places[i].isMainGumSide;
                sort1[i].gumSide = sort1[places[i].gumSide];
            }

            sort1[i].obstacle = places[i].obstacle;

            if (sort1[i].obstacle != null)
                sort1[i].obstacle.SwitchItemState(sort1[i]);
        }

        sort.Clear();
        sort1.Clear();
        places.Clear();
    }
}

[System.Serializable]
public class PlaceData
{
    public Item originalItem;
    public Vector3 position;
    public ItemSide itemSide = new ItemSide();
    public ItemData.EObstacle Eobstacle = new ItemData.EObstacle();

    public int sortingOrder = -1;

    public IObstacle obstacle;

    public int gumSide = 0;
    public bool isMainGumSide;

    public bool isReItem;
    public int reItemIndex = 0;

    public bool isUnderItem;
    public int boxIndex = 0;

    public ItemBox frontBox;
    public ItemBox backBox;

}

[System.Serializable]
public class ItemSide
{
    public List<int>[] sides = new List<int>[6];
    public List<bool>[] isSideCollected = new List<bool>[6];
}