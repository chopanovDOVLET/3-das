using DG.Tweening;
using ExtensionMethods;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class BusterController : MonoBehaviour
{

    public static BusterController instance;

    private ItemController itemController;

    List<Item> items = new List<Item>();
    List<Item> lastCollectedItems = new List<Item>();

    Level currnetLevel;

    [Header("Buy Busters")]
    public List<Buster> buyBusters;

    [Header("Undo")]
    [SerializeField] ParticleSystem undoPressParticle;
    [SerializeField] RectTransform undoIconRect;
    [SerializeField] GameObject undoLock;
    [SerializeField] GameObject undoAmount;
    public bool _undoLock;

    [Header("Mix")]
    [SerializeField] ParticleSystem mixPressParticle;
    [SerializeField] RectTransform mixIconRect;
    [SerializeField] ParticleSystem mixItemShine;
    [SerializeField] GameObject mixLock;
    [SerializeField] GameObject mixAmount;
    public bool _mixLock;

    [Header("ReturnTile")]
    [SerializeField] ParticleSystem rePressParticle;
    [SerializeField] RectTransform reIconRect;
    [SerializeField] ParticleSystem reItemShineTrail;
    [SerializeField] GameObject reLock;
    [SerializeField] GameObject reAmount;
    [SerializeField] List<RectTransform> rePoints = new List<RectTransform>();
    public Item[] reItems = new Item[90];
    public List<Item> reItemsCopy = new List<Item>();
    public bool _reLock;

    [Header("Magic")]
    [SerializeField] ParticleSystem magPressParticle;
    [SerializeField] ParticleSystem magIconShineParticle;
    [SerializeField] ParticleSystem magWandParticle;
    [SerializeField] SpriteRenderer magIconGlow;
    [SerializeField] ParticleSystem magExplode;
    [SerializeField] RectTransform magIconRect;
    [SerializeField] AnimationClip animLeft;
    [SerializeField] AnimationClip animRight;
    [SerializeField] LineRenderer line;
    [SerializeField] GameObject magLock;
    [SerializeField] GameObject magAmount;
    public List<RectTransform> magicPoints = new List<RectTransform>();
    public bool _magLock;

    [Header("ExtraPlace")]
    [SerializeField] ParticleSystem extPressParticle;
    [SerializeField] ParticleSystem extExplode;
    [SerializeField] RectTransform extIconRect;
    [SerializeField] RectTransform extIconNum;
    [SerializeField] GameObject extLock;
    [SerializeField] GameObject extAmount;
    public bool _extLock;
    public bool extraUsed;

    private LineRenderer[] lines = new LineRenderer[2];
    private Transform[] transforms = new Transform[3];

    private bool moveLine;

    List<Item> magiced = new List<Item>();

    [SerializeField] List<Item> allItems = new List<Item>();

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        itemController = ItemController.instance;
    }

    void Update()
    {
        if (moveLine)
        {
            for (int i = 0; i < lines.Length; i++)
            {
                lines[i].SetPosition(0, new Vector3(transforms[i + 1].position.x, transforms[i + 1].position.y, transforms[i + 1].position.z + 0.05f));

                lines[i].SetPosition(1, Vector3.Lerp(lines[i].GetPosition(1), new Vector3(transforms[0].position.x, transforms[0].position.y, transforms[0].position.z + 0.05f), Time.deltaTime * 60));
            }
        }
    }

    public void InitializeBusters(int level)
    {
        level++;

        undoLock.SetActive(true);
        mixLock.SetActive(true);
        reLock.SetActive(true);
        magLock.SetActive(true);
        extLock.SetActive(true);

        UIController.instance.DisableUndoBuster();
        UIController.instance.DisableMixBuster();
        UIController.instance.DisableReturnBuster();
        UIController.instance.DisableMagicBuster();
        UIController.instance.DisableExtraPlaceBuster();

        for (int i = 1; i <= level; i++)
        {
            if (i == 6)
            {
                undoAmount.SetActive(true);
                undoLock.SetActive(false);
                UIController.instance.EnableUndoBuster();
            }
            if (i == 7)
            {
                mixAmount.SetActive(true);
                mixLock.SetActive(false);
                UIController.instance.EnableMixBuster();
            }
            if (i == 8)
            {
                reAmount.SetActive(true);
                reLock.SetActive(false);
                UIController.instance.EnableTileReturnBuster();
            }
            if (i == 9)
            {
                magAmount.SetActive(true);
                magLock.SetActive(false);
                UIController.instance.EnableMagicBuster();
            }
            if (i == 10)
            {
                extAmount.SetActive(true);
                extLock.SetActive(false);
                UIController.instance.EnableExtraPlaceBuster();
            }
        }
    }

    public void Undo()
    {
        if (ItemCollector.instance.matchEnded)
            return;

        if (_undoLock)
            return;

        if (ResourcesData.instance.UndoSize == 0)
        {
            UIController.instance.OpenBuyBuster(buyBusters[0]);
            return;
        }

        if (itemController.lastCollectedItems.Count == 0)
            return;

        ResourcesData.instance.RemoveUndo(1);

        undoPressParticle.Play();

        undoIconRect.GetChild(0).DOScale(Vector3.one * 0.9f, 0.375f).OnComplete(() => undoIconRect.GetChild(0).DOScale(Vector3.one, 0.375f));

        undoIconRect.DORotate(new Vector3(0, 0, -10), 0.375f).OnComplete(() => undoIconRect.DORotate(new Vector3(0, 0, 0), 0.375f));

        lastCollectedItems = itemController.lastCollectedItems;


        foreach (var box in ItemController.instance.GetCurrentLevel().itemBox)
        {
            foreach (var item in lastCollectedItems)
            {
                box.ReturnLastUnboxed(item);
            }
        }

        ItemCollector.instance.RemoveItem(lastCollectedItems);

        for (int i = 0; i < lastCollectedItems.Count; i++)
        {
            itemController.items.Add(lastCollectedItems[i]);

            bool n = false;

            foreach (var box in ItemController.instance.GetCurrentLevel().itemBox)
            {
                if (box.lastCollected != null && box.lastCollected == lastCollectedItems[i])
                {
                    lastCollectedItems[i].rectTransform.DOJump(lastCollectedItems[i].startPos, 1, 1, 0.75f);
                    n = true;
                }

            }

            if (!n)
                lastCollectedItems[i].rectTransform.DOJump(lastCollectedItems[i].startPos, 1, 1, 0.5f);

            lastCollectedItems[i].rectTransform.DOScale(lastCollectedItems[i].startScale, 0.5f);
            lastCollectedItems[i].ItemDisCollect();
        }

        StartCoroutine(UndoINnu());


        AudioManager.instance.Play("Undo");

        itemController.CheckAllItems();
        itemController.DisableAllItems();
        UIController.instance.DisableAllBustersVisible();

        lastCollectedItems.Clear();

        CheckBustersOnItemRemove();
    }

    public void Mix()
    {
        if (ItemCollector.instance.matchEnded)
            return;

        if (_mixLock)
            return;

        if (ResourcesData.instance.MixSize == 0)
        {
            UIController.instance.OpenBuyBuster(buyBusters[1]);
            return;
        }

        if (itemController.items.Count == 0) return;

        ResourcesData.instance.RemoveMix(1);

        mixPressParticle.Play();

        float angle = mixIconRect.rotation.eulerAngles.z;

        mixIconRect.DOLocalRotate(new Vector3(0, 0, 360), 0.75f, RotateMode.FastBeyond360).SetRelative(true).SetEase(Ease.OutBack);

        currnetLevel = itemController.GetCurrentLevel();

        Transform mixPoint = currnetLevel.mixPoint;

        itemController.DisableAllItems();
        UIController.instance.DisableAllBustersVisible();

        foreach (var box in ItemController.instance.GetCurrentLevel().itemBox)
        {
            RectTransform rect = box.GetComponent<RectTransform>();
            box.ResetFrontItems();


            Vector3 pos1 = rect.position;

            Vector3 startPos = pos1;
            Vector3 startScale = rect.localScale;

            box.SmoothOpenColor();

            pos1 = pos1 + (rect.position - mixPoint.position) * 1.4f;

            rect.DOScale(rect.localScale * 1.15f, 0.3f);
            rect.DOMove(pos1, 0.5f).SetEase(Ease.OutQuad).OnComplete(() =>
            {
                rect.DOMove(startPos, 0.3f).SetEase(Ease.InQuad).OnComplete(() =>
                {
                    box.SmoothCloseColor();

                    rect.DOScale(startScale * 1.05f, 0.15f).SetEase(Ease.OutCubic).OnComplete(() =>
                    rect.DOScale(startScale, 0.15f).SetEase(Ease.InCubic).OnComplete(() =>
                    {
                        box.CheckState();
                    }));
                });
            });
        }

        currnetLevel.FindPlaces(itemController.items);

        AudioManager.instance.Play("Mix");

        StartCoroutine(MixIEnu());

        foreach (var item in itemController.items)
        {
            Vector3 pos = item.rectTransform.position;
            pos = pos + (item.rectTransform.position - mixPoint.position) * 1.4f;
            item.SmoothOpenColor();
            item.rectTransform.DOScale(item.rectTransform.localScale * 1.15f, 0.3f);
            item.rectTransform.DOMove(pos, 0.5f).SetEase(Ease.OutQuad).OnComplete(() =>
            {
                if (item.obstacle != null)
                    item.obstacle.SwitchItem(item);
                item.rectTransform.DOScale(item.rectTransform.localScale / 1.25f, 0.3f);

                item.rectTransform.DOMove(item.startPos, 0.3f).SetEase(Ease.InQuad).OnComplete(() =>
                {
                    ParticleSystem particle = Instantiate(mixItemShine, item.rectTransform.position, mixItemShine.transform.rotation, item.transform);

                    particle.transform.position = new Vector3(item.rectTransform.position.x, item.rectTransform.position.y, item.rectTransform.position.z - 0.05f);

                    particle.Play();

                    item.SmoothCloseColor();

                    item.rectTransform.DOScale(item.startScale * 1.05f, 0.15f).SetEase(Ease.OutCubic).OnComplete(() =>
                    item.rectTransform.DOScale(item.startScale, 0.15f).SetEase(Ease.InCubic).OnComplete(() =>
                    {
                        itemController.CheckAllItems();
                        UIController.instance.EnableAllBustersVisible();

                    }));
                });
            });
        }
    }

    public void ReturnTile(bool forFree)
    {

        if (ItemCollector.instance.matchEnded)
            return;

        if (!forFree)
        {
            if (_reLock)
                return;

            if (ResourcesData.instance.ReturnTileSize == 0)
            {
                UIController.instance.OpenBuyBuster(buyBusters[2]);
                return;
            }

            if (ItemCollector.instance.collectedItems.Count == 0)
                return;

            ResourcesData.instance.RemoveReturnTile(1);
        }

        itemController.DisableAllItems();
        UIController.instance.DisableAllBustersVisible();

        List<Item> returnedItems = new List<Item>();

        if (!forFree)
        {
            rePressParticle.Play();
            reIconRect.DOScaleY(0.8f, 0.25f).SetEase(Ease.InQuad).OnComplete(() => reIconRect.DOScaleY(1f, 0.25f).SetEase(Ease.OutQuad));
        }

        for (int i = 0; i < ItemCollector.instance.collectedItems.Count; i++)
        {
            if (i < 3)
            {
                Item returnItem = ItemCollector.instance.collectedItems[i];
                returnedItems.Add(returnItem);
                itemController.items.Add(returnItem);

                for (int j = 0; j < reItems.Length; j++)
                {
                    if (reItems[j] == null)
                    {
                        reItems[j] = returnItem;
                        break;
                    }
                }

                RemoveItemSides(returnItem);

                SetResidesItem(returnItem);

                ParticleSystem particle = Instantiate(reItemShineTrail, returnItem.rectTransform.position, reItemShineTrail.transform.rotation, returnItem.transform);

                particle.transform.position = new Vector3(returnItem.rectTransform.position.x, returnItem.rectTransform.position.y, returnItem.rectTransform.position.z - 0.05f);
                particle.Play();

                returnItem.rectTransform.DOScale(returnItem.startScale, 0.4f);

                reItemsCopy.Clear();

                reItemsCopy = reItems.ToList();

                Vector3 pos = rePoints[reItemsCopy.IndexOf(returnItem) % 3].position;

                pos.z -= (float)(reItemsCopy.IndexOf(returnItem) / 100f);


                int layer = 0;

                if (reItemsCopy.IndexOf(returnItem) < 3)
                    layer = 0;
                else if (reItemsCopy.IndexOf(returnItem) < 6)
                    layer = 1;
                else if (reItemsCopy.IndexOf(returnItem) < 9)
                    layer = 2;
                else if (reItemsCopy.IndexOf(returnItem) < 12)
                    layer = 3;
                else if (reItemsCopy.IndexOf(returnItem) < 15)
                    layer = 4;
                else if (reItemsCopy.IndexOf(returnItem) < 18)
                    layer = 5;


                pos.y -= (float)(layer / 50f);

                returnItem.rectTransform.DOJump(pos, 1, 1, 0.5f).OnComplete(() =>
                {

                    returnItem.startPos = returnItem.rectTransform.position;

                    itemController.CheckAllItems();
                    UIController.instance.EnableAllBustersVisible();

                    Destroy(particle.gameObject);

                });
                returnItem.ItemDisCollect();
            }
        }

        AudioManager.instance.Play("Return Tile");

        ItemCollector.instance.RemoveItem(returnedItems);
        itemController.lastCollectedItems.Clear();

        CheckBustersOnItemRemove();

    }

    public void Magic()
    {

        if (ItemCollector.instance.matchEnded)
            return;

        if (_magLock)
            return;

        if (ResourcesData.instance.MagicSize == 0)
        {
            UIController.instance.OpenBuyBuster(buyBusters[3]);
            return;
        }

        magiced = new List<Item>();
        items = itemController.items;

        bool isUnderNot = false;

    Up:

        if (ItemCollector.instance.collectedItems.Count == 0 || isUnderNot)
        {
            if (items.Count < 3) return;

            for (int i = 0; i < items.Count; i++)
            {
                if (items[i]._state != ItemData.ItemState.Inactive)
                {
                    if (!IsFullItem(items[i]))
                        continue;

                    magiced.Add(items[i]);
                    items.Remove(items[i]);
                    break;
                }
            }

            for (int i = 0; i < items.Count; i++)
            {
                if (items[i]._type == magiced[0]._type && magiced.Count != 3)
                    magiced.Add(items[i]);
                else if (magiced.Count == 3)
                    break;
            }
        }
        else
        {
            /*           if (items.Count + ItemCollector.instance.collectedItems.Count < 3) return;*/

            int collectedItemsCount = ItemCollector.instance.collectedItems.Count;

        newWave:

            if (!IsFullItemUnder(ItemCollector.instance.collectedItems[collectedItemsCount - 1]))
            {
                if (collectedItemsCount - 1 != 0)
                {
                    collectedItemsCount--;
                    goto newWave;
                }
                else
                {
                    isUnderNot = true;
                    goto Up;
                }
            }

            magiced.Add(ItemCollector.instance.collectedItems[collectedItemsCount - 1]);

            if (ItemCollector.instance.collectedItems.Count != 1 && collectedItemsCount - 2 > -1)
            {
                if (ItemCollector.instance.collectedItems[collectedItemsCount - 2]._type == magiced[0]._type)
                    magiced.Add(ItemCollector.instance.collectedItems[collectedItemsCount - 2]);
            }

            for (int i = 0; i < items.Count; i++)
            {
                if (items[i]._type == magiced[0]._type && magiced.Count != 3)
                    magiced.Add(items[i]);
                else if (magiced.Count == 3)
                    break;
            }


        }

        foreach (var item in magiced)
        {
            item._state = ItemData.ItemState.Collected;
        }

        foreach (var item in magiced)
        {
            SwitchItemFB(item);
        }

        ResourcesData.instance.RemoveMagic(1);

        itemController.DisableAllItems();
        UIController.instance.DisableAllBustersVisible();

        magPressParticle.Play();
        magWandParticle.Play();
        magIconRect.DORotate(new Vector3(0, 0, 41), 0.25f).SetEase(Ease.OutQuad).OnComplete(() => magIconRect.DORotate(new Vector3(0, 0, -20), 0.25f).SetEase(Ease.InOutQuad).OnComplete(() => magIconRect.DORotate(new Vector3(0, 0, 0), 0.15f).SetEase(Ease.InQuad).OnComplete(() => magIconShineParticle.Play())));

        StartCoroutine(WaitForMagicWand(magiced));
    }

    public void ExtraPlace()
    {

        if (ItemCollector.instance.matchEnded)
            return;

        if (extraUsed)
            return;

        if (_extLock)
            return;

        if (ResourcesData.instance.ExtraPlaceSize == 0)
        {
            UIController.instance.OpenBuyBuster(buyBusters[4]);
            return;
        }


        extraUsed = true;

        ResourcesData.instance.RemoveExtraPlace(1);

        extPressParticle.Play();
        extIconRect.DOLocalRotate(new Vector3(0, 0, 45), 0.5f, RotateMode.FastBeyond360).SetRelative(true);
        extIconNum.DOScale(Vector3.one * 1.2f, 0.25f).OnComplete(() => extIconNum.DOScale(Vector3.one, 0.25f));

        AudioManager.instance.Play("Extra");

        UIController.instance.AddExtraPlace(extExplode);
        ItemCollector.instance.maxPlaceCount = 8;
    }

    WaitForSeconds delay = new WaitForSeconds(.5f);

    WaitForSeconds delay2 = new WaitForSeconds(1f);

    WaitForSeconds delay08 = new WaitForSeconds(0.8f);

    IEnumerator MixIEnu()
    {
        yield return delay08;

        AudioManager.instance.Play("Mix End");

    }

    IEnumerator UndoINnu()
    {
        yield return delay;
        itemController.CheckAllItems();
        UIController.instance.EnableAllBustersVisible();
    }

    IEnumerator WaitForMagicWand(List<Item> magiced)
    {
        AudioManager.instance.Play("Magic");

        yield return delay2;

        lastCollectedItems = itemController.lastCollectedItems;

        ItemCollector.instance.RemoveItem(magiced);

        for (int i = 0; i < magiced.Count; i++)
        {
            magiced[i].GetComponent<SortingGroup>().sortingOrder = 7;

            if (items.Contains(magiced[i]))
                items.Remove(magiced[i]);

            if (lastCollectedItems.Contains(magiced[i]))
                lastCollectedItems.Remove(magiced[i]);

            foreach (var box in ItemController.instance.GetCurrentLevel().itemBox)
            {
                box.UnBox(magiced[i]);
            }

            RemoveItemsFromReItems(magiced);

            Item magic = magiced[i];

            magic.rectTransform.anchoredPosition3D = new Vector3(magic.rectTransform.anchoredPosition3D.x, magic.rectTransform.anchoredPosition3D.y, -250f);

            transforms[i] = magic.transform;

            magic.RemoveObstacle();

            magic._state = ItemData.ItemState.Collected;
            magic.SmoothOpenColor();
            magic.DisableItem();

            magic.transform.SetParent(magic.transform.parent.parent.parent);

            if (magic.rectTransform.localScale != magic.startScale)
                magic.rectTransform.DOScale(magic.startScale, 0.75f);

            magic.rectTransform.DOMove(magicPoints[i].position, 0.75f).SetEase(Ease.InOutQuad).OnComplete(() =>
            {

                SpriteRenderer glow = Instantiate(magIconGlow,
                    new Vector3(magic.transform.position.x, magic.transform.position.y, magic.transform.position.z - 0.05f),
                    Quaternion.identity, magic.transform);
                //    particle.Play();

                if (magiced.IndexOf(magic) == 0)
                {
                    magic._anim.PlayQueued("MagicUpSide");
                }
                else if (magiced.IndexOf(magic) == 1)
                {
                    lines[0] = Instantiate(line, Vector3.zero, Quaternion.identity);
                    magic._anim.PlayQueued("MagicLeftSide");
                }
                else if (magiced.IndexOf(magic) == 2)
                {
                    lines[1] = Instantiate(line, Vector3.zero, Quaternion.identity);
                    magic._anim.PlayQueued("MagicRightSide");
                    moveLine = true;
                }
            });
        }

    }

    public void MagicEnd()
    {
        moveLine = false;

        for (int i = 0; i < lines.Length; i++)
        {
            if (lines[i] != null)
                Destroy(lines[i].gameObject);
        }

        lines = null;
        lines = new LineRenderer[2];

        transforms = null;
        transforms = new Transform[3];

        ParticleSystem particle = Instantiate(magExplode, magicPoints[3].position, magExplode.transform.rotation, magicPoints[3].parent);
        particle.Play();
        for (int i = 0; i < magiced.Count; i++)
        {
            Item magic = magiced[i];
            itemController.CheckAllItems();
            UIController.instance.EnableAllBustersVisible();
            magic._state = ItemData.ItemState.Collected;
            magic.gameObject.SetActive(false);
            itemController.CheckWinLose();
        }

        CheckBustersOnItemRemove();
        CheckBustersOnItemDestroy();
    }

    private void RemoveItemSides(Item item)
    {
        if (item.itemSides[0].items.Count != 0)
        {
            for (int i = 0; i < item.itemSides[0].items.Count; i++)
            {
                item.itemSides[0].items[i].itemSides[1].items.Remove(item);

            }

            item.itemSides[0].items.Clear();
        }

        if (item.itemSides[1].items.Count != 0)
        {
            for (int i = 0; i < item.itemSides[1].items.Count; i++)
            {
                item.itemSides[1].items[i].itemSides[0].items.Remove(item);
            }
            item.itemSides[1].items.Clear();
        }

        item.boxFront = null;
        item.boxBack = null;

        foreach (var box in ItemController.instance.GetCurrentLevel().itemBox)
        {
            if (box.frontItems.Contains(item))
                box.frontItems.Remove(item);
        }

    }

    private void SetResidesItem(Item item)
    {

        reItemsCopy.Clear();
        reItemsCopy = reItems.ToList();

        if (reItemsCopy.IndexOf(item) - 3 > -1)
        {
            item.itemSides[1].items.Add(reItemsCopy[reItemsCopy.IndexOf(item) - 3]);
            reItems[reItemsCopy.IndexOf(item) - 3].itemSides[0].items.Add(item);
        }
    }

    public void RemoveItemsFromReItems(List<Item> items)
    {

        foreach (Item item in items)
        {
            if (reItems.Contains(item))
            {
                for (int i = 0; i < reItems.Length - 1; i++)
                {
                    if (reItems[i] == item)
                    {
                        reItems[i] = null;
                        break;
                    }
                }
            }
        }
    }

    private void SwitchItemFB(Item item)
    {
        if (item._state == ItemData.ItemState.Active || ItemCollector.instance.collectedItems.Contains(item))
            return;

        for (int i = 0; i < item.itemSides[1].items.Count; i++)
        {
            if (item.itemSides[1].items[i].itemSides[0].items.IsAllItemsEqual(ItemData.ItemState.Collected))
            {
                for (int j = 0; j < item.itemSides[1].items[i].itemSides[0].items.Count; j++)
                {
                    if (item.itemSides[1].items[i].itemSides[0].items[j].itemSides[0].items.AtLeastOneNotEqual(ItemData.ItemState.Collected) && (item.itemSides[1].items[i].boxFront == null || item.itemSides[1].items[i].boxFront.isDisabled))
                    {
                        List<Item> list = new List<Item>();
                        list.AddRange(item.itemSides[1].items[i].CheckRadius());

                        for (int k = 0; k < list.Count; k++)
                        {
                            if (!item.itemSides[1].items[i].itemSides[0].items.Contains(list[k]))
                                item.itemSides[1].items[i].itemSides[0].items.Add(list[k]);
                        }

                        break;
                    }
                }
            }
        }
    }

    public void CheckBustersOnItemCollect()
    {
        if (itemController.lastCollectedItems.Count != 0)
        {
            UIController.instance.EnableUndoBuster();
        }

        if (ItemCollector.instance.collectedItems.Count != 0)
        {
            UIController.instance.EnableTileReturnBuster();
        }
    }

    public void CheckBustersOnItemRemove()
    {
        if (itemController.lastCollectedItems.Count == 0)
        {
            UIController.instance.DisableUndoBuster();
        }

        if (ItemCollector.instance.collectedItems.Count == 0)
        {
            UIController.instance.DisableReturnBuster();
        }
    }

    public void CheckBustersOnItemIncrease()
    {
        if (!CheckIsFullItem())
            UIController.instance.EnableMagicBuster();
    }

    public void CheckBustersOnItemDestroy()
    {
        if (CheckIsFullItem())
            UIController.instance.DisableMagicBuster();
    }

    private bool CheckIsFullItem()
    {

        allItems.Clear();

        allItems.AddRange(ItemController.instance.items);
        allItems.AddRange(ItemCollector.instance.collectedItems);

        if (allItems.Count == 0)
            return false;

        int currentCheck = 0;
    newWave:

        int equalCount = 1;

        Item checkItem = allItems[currentCheck];

        for (int i = 0; i < allItems.Count; i++)
        {
            if (allItems[i] != checkItem && allItems[i]._type == checkItem._type)
            {
                equalCount++;

                if (equalCount == 3)
                    return false;
            }
        }

        if (currentCheck != allItems.Count - 1)
        {
            currentCheck++;

            if (currentCheck == allItems.Count)
                return true;

            goto newWave;
        }

        return true;
    }

    private bool IsFullItem(Item item)
    {
        int count = 1;

        for (int i = 0; i < items.Count; i++)
        {
            if (items[i] != item && items[i]._type == item._type)
            {
                count++;

                if (count == 3)
                    return true;
            }
        }

        return false;
    }

    private bool IsFullItemUnder(Item item)
    {
        int count = 1;

        allItems.Clear();

        allItems.AddRange(ItemCollector.instance.collectedItems);
        allItems.AddRange(ItemController.instance.items);

        for (int i = 0; i < allItems.Count; i++)
        {
            if (allItems[i] != item && allItems[i]._type == item._type)
            {
                count++;

                if (count == 3)
                    return true;
            }
        }

        return false;
    }

}

[Serializable]
public class Buster
{
    public string name;
    public int price;
    public int amount;
    public int id;
}