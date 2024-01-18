using DG.Tweening;
using ExtensionMethods;
using System.Collections;
using UnityEngine;
using static ItemData;

public class GumState : BaseState
{

    public override void InitializeItem(Item item)
    {

        if (item.isMainGumSide)
        {
            item.obstacle = ObstacleController.instance.InitializeGum(item);
            item.gumSide.gumSide = item;
            item.gumSide.obstacle = item.obstacle;
        }
    }

    public override void CheckState(Item item)
    {

        if (item._state == ItemState.Collected)
            return;

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

        item._state = ItemState.Collected;
        item.gumSide._state = ItemState.Collected;

        item._obstacle = EObstacle.None;
        item.gumSide._obstacle = EObstacle.None;

        item.SwitchState(item.mainState);
        item.gumSide.SwitchState(item.gumSide.mainState);

        if (item.obstacle != null)
            item.obstacle.Remove();

        item.obstacle = null;
        item.gumSide.obstacle = null;
    }

    public override void ActiveItem(Item item)
    {
        item._state = ItemState.Active;
        item.EnableItem();
        item.SmoothOpenColor();

        if (item.gumSide.obstacle != null && (item.gumSide._state == ItemState.Active || item._state == ItemState.Active))
            item.obstacle.SmoothActive();
    }

    public override void InactiveItem(Item item)
    {

        item._state = ItemState.Inactive;
        item.DisableItem();
        item.SmoothCloseColor();

        if (item._state == ItemState.Inactive && (item.gumSide != null && item.gumSide._state == ItemState.Inactive))
            item.obstacle.SmoothInactive();
    }

    public override void ItemCollect(Item item)
    {
        if (item.gumSide._state == ItemState.Inactive)
            return;

        if (!item.isPointerOn)
        {
            item.ItemDown();
            return;
        }

        item._state = ItemState.Collected;
        item.gumSide._state = ItemState.Collected;

        item.DisableItem();
        item.gumSide.DisableItem();

        item.StartCoroutine(GumCollect(item));
    }

    public override void ItemDown(Item item)
    {
        if (item.gumSide._state == ItemState.Inactive)
            return;

        item.MoveDown();
        item.gumSide.MoveDown();

        item._state = ItemState.Active;
        item.gumSide._state = ItemState.Active;
    }

    public override void ItemUp(Item item)
    {
        if (item._obstacle != EObstacle.None && item._obstacle != EObstacle.Gum)
            return;

        if (item.gumSide._state == ItemState.Inactive)
        {
            item.Vibrate();
            item.gumSide.Vibrate();
            return;
        }

        item.MoveUp();
        item.gumSide.MoveUp();

        item._state = ItemState.Sellected;
        item.gumSide._state = ItemState.Sellected;
    }

    public override void SmoothCloseColor(Item item)
    {
        if (item.itemSides[0].items.Count != 0 && item.itemSides[0].items.AtLeastOneNotEqual(ItemState.Collected) && (item.boxFront == null || !item.boxFront.isDisabled))
        {
            item.material.DOColor(Color.gray, 0.5f);
        }

        else
            return;
    }

    public override void SmoothOpenColor(Item item)
    {
        item.material.DOColor(Color.white, 0.5f);
    }

    WaitForSeconds delay02 = new WaitForSeconds(0.2f);
    WaitForSeconds delay031 = new WaitForSeconds(0.31f);
    
    public IEnumerator GumCollect(Item item)
    {
        if (item.isMainGumSide)
        {
            item.rectTransform.anchoredPosition3D = new Vector3(item.rectTransform.anchoredPosition3D.x, item.rectTransform.anchoredPosition3D.y, -242f);
            item.gumSide.rectTransform.anchoredPosition3D = new Vector3(item.gumSide.rectTransform.anchoredPosition3D.x, item.gumSide.rectTransform.anchoredPosition3D.y, -240f);
        }
        else
        {
            item.rectTransform.anchoredPosition3D = new Vector3(item.rectTransform.anchoredPosition3D.x, item.rectTransform.anchoredPosition3D.y, -240f);
            item.gumSide.rectTransform.anchoredPosition3D = new Vector3(item.gumSide.rectTransform.anchoredPosition3D.x, item.gumSide.rectTransform.anchoredPosition3D.y, -242f);
        }


        item.rectTransform.DOMoveY(item.startPos.y + 0.1f, 0.2f);
        item.gumSide.rectTransform.DOMoveY(item.gumSide.startPos.y + 0.1f, 0.2f);

        yield return delay02;

        item.obstacle.GetTransform().SetParent(item.transform.parent);

        item.rectTransform.DOScale(item.rectTransform.localScale * 1.1f, 0.2f);
        item.gumSide.rectTransform.DOScale(item.gumSide.rectTransform.localScale * 1.1f, 0.2f);
        item.obstacle.GetRectTransform().DOScale(item.obstacle.GetRectTransform().localScale * 1.1f, 0.2f);

        yield return delay02;

        if (item.isMainGumSide)
        {
            item.rectTransform.DOAnchorPos(new Vector2(item.rectTransform.anchoredPosition.x - 25, item.rectTransform.anchoredPosition.y), 0.3f);
            item.gumSide.rectTransform.DOAnchorPos(new Vector2(item.gumSide.rectTransform.anchoredPosition.x + 25, item.gumSide.rectTransform.anchoredPosition.y), 0.3f);

            item.rectTransform.DORotate(new Vector3(0, 0, 10), 0.4f);
            item.gumSide.rectTransform.DORotate(new Vector3(0, 0, -10), 0.4f);
        }
        else
        {
            item.rectTransform.DOAnchorPos(new Vector2(item.rectTransform.anchoredPosition.x + 25, item.rectTransform.anchoredPosition.y), 0.3f);
            item.gumSide.rectTransform.DOAnchorPos(new Vector2(item.gumSide.rectTransform.anchoredPosition.x - 25, item.gumSide.rectTransform.anchoredPosition.y), 0.3f);

            item.rectTransform.DORotate(new Vector3(0, 0, -10), 0.3f);
            item.gumSide.rectTransform.DORotate(new Vector3(0, 0, 10), 0.3f);
        }

        item.obstacle.GetRectTransform().DOScaleX(item.obstacle.GetRectTransform().localScale.x * 1.5f, 0.3f);

        yield return delay031;

        item.rectTransform.DOScale(item.rectTransform.localScale / 1.1f, 0.25f);
        item.gumSide.rectTransform.DOScale(item.gumSide.rectTransform.localScale / 1.1f, 0.25f);

        item.rectTransform.DORotate(new Vector3(0, 0, 0), 0.25f);
        item.gumSide.rectTransform.DORotate(new Vector3(0, 0, 0), 0.25f);

        RemoveObstacle(item);

        AudioManager.instance.Play("Gum");

        if (item.isMainGumSide)
        {
            item.collect.Add(item);
            item.collect.Add(item.gumSide);

            ItemController.instance.CollectItem(item.collect);
        }
        else
        {
            item.collect.Add(item.gumSide);
            item.collect.Add(item);

            ItemController.instance.CollectItem(item.collect);
        }

        item.collect.Clear();
    }
}