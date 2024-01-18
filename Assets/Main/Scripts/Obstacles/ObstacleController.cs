using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    public static ObstacleController instance;

    [Header("Obstacles")]
    public Gum gum;
    public Chain chain;
    public Freeze freeze;

    private void Awake()
    {
        instance = this;
    }

    public IObstacle InitializeGum(Item item)
    {
        Gum newGum = Instantiate(gum, item.transform.position, gum.transform.rotation, item.transform);

        newGum.transform.position = new Vector3(newGum.transform.position.x, newGum.transform.position.y, newGum.transform.position.z - 0.01f);

        RectTransform rect = newGum.rectTransform;
        rect.anchoredPosition = new Vector2(rect.anchoredPosition.x + 10.5f, rect.anchoredPosition.y);

        newGum.Initialize();

        return newGum.GetComponent<IObstacle>();
    }

    public IObstacle InitializeChain(Item item)
    {
        Chain newChain = Instantiate(chain, item.transform.position, chain.transform.rotation, item.transform);

        newChain.transform.position = new Vector3(newChain.transform.position.x, newChain.transform.position.y, newChain.transform.position.z - 0.01f);

        newChain.Initialize();

        return newChain.GetComponent<IObstacle>();
    }

    public IObstacle InitializeFreeze(Item item)
    {
        Freeze newFreeze = Instantiate(freeze, item.transform.position, freeze.transform.rotation, item.transform);

        newFreeze.transform.position = new Vector3(newFreeze.transform.position.x, newFreeze.transform.position.y, newFreeze.transform.position.z - 0.01f);

        newFreeze.Initialize();

        return newFreeze.GetComponent<IObstacle>();
    }

}