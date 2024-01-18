using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Freeze : MonoBehaviour, IObstacle
{
    public ParticleSystem[] explode;
    public SpriteRenderer[] _icon;
    public RectTransform rectTransform;

    [HideInInspector]
    public Vector3 startPos;

    private int currentPart;


    public void Initialize()
    {
        startPos = rectTransform.position;
    }

    public void SwitchItemState(Item item)
    {
        item.SwitchState(item.freezeState);
    }

    public void SwitchItem(Item item)
    {
        transform.SetParent(item.transform);
        transform.position = new Vector3(item.transform.position.x, item.transform.position.y, item.transform.position.z - 0.01f);
    }

    public bool Remove()
    {
        _icon[currentPart].gameObject.SetActive(false);
        ParticleSystem exp = Instantiate(explode[currentPart], transform.position, explode[currentPart].transform.rotation, transform.parent);
        exp.Play();

        exp.transform.parent = null;

        currentPart++;

        if (currentPart == _icon.Length)
            return true;
        else
            return false;
    }

    public void Active()
    {
        for (int i = 0; i < _icon.Length; i++)
        {
            _icon[i].color = Color.white;
        }
    }

    public void Inactive()
    {
        for (int i = 0; i < _icon.Length; i++)
        {
            _icon[i].color = Color.gray;
        }
    }

    public void SmoothActive()
    {
        for (int i = 0; i < _icon.Length; i++)
        {
            _icon[i].DOColor(Color.white, 0.25f);
        }
    }

    public void SmoothInactive()
    {
        for (int i = 0; i < _icon.Length; i++)
        {
            _icon[i].DOColor(Color.gray, 0.25f);
        }
    }

    public void Hide()
    {
        for (int i = 0; i < _icon.Length; i++)
        {
            _icon[i].DOFade(0, 0.1f);
        }
    }

    public void Show()
    {
        for (int i = 0; i < _icon.Length; i++)
        {
            _icon[i].DOFade(1, 0.25f);
        }
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