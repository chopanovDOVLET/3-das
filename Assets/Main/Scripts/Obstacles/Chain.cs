using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chain : MonoBehaviour, IObstacle
{
    public ParticleSystem explode;
    public SpriteRenderer _icon;
    public RectTransform rectTransform;

    [HideInInspector]
    public Vector3 startPos;

    public void Initialize()
    {
        startPos = rectTransform.position;
    }

    public void SwitchItemState(Item item)
    {
        item.SwitchState(item.chainState);
    }

    public void SwitchItem(Item item)
    {
        transform.SetParent(item.transform);
        transform.position = new Vector3(item.transform.position.x, item.transform.position.y, item.transform.position.z - 0.01f);
    }

    public bool Remove()
    {
        ParticleSystem exp = Instantiate(explode, transform.position, explode.transform.rotation, transform);
        exp.Play();

        exp.transform.parent = null;

        gameObject.SetActive(false);

        return false;
    }
    
    public void Active()
    {
        _icon.color = Color.white;
    }

    public void Inactive()
    {
        _icon.color = Color.gray;
    }

    public void SmoothActive()
    {
       
        _icon.DOColor(Color.white, 0.25f);
    }

    public void SmoothInactive()
    {
        _icon.DOColor(Color.gray, 0.25f);
    }

    public void Hide()
    {
        _icon.DOFade(0, 0.1f);
    }

    public void Show()
    {
        _icon.DOFade(1, 0.25f);
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public RectTransform GetRectTransform()
    {
        return rectTransform;
    }
}