using DG.Tweening;
using ExtensionMethods;
using UnityEngine;
using static ItemData;

public class MainState : BaseState
{

    public override void InitializeItem(Item item)
    {

    }

    public override void CheckState(Item item)
    {
        if ((item.itemSides[0].items.Count == 0 || item.itemSides[0].items.IsAllItemsEqual(ItemState.Collected)) && (item.boxFront == null || item.boxFront.isDisabled))
            item.ActiveItem();
        else
            item.InactiveItem();
    }

    public override void CheckOnCollect(Item item)
    {

    }

    public override void RemoveObstacle(Item item)
    {

    }

    public override void ActiveItem(Item item)
    {
        item._state = ItemState.Active;

        item.EnableItem();

        item.SmoothOpenColor();
    }

    public override void InactiveItem(Item item)
    {
        item._state = ItemState.Inactive;

        item.DisableItem();

        item.SmoothCloseColor();
    }

    public override void ItemCollect(Item item)
    {
        if (item._obstacle != EObstacle.None && item._obstacle != EObstacle.Gum)
            return;

        if (!item.isPointerOn)
        {
            item.ItemDown();
            return;
        }
        item._state = ItemState.Collected;

        item.DisableItem();

        item.collect.Add(item);
        ItemController.instance.CollectItem(item.collect);

        AudioManager.instance.Play("Select tile");

        item.collect.Clear();
    }

    public override void ItemDown(Item item)
    {
        item.MoveDown();

        item._state = ItemState.Active;
    }

    public override void ItemUp(Item item)
    {

        if (item._obstacle != EObstacle.None && item._obstacle != EObstacle.Gum)
            return;

        item.MoveUp();
        item._state = ItemState.Sellected;
    }

    public override void SmoothCloseColor(Item item)
    {
        if (item.itemSides[0].items.Count != 0 && item.itemSides[0].items.AtLeastOneNotEqual(ItemState.Collected) && (item.boxFront == null || !item.boxFront.isDisabled))
            item.material.DOColor(Color.gray, 0.5f);
        else
            return;
    }

    public override void SmoothOpenColor(Item item)
    {
        item.material.DOColor(Color.white, 0.5f);
    }

}