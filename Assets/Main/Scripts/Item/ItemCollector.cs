using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ItemCollector : MonoBehaviour
{
    public static ItemCollector instance;

    public List<RectTransform> points = new List<RectTransform>();
    public List<Item> collectedItems = new List<Item>();
    private List<Item> poolList = new List<Item>();

    public int maxPlaceCount = 7;

    public int currentCollectCount;

    private enum matchState { first, second, third }

    [HideInInspector]
    public bool matchEnded;

    private void Awake()
    {
        instance = this;
        maxPlaceCount = 7;

    }

    public void Collect(List<Item> collectItems)
    {

        UIController.instance.DisableAllBustersUnvisible();

        foreach (Item item in collectItems)
        {
            item.rectTransform.anchoredPosition3D = new Vector3(item.rectTransform.anchoredPosition3D.x, item.rectTransform.anchoredPosition3D.y, -240f);

            bool isMatched = CheckPlaces(item);

            if (!isMatched)
                collectedItems.Add(item);

            List<Item> removeItems = new List<Item>();

            removeItems = CheckItemMatch(item);

            item.rectTransform.DOScale(item.startScale / 1.2f, 0.4f);

            BusterController.instance.CheckBustersOnItemCollect();

            /*  string side = null;
              float t = 0;

              int rd = Random.Range(0, 2);

              if (rd == 0)
                  side = "Up";
              else if (rd == 1)
                  side = "Down";*/

            currentCollectCount++;

            if (removeItems.Count == 3)
                currentCollectCount -= 3;


          /*  Vector3 mid = CurveCalculator.instance.CalcCurveMiddle(item.transform, points[collectedItems.IndexOf(item)].transform, side);

            Vector3 start = item.rectTransform.position;


            float t = 0;

            item.tweener = DOTween.To(() => t, x => t = x, 1, 0.5f).OnUpdate(() =>
            {
                if (item.currentTileIndex == collectedItems.IndexOf(item))
                    item.rectTransform.position = CurveCalculator.instance.CurveMove(start, mid, points[collectedItems.IndexOf(item)].transform.position, t);
                else
                    item.tweener.Kill();
            });
*/

            item.currentTileIndex = collectedItems.IndexOf(item);

            item.rectTransform.DOMove(points[collectedItems.IndexOf(item)].position, 0.5f).OnComplete(() => CheckItemSize());

            float f = 0;

            DOTween.To(() => f, x => f = x, 1, 0.5f).OnComplete(() =>
            {
                if (removeItems.Count == 3)
                    StartCoroutine(RemoveMatchedItems(removeItems));

                CheckItemSize();

                UIController.instance.EnableAllBustersUnvisible();
            });
        }

        CheckItemSize();

        foreach (var items in collectedItems)
        {
            if (items.currentTileIndex != collectedItems.IndexOf(items))
            {
                items.rectTransform.DOMove(points[collectedItems.IndexOf(items)].position, 0.5f).OnComplete(() =>
                {
                    CheckItemSize();
                });
                items.currentTileIndex = collectedItems.IndexOf(items);
            }
        }
    }

    public void CorrectPlaces()
    {
        foreach (var items in collectedItems)
        {
            if (items.currentTileIndex != collectedItems.IndexOf(items) || points[collectedItems.IndexOf(items)].position != items.rectTransform.position)
            {
                items.rectTransform.DOMove(points[collectedItems.IndexOf(items)].position, 0.5f).OnComplete(() =>
                {
                    CheckItemSize();
                });
                items.currentTileIndex = collectedItems.IndexOf(items);
            }
        }
    }

    private void CheckItemSize()
    {
        if (currentCollectCount >= maxPlaceCount)
        {
            if (!matchEnded)
                ItemController.instance.Lose();

            matchEnded = true;
        }
        else if (ItemController.instance.items.Count == 0 && collectedItems.Count == 0)
        {
            if (!matchEnded)
                ItemController.instance.Win();

            matchEnded = true;
        }
    }

    private bool CheckPlaces(Item _item)
    {

        matchState matchState = matchState.first;

        int lastMatchState = -1;

        if (collectedItems.Count < 2) return false;
        else
        {
            for (int i = 0; i < collectedItems.Count; i++)
            {
                if (collectedItems[i]._type == _item._type && matchState == matchState.first)
                {
                    lastMatchState = i;
                    matchState = matchState.second;
                }
                else if (collectedItems[i]._type == _item._type && matchState == matchState.second)
                {
                    if (lastMatchState == i - 1)
                    {
                        lastMatchState = i;
                        matchState = matchState.third;
                    }
                }
                else if (collectedItems[i]._type == _item._type && matchState == matchState.third)
                {
                    if (lastMatchState == i - 1)
                    {
                        return false;
                    }
                }
                else if (matchState == matchState.second || matchState == matchState.third)
                {
                    SortItems(lastMatchState, _item);
                    return true;
                }
            }
        }

        return false;
    }

    private void SortItems(int lastMatchState, Item _item)
    {

        if (lastMatchState + 1 == collectedItems.Count - 1)
        {
            poolList.Add(collectedItems[lastMatchState + 1]);
            collectedItems.RemoveAt(lastMatchState + 1);
        }
        else
        {
            int range = Mathf.Abs((collectedItems.Count - 1) - (lastMatchState + 1)) + 1;

            poolList.AddRange(collectedItems.GetRange(lastMatchState + 1, range));
            collectedItems.RemoveRange(lastMatchState + 1, range);
        }

        collectedItems.Add(_item);
        collectedItems.AddRange(poolList);
        poolList.Clear();
    }

    private List<Item> CheckItemMatch(Item _item)
    {

        List<Item> list = new List<Item>();

        matchState matchState = matchState.first;

        list.Add(_item);

        for (int i = 0; i < 3; i++)
        {
            if (matchState == matchState.first && collectedItems.IndexOf(_item) != 0 && collectedItems[collectedItems.IndexOf(_item) - 1]._type == _item._type)
            {
                matchState = matchState.second;
                list.Add(collectedItems[collectedItems.IndexOf(_item) - 1]);
            }
            else if (matchState == matchState.second && collectedItems.IndexOf(_item) != 1 && collectedItems[collectedItems.IndexOf(_item) - 2]._type == _item._type)
            {

                if (collectedItems.IndexOf(_item) != 2 && collectedItems[collectedItems.IndexOf(_item) - 3]._type == _item._type)
                {
                    list.Clear();
                    return list;
                }

                list.Add(collectedItems[collectedItems.IndexOf(_item) - 2]);
                return list;
            }
        }

        return list;
    }

    WaitForSeconds delay = new WaitForSeconds(.25f);

    private IEnumerator RemoveMatchedItems(List<Item> removeItems)
    {
        UIController.instance.lockBusters = true;
        UIController.instance.DisableAllBustersUnvisible();


        foreach (Item item in removeItems)
        {
            float f = 0;

            SpriteRenderer glow = Instantiate(ItemController.instance.itemCollectGlow, item.rectTransform.position, item.rectTransform.rotation, item.transform);

            glow.transform.position = new Vector3(glow.transform.position.x, glow.transform.position.y, glow.transform.position.z - 0.05f);

            Material material = glow.material;
            DOTween.To(() => f, x => f = x, 1, 0.25f).OnUpdate(() => material.SetFloat("_Alpha", f));

            item.rectTransform.DOScale(item.rectTransform.localScale * 1.25f, .25f).OnComplete(() =>
            {
                DOTween.To(() => f, x => f = x, 0, 0.1f).OnUpdate(() => material.SetFloat("_Alpha", f));

                float f2 = 1;
                DOTween.To(() => f2, x => f2 = x, 0, 0.1f).OnUpdate(() => item.material.SetFloat("_Alpha", f2));
            });
        }

        yield return delay;

        foreach (var item in removeItems)
        {
            if (collectedItems.Contains(item))
                collectedItems.Remove(item);
        }

        AudioManager.instance.Play("Match tiles");

        for (int i = 0; i < removeItems.Count; i++)
        {

            ParticleSystem particle = Instantiate(ItemController.instance.itemExplode, removeItems[i].rectTransform.position, removeItems[i].rectTransform.rotation, removeItems[i].transform.parent);
            particle.transform.position = new Vector3(particle.transform.position.x, particle.transform.position.y, particle.transform.position.z + 0.05f);

            particle.Play();
            removeItems[i].gameObject.SetActive(false);

            if (ItemController.instance.lastCollectedItems.Contains(removeItems[i]))
            {
                ItemController.instance.lastCollectedItems.Remove(removeItems[i]);
            }
        }

        BusterController.instance.CheckBustersOnItemRemove();
        BusterController.instance.CheckBustersOnItemDestroy();

        UIController.instance.lockBusters = false;
        UIController.instance.EnableAllBustersUnvisible();

        CheckItemSize();
        CorrectPlaces();
    }

    public void RemoveItem(List<Item> item)
    {

        foreach (Item item2 in item)
        {
            currentCollectCount--;
            collectedItems.Remove(item2);
        }

        CorrectPlaces();
    }
}