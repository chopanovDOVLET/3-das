using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class ShopController : MonoBehaviour
{
    public static ShopController Instance;

    [SerializeField] Transform shopPanel;
    [SerializeField] Transform bundlePack;
    [SerializeField] Transform shopPackBox;
    [SerializeField] Transform shopBtn;
    [SerializeField] Transform closeBtn;

    private void Awake()
    {
        Instance = this;
    }
    
    
    public void OpenShopPanel()
    {
        AudioManager.instance.Play("Button");
        shopBtn.transform.DOScale(Vector3.zero, 0.35f);
        
        shopPanel.gameObject.SetActive(true);
        closeBtn.DOScale(Vector3.one, 0.35f).From(0);
        bundlePack.GetComponent<Image>().DOFade(1, .2f).From(0);
        bundlePack.DOScale(Vector3.one, 0.2f).From(0);

        float sec = 0;
        foreach (Transform pack in shopPackBox)
        {
            pack.DOScale(Vector3.one, 0.2f).From(0).SetDelay(sec);
            pack.GetComponent<Image>().DOFade(1, .2f).From(0).SetDelay(sec);
            sec += 0.03f;
        }
        
        UIController.instance.HidePlayTut();
    }
    
    public void CloseShopPanel()
    {
        AudioManager.instance.Play("Button");
        shopBtn.transform.DOScale(Vector3.one, 0.35f);

        closeBtn.DOScale(Vector3.zero, 0.35f);
        bundlePack.GetComponent<Image>().DOFade(0, .15f);
        bundlePack.DOScale(Vector3.zero, 0.15f);

        float sec = 0;
        foreach (Transform pack in shopPackBox)
        {
            pack.DOScale(Vector3.zero, .15f).SetDelay(sec);
            pack.GetComponent<Image>().DOFade(0, .15f).SetDelay(sec);
            sec += 0.03f;
        }
        shopPanel.GetComponent<Image>().DOFade(.98f, 0).SetDelay(.25f).OnComplete(() => shopPanel.gameObject.SetActive(false));
        
        StartCoroutine(UIController.instance.PlayTut());
    }
    
}
