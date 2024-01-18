using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPart : MonoBehaviour
{
    public Vector2 startPos;

    public RectTransform rectTransform;

    private void Awake()
    {
        Initialize();
    }


    public void Initialize()
    {
        rectTransform = GetComponent<RectTransform>();

        startPos = rectTransform.anchoredPosition;
    }
}