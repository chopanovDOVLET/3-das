using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObstacle
{

    public void Initialize();

    public bool Remove();

    public void Hide();

    public void Show();

    public void Active();
    public void Inactive();

    public void SmoothActive();

    public void SmoothInactive();

    public void SwitchItemState(Item item);

    public void SwitchItem(Item item);

    public Transform GetTransform();

    public RectTransform GetRectTransform();

}