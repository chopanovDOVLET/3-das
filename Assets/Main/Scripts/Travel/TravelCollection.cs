
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TravelCollection : MonoBehaviour
{
    public static TravelCollection instance;

    [SerializeField] TownUICollection town;
    [SerializeField] RectTransform sizeFitter;
    
    [SerializeField] GameObject leaderboard;
    [SerializeField] RectTransform leaderboardBtn;
    
    [SerializeField] GameObject settings;
    [SerializeField] RectTransform settingsBtn;
    [SerializeField] GameObject collection;
    [SerializeField] Transform background;
    
    private List<TownUICollection> towns = new List<TownUICollection>();

    private TravelLevel level;


    public Sprite defeaultImageArka;
    public Sprite defeaultImageBackground;
     
    public Sprite activeImageArka;
    public Sprite activeImageBackground;


    private void Awake()
    {
        instance = this;
    }

    public void Initialize(int travelLevel, int townCount)
    {   

        if(towns.Count != 0) {
        for(int i = 0; i < towns.Count; i++)
            {
                Destroy(towns[i].gameObject);
            }
            towns.Clear();
        }
        
        for (int i = 0; i < townCount; i++)
        {
            TownUICollection newTown = Instantiate(town, Vector3.zero, Quaternion.identity, sizeFitter);

            newTown.townID = i;
            newTown.townName.text = TravelController.instance.travelLevels[i].cityName;


            newTown.button.onClick.AddListener(delegate { OpenTown(newTown); });

            if (i >= travelLevel)
            {
                newTown.button.interactable = false;
                newTown.isBlocked = true;
                //newTown.blockedImage.gameObject.SetActive(true);
                newTown.BackgroundRectangle.sprite = defeaultImageBackground;
                newTown.ArkaImage.sprite = defeaultImageArka;
                newTown.townImage.sprite = TravelController.instance.travelLevels[i].defaultCityCollectionImage;
            }
            else
            {
                newTown.button.interactable = true;
                newTown.ViewBTN.SetActive(true);
                newTown.BackgroundRectangle.sprite = activeImageBackground;
                newTown.ArkaImage.sprite = activeImageArka;
                newTown.townImage.sprite = TravelController.instance.travelLevels[i].cityCollectionImage;
            }
            towns.Add(newTown);
        }
    }

    public void OpenTown(TownUICollection town)
    {
        background.gameObject.SetActive(true);
        level = Instantiate(TravelController.instance.travelLevels[town.townID], background);

        level.sprite.material = null;
        level.OpenForCollection();

        level.transform.SetSiblingIndex(0);

    }

    public void CloseTown()
    {
        StartCoroutine(level.CloseForCollection());
        level.transform.SetParent(level.transform.parent.parent);
        background.gameObject.SetActive(false);
    }

    public void OpenTownCollection()
    {
        sizeFitter.anchoredPosition = Vector2.zero;
        AudioManager.instance.Play("Button");
        collection.SetActive(true);
    }

    public void CloseTownCollection()
    {
        AudioManager.instance.Play("Button");
        collection.SetActive(false);
    }

    public void OpenSettings()
    {
        sizeFitter.anchoredPosition = Vector2.zero;
        settingsBtn.DOScale(Vector3.zero, 0.5f);
        AudioManager.instance.Play("Button");
        settings.SetActive(true);
    }

    public void CloseSettings()
    {
        AudioManager.instance.Play("Button");
        settingsBtn.DOScale(Vector3.one, 0.5f);
        settings.SetActive(false);
    }
    
    public void OpenLeaderboard()
    {
        leaderboardBtn.DOScale(Vector3.zero, 0.5f);
        AudioManager.instance.Play("Button");
        leaderboard.SetActive(true);
    }

    public void CloseLeaderboard()
    {
        AudioManager.instance.Play("Button");
        leaderboardBtn.DOScale(Vector3.one, 0.5f);
        leaderboard.SetActive(false);
        DisplayHighscores.Instance.done = false;
        StartCoroutine(DisplayHighscores.Instance.RefreshHighscores());
    }
}