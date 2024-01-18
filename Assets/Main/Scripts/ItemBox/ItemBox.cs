using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using TMPro;
using ExtensionMethods;
using static ItemData;

public class ItemBox : MonoBehaviour
{
    public List<Item> boxedItems = new List<Item>();

    public List<SortingGroup> groups = new List<SortingGroup>();

    public List<SpriteRenderer> sprites = new List<SpriteRenderer>();

    public List<Item> frontItems = new List<Item>();
    public List<Item> backItems = new List<Item>();

    public Item lastCollected;
    public Item underItem;
    public Animation openAnimation;
    public Animation glowAnimation;
    public TextMeshProUGUI textNumber;

    [SerializeField] ParticleSystem expParticle;

    [SerializeField] Transform vib;

    private RectTransform rect;

    private Vector3 startScale;

    private bool mustUnBox = false;

    [HideInInspector]
    public bool active, isDisabled = false;

    public void InitializeBox()
    {
        foreach (var item in boxedItems)
        {
            item.gameObject.SetActive(true);

            item.GetComponent<SortingGroup>().sortingOrder = 0;
            item.rectTransform.localScale = Vector3.one * 3;
            item.gameObject.SetActive(false);
        }

        startScale = transform.localScale;

        textNumber.text = boxedItems.Count.ToString();
        rect = GetComponent<RectTransform>();

        glowAnimation.Play();

        CheckState();
    }

    public void ReturnLastUnboxed(Item item)
    {
        if (lastCollected != null && lastCollected == item)
        {

            if (transform.localScale == Vector3.zero)
            {
                groups[0].sortingOrder = 3;
                groups[1].sortingOrder = 1;

                isDisabled = false;
                ItemController.instance.CheckAllItems();
                transform.DOScale(startScale, 0.25f);
            }

            openAnimation.Play("Box");

            UIController.instance.lockBusters = true;
            UIController.instance.DisableAllBustersUnvisible();

            Vector3 pos = new Vector3(rect.anchoredPosition3D.x, rect.anchoredPosition3D.y, rect.anchoredPosition3D.z);
            Item box = underItem;

            ItemController.instance.items.Remove(box);
            box.DisableItem();

            List<Item> list = new List<Item>();

            foreach (var items in boxedItems)
            {
                list.Add(items);
            }

            boxedItems.Clear();

            boxedItems.Add(box);
            boxedItems.AddRange(list);

            box.GetComponent<SortingGroup>().sortingOrder = 2;

            StartCoroutine(BoxAnim(box, item, pos));

        }
    }

    private void DisableBox()
    {
        foreach (var item in groups)
        {
            item.sortingOrder = 5;
        }

        rect.DOScale(rect.localScale * 1.2f, 0.5f).SetEase(Ease.InOutQuad).OnComplete(() =>
        {

            ParticleSystem particle = Instantiate(expParticle, rect.position, rect.rotation);
            particle.Play();

            AudioManager.instance.Play("Box Pop");

            underItem.GetComponent<SortingGroup>().sortingOrder = 4;
            transform.localScale = Vector3.zero;
            isDisabled = true;

            foreach (var item in backItems)
            {
                ItemController.instance.CheckItemFrontWithRay(item);
            }

            ItemController.instance.CheckAllItems();
        });
    }

    public void UnBox(Item item)
    {
        if (item != null && item != underItem && !mustUnBox)
            return;

        if (boxedItems.Count == 0)
            return;

        underItem = null;

        if (!active)
        {
            mustUnBox = true;
            return;
        }

        mustUnBox = false;

        UIController.instance.lockBusters = true;
        UIController.instance.DisableAllBustersUnvisible();

        lastCollected = item;

        openAnimation.Play("UnBox");

        Vector3 pos = new Vector3(rect.anchoredPosition3D.x, rect.anchoredPosition3D.y - 106.5f, rect.anchoredPosition3D.z);
        Item unBox = boxedItems[0];
        boxedItems.Remove(unBox);

        unBox.gameObject.SetActive(true);

        textNumber.text = boxedItems.Count.ToString();

        unBox.GetComponent<SortingGroup>().sortingOrder = 2;
        unBox.DisableItem();

        StartCoroutine(UnBoxAnim(unBox, pos));
    }

    IEnumerator UnBoxAnim(Item unBox, Vector3 pos)
    {
        yield return delay;  

        unBox.rectTransform.DOScale(5.8f, 0.75f);
        unBox.rectTransform.DOAnchorPos3D(pos, 0.75f).OnComplete(() =>
        {
            unBox.InitializeItem();
            unBox.EnableItem();

            if (boxedItems.Count == 0)
            {
                unBox.GetComponent<SortingGroup>().sortingOrder = 5;
                DisableBox();
            }

            UIController.instance.lockBusters = false;
            UIController.instance.EnableAllBustersUnvisible();
            BusterController.instance.CheckBustersOnItemIncrease();
        });

        yield return delay2;

        AudioManager.instance.Play("Unbox");

        unBox.GetComponent<SortingGroup>().sortingOrder = 4;

        underItem = unBox;

    }

    WaitForSeconds delay = new WaitForSeconds(.1f);
    WaitForSeconds delay2 = new WaitForSeconds(.5f);

    IEnumerator BoxAnim(Item unBox, Item item, Vector3 pos)
    {
        yield return delay;

        textNumber.text = boxedItems.Count.ToString();

        unBox.rectTransform.DOScale(Vector3.one * 3, 0.5f);
        unBox.rectTransform.DOAnchorPos3D(pos, 0.5f).OnComplete(() =>
        {
            unBox.GetComponent<SortingGroup>().sortingOrder = 0;

            UIController.instance.lockBusters = false;
            UIController.instance.EnableAllBustersUnvisible();
            BusterController.instance.CheckBustersOnItemDestroy();

            unBox.gameObject.SetActive(false);
        });

        underItem = item;
    }

    public void CheckState()
    {
        if (frontItems.Count == 0 || frontItems.IsAllItemsEqual(ItemState.Collected))
            Active();
        else
            Inactive();
    }

    public void Active()
    {
        active = true;

        if (mustUnBox)
            UnBox(null);

        SmoothOpenColor();
    }

    public void Inactive()
    {
        active = false;
        SmoothCloseColor();
    }

    public void SmoothOpenColor()
    {

        foreach (var sprite in sprites)
        {
            sprite.DOColor(Color.white, 0.5f);
        }

        textNumber.DOColor(Color.white, 0.5f);
    }

    public void SmoothCloseColor()
    {
        if (frontItems.Count != 0 && frontItems.AtLeastOneNotEqual(ItemState.Collected))
        {
            foreach (var sprite in sprites)
            {
                sprite.DOColor(Color.gray, 0.5f);
            }

            textNumber.DOColor(Color.gray, 0.5f);
        }
        else
            return;
    }

    public void ResetFrontItems()
    {
        frontItems.Clear();
        backItems.Clear();
    }

    public void AddFrontItem(Item item)
    {
        frontItems.Add(item);
    }

    public void AddBackItem(Item item)
    {
        backItems.Add(item);
    }

    public void Vibrate()
    {
        if (active)
            vib.DOScaleY(1.1f, 0.1f).OnComplete(() => vib.DOScaleY(1f, 0.1f));
    }

    float m_MaxDistance = 5;

    Collider m_Collider;
    RaycastHit[] m_Hit;

    public List<Item> CheckRadius()
    {

        m_Collider = GetComponent<Collider>();
        m_Hit = Physics.BoxCastAll(transform.position, m_Collider.bounds.size / 2.05f, Vector3.back, transform.rotation, m_MaxDistance);

        List<Item> hits = new List<Item>();

        foreach (var item in m_Hit)
        {
            hits.Add(item.collider.gameObject.GetComponent<Item>());
        }

        for (int i = 0; i < hits.Count; i++)
        {
            Item hit = hits[i];

            if (hit._state == ItemState.Collected)
                hits.RemoveAt(i);
            else if (Mathf.Abs(hit.rectTransform.anchoredPosition3D.z - rect.anchoredPosition3D.z) <= 5)
                hits.RemoveAt(i);
        }

        return hits;
    }

}