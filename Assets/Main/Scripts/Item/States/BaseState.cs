using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState
{

    public abstract void InitializeItem(Item item);

    public abstract void ActiveItem(Item item);

    public abstract void InactiveItem(Item item);

    public abstract void ItemUp(Item item);

    public abstract void ItemDown(Item item);

    public abstract void ItemCollect(Item item);

    public abstract void SmoothOpenColor(Item item);

    public abstract void SmoothCloseColor(Item item);

    public abstract void CheckState(Item item);

    public abstract void CheckOnCollect(Item item);

    public abstract void RemoveObstacle(Item item);

}