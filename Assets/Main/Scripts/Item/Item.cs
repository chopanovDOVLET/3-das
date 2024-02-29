using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using ExtensionMethods;
using UnityEngine.Rendering;
using MoreMountains.NiceVibrations;
using UnityEngine.UIElements;

public class Item : ItemData
{
    public Type _type;
    public ItemState _state;
    public EObstacle _obstacle;

    [Header("Side Items")]
    [LabeledArray(new string[] { "Front", "Back", "Left", "Right", "Up", "Down" })]
    public ItemSides[] itemSides = new ItemSides[6];

    public ItemBox boxFront;
    public ItemBox boxBack;

    [Header("Other Compoments")]
    public SpriteRenderer _icon;
    [SerializeField] EventTrigger _trigger;
    public RectTransform rectTransform;
    public ItemController controller;
    public Animation _anim;
    public SortingGroup sortingGroup;

    [HideInInspector]
    public Level currentLevel;

    [Header("Obstacle")]
    [HideInInspector]
    public Item gumSide;
    [HideInInspector]
    public bool isMainGumSide = false;

    [HideInInspector]
    public Vector3 startPos;
    public Vector3 startScale;

    [HideInInspector]
    public bool isPointerOn;
    [HideInInspector]
    public Tweener tweener;

    [HideInInspector]
    public int currentTileIndex;

    BaseState currentState;
    public MainState mainState = new MainState();
    public GumState gumState = new GumState();
    public ChainState chainState = new ChainState();
    public FreezeState freezeState = new FreezeState();

    [HideInInspector]
    public List<Item> collect = new List<Item>();

    public IObstacle obstacle;

    [HideInInspector]
    public Material material;

    public void InitializeItem()
    {
        if (_obstacle == EObstacle.None)
            currentState = mainState;
        else if (_obstacle == EObstacle.Gum)
            currentState = gumState;
        else if (_obstacle == EObstacle.Chain)
            currentState = chainState;
        else if (_obstacle == EObstacle.Freeze)
            currentState = freezeState;

        material = _icon.material;

        _trigger.enabled = false;
        _icon.sprite = ItemController.instance.ApplyItem(this);

        currentState.InitializeItem(this);

        if (boxBack != null)
            boxBack.AddFrontItem(this);

        if (boxFront != null)
            boxFront.AddBackItem(this);

        CheckState();
        SaveStartPos();

    }

    public void SaveStartPos()
    {
        if (isMainGumSide)
        {
            rectTransform.anchoredPosition3D = new Vector3(rectTransform.anchoredPosition3D.x, rectTransform.anchoredPosition3D.y, rectTransform.anchoredPosition3D.z - 2);
        }

        startPos = rectTransform.position;
        startScale = rectTransform.localScale;
        if (itemSides[0].items.Count == 0 || itemSides[0].items.IsAllItemsEqual(ItemState.Collected))
            EnableItem();
        else
            DisableItem();
    }

    public void CheckState()
    {
        currentState.CheckState(this);
    }

    public void CheckOnCollect()
    {
        currentState.CheckOnCollect(this);
    }

    public void ActiveItem()
    {
        currentState.ActiveItem(this);
    }

    public void InactiveItem()
    {
        currentState.InactiveItem(this);
    }

    #region IPointer

    public void ItemUp()
    {
        currentState.ItemUp(this);
    }

    public void ItemDown()
    {
        currentState.ItemDown(this);
    }

    public void PointerEnter()
    {
        isPointerOn = true;
    }

    public void PointerExit()
    {
        isPointerOn = false;
    }

    public void ItemCollect()
    {
        currentState.ItemCollect(this);
    }

    public void ItemDisCollect()
    {
        _state = ItemState.Active;

        tweener.Kill();
    }

    #endregion

    public void SmoothOpenColor()
    {
        currentState.SmoothOpenColor(this);
    }

    public void SmoothCloseColor()
    {
        currentState.SmoothCloseColor(this);
    }

    public void RemoveObstacle()
    {
        currentState.RemoveObstacle(this);
    }

    public void DisableItem()
    {
        _trigger.enabled = false;
    }

    public void EnableItem()
    {
        if (_state == ItemState.Active && (itemSides[0].items.Count == 0 || itemSides[0].items.IsAllItemsEqual(ItemState.Collected)))
        {
            _trigger.enabled = true;
        }
    }

    public void ResetProperties()
    {
        for (int i = 0; i < itemSides.Length; i++)
        {
            itemSides[i].items.Clear();
        }
    }

    public void SwitchState(BaseState newState)
    {
        currentState = newState;
    }

    public void MoveUp()
    {
        tweener = rectTransform.DOMoveY(startPos.y + 0.05f, 0.2f);
    }

    public void MoveDown()
    {
        tweener = rectTransform.DOMoveY(startPos.y, 0.2f);
    }

    public void MagicMerge()
    {
        BusterController.instance.MagicEnd();
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
            if (item.collider.gameObject.GetComponent<Item>())
                hits.Add(item.collider.gameObject.GetComponent<Item>());
        }

        if (hits.Contains(this))
            hits.Remove(this);

        for (int i = 0; i < hits.Count; i++)
        {
            Item hit = hits[i];

            if (hit._state == ItemState.Collected)
                hits.RemoveAt(i);
            else if (Mathf.Abs(hit.rectTransform.anchoredPosition3D.z - rectTransform.anchoredPosition3D.z) <= 5)
                hits.RemoveAt(i);
        }

        return hits;
    }

    public void Vibrate()
    {
        if (!SettingButton.Instance.vibrationOn) MMVibrationManager.Haptic(HapticTypes.LightImpact);
        rectTransform.DOMoveX(startPos.x - 0.0125f, 0.025f).OnComplete(() => rectTransform.DOMoveX(startPos.x + 0.0125f, 0.05f).OnComplete(() => rectTransform.DOMoveX(startPos.x - 0.0125f, 0.05f).OnComplete(() => rectTransform.DOMoveX(startPos.x + 0.0125f, 0.05f).OnComplete(() => rectTransform.DOMoveX(startPos.x, 0.025f)))));
    }
}

[System.Serializable]
public class ItemSides
{
    public List<Item> items = new List<Item>();
}