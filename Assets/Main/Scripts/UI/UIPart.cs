using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPart : MonoBehaviour
{
    public Vector2 startPos;
    public Vector3 scale;
    public float upLength;

    public RectTransform rectTransform;

    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        rectTransform = GetComponent<RectTransform>();

        startPos = rectTransform.anchoredPosition;
    }
}