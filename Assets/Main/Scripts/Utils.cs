using Mono.CSharp;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class Utils
{
    public static void CopyRectTransform(Transform target, Transform source)
    {
        if (target.tag == "Uncheck")
        {
            return;
        }
        
        if (target.TryGetComponent<UIPart>(out var uiPart))
        {
            source.TryGetComponent<UIPart>(out var uiPart2);
            uiPart.startPos = uiPart2.startPos;
            uiPart.scale = uiPart2.scale;
            uiPart.upLength = uiPart2.upLength;
        }
        
        if (target.TryGetComponent<Transform>(out var transform))
        {
            source.TryGetComponent<Transform>(out var transform2);
            transform.localPosition = transform2.localPosition;
            transform.localScale = transform2.localScale;
            transform.localRotation = transform2.localRotation;
        }
        
        if (target.TryGetComponent<RectTransform>(out var targetRect))
        {
            source.TryGetComponent<RectTransform>(out var sourceRect);
            targetRect.localPosition = sourceRect.localPosition;
            targetRect.localRotation = sourceRect.localRotation;
            targetRect.localScale = sourceRect.localScale;
            targetRect.anchorMin = sourceRect.anchorMin;
            targetRect.anchorMax = sourceRect.anchorMax;
            targetRect.anchoredPosition = sourceRect.anchoredPosition;
            targetRect.sizeDelta = sourceRect.sizeDelta;
            targetRect.pivot = sourceRect.pivot;
        }

        if (target.childCount <= source.childCount)
            for (int i = 0; i < target.childCount; i++)
            {
                CopyRectTransform(target.GetChild(i), source.GetChild(i));
            }
        else if (target.childCount > source.childCount)
            for (int i = 0; i < source.childCount; i++)
            {
                CopyRectTransform(target.GetChild(i), source.GetChild(i));
            }
    }
}