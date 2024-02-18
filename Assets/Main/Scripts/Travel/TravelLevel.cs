using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TravelLevel : MonoBehaviour
{
    public List<Sight> sights = new List<Sight>();

    private List<int> sightStatus = new List<int>();

    public Image sprite;

    [Header("Turkmen")]
    public string cityName;
    public Sprite cityCollectionImage;
    public Sprite defaultCityCollectionImage;

    public Image blurPanel;

    public void InitializeLevel()
    {
        //UIController.instance.CityName(cityName);
        UIController.instance.buildTxt.text = (TravelController.instance.currentTravelLevel + 1).ToString();

        for (int i = 0; i < sights.Count; i++)
        {
            sightStatus.Add(PlayerPrefs.GetInt("TravelSight" + i, 0));

            if (sightStatus[i] == 1)
            {
                for (int j = 0; j < sights[i].sight.Length; j++)
                {
                    sights[i].sight[j].gameObject.SetActive(true);
                    sights[i].sight[j].transform.localScale = Vector3.one;
                }
            }

            sights[i].openBtn.gameObject.SetActive(false);
            sights[i].openBtn.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = sights[i].price.ToString();
        }

        Color color = Color.white;
        color.a = 0;
        blurPanel.color = color;
    }

    public void OpenBuildMode()
    {
        for (int i = 0; i < sights.Count; i++)
        {
            if (sightStatus[i] == 0)
                sights[i].openBtn.gameObject.SetActive(true);
            /* if (sights[i].openBtn.transform.parent == sights[i].sight[0].transform)
             {
                 sights[i].openBtn.transform.SetParent(sights[i].sight[0].transform.parent);
                 sights[i].openEffect.transform.SetParent(sights[i].sight[0].transform.parent);
             }*/
        }
    }

    public void OpenSight(int sightIndex)
    {
        if (ItemController.instance.currentLvl == 3)
        {
            PlayerPrefs.SetInt("_OpenSightTut", 1);
            UIController.instance.HandList[2].GetComponent<SpriteRenderer>().DOFade(0.0f, 0.2f);
            UIController.instance.RulesList[2].DOScale(Vector3.zero, 0.2f);
            UIController.instance.RulesList[2].GetComponent<Image>().DOFade(0.0f, 0.2f).OnComplete(() =>
            {
                UIController.instance.TravelTutBackground.DOFade(0f, 0.35f).OnComplete(() =>
                {
                    UIController.instance.GamePlayTutorial.gameObject.SetActive(false);
                    UIController.instance.HandList[2].SetActive(false);
                });

            });
        }
        
        if (ResourcesData.instance._star < sights[sightIndex].price)
        {
            UIController.instance.OpenStarEarn();
            return;
        }
        else
            ResourcesData.instance.RemoveStar(sights[sightIndex].price);


        AudioManager.instance.Play("Decorate");

        sights[sightIndex].openBtn.gameObject.SetActive(false);

        sights[sightIndex].openEffect.Play();

        for (int i = 0; i < sights[sightIndex].sight.Length; i++)
        {
            sights[sightIndex].sight[i].transform.localScale = Vector3.zero;
            sights[sightIndex].sight[i].gameObject.SetActive(true);

            Transform tr = sights[sightIndex].sight[i].transform;
            //1.2f
            tr.DOScale(Vector3.one * 1.2f, 0.25f).OnComplete(() => tr.DOScale(Vector3.one, 0.1f));

            sightStatus[sightIndex] = 1;
            PlayerPrefs.SetInt("TravelSight" + sightIndex, 1);
        }

        Debug.Log("Sutayda");

        CheckForComplete();
    }

    private void CheckForComplete()
    {

        for (int i = 0; i < sightStatus.Count; i++)
        {
            if (sightStatus[i] == 0)
                return;
        }
        Debug.Log("Sutayda 2");
        TravelController.instance.SaveNewLevel();

        StartCoroutine(UIController.instance.NewTravelLevel());
    }

    public void Close()
    {
        for (int i = 0; i < sights.Count; i++)
        {
            sights[i].openBtn.gameObject.SetActive(false);
        }
    }

    public void HideOpenedSights()
    {
        for (int i = 0; i < sightStatus.Count; i++)
        {
            if (sightStatus[i] == 1)
            {
                for (int j = 0; j < sights[i].sight.Length; j++)
                {
                    Transform tr = sights[i].sight[j].transform;

                    tr.DOScale(Vector3.zero, 0.25f);
                }
            }

        }
    }

    public void ShowOpenedSights()
    {
        for (int i = 0; i < sightStatus.Count; i++)
        {
            if (sightStatus[i] == 1)
            {
                for (int j = 0; j < sights[i].sight.Length; j++)
                {
                    Transform tr = sights[i].sight[j].transform;
                    //1.05f
                    tr.DOScale(Vector3.one * 1.05f, 0.25f).OnComplete(() => tr.DOScale(Vector3.one, 0.25f));
                }
            }
        }
    }

    public void OpenForCollection()
    {
        for (int i = 0; i < sights.Count; i++)
        {
            for (int j = 0; j < sights[i].sight.Length; j++)
            {
                Transform tr = sights[i].sight[j].transform;

                tr.localScale = Vector3.zero;
                tr.gameObject.SetActive(true);
                //1.1f
                tr.DOScale(Vector3.one * 1.1f, 0.25f).OnComplete(() => tr.DOScale(Vector3.one, 0.25f));
            }

        }
    }

    public IEnumerator CloseForCollection()
    {
        for (int i = 0; i < sights.Count; i++)
        {
            for (int j = 0; j < sights[i].sight.Length; j++)
            {
                Transform tr = sights[i].sight[j].transform;

                tr.DOScale(Vector3.zero, 0.25f);
            }
        }

        yield return new WaitForSeconds(0.26f);

        Destroy(gameObject);
    }


}

[System.Serializable]
public class Sight
{
    public RectTransform[] sight;
    public Button openBtn;
    public ParticleSystem openEffect;
    public int price;
}