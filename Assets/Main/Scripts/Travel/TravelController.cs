using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TravelController : MonoBehaviour
{
    public static TravelController instance;

    public List<TravelLevel> travelLevels = new List<TravelLevel>();

    public List<GameObject> landscapePrefab;
    public List<GameObject> portraitPrefab;

    [SerializeField] Transform background;

    private TravelLevel currentLevel;

    [HideInInspector]
    public int currentTravelLevel;

    public int custumLevelIndex;
    public bool custumLevel;

    private void Awake()
    {
        instance = this;
    }

    public void TravelLevelStart()
    {
        if (currentLevel is not null) 
            Destroy(currentLevel.gameObject);
        currentTravelLevel = PlayerPrefs.GetInt("TravelLvl", 0);

        if (custumLevel)
            currentTravelLevel = custumLevelIndex;

        currentLevel = Instantiate(travelLevels[currentTravelLevel], background);
        currentLevel.InitializeLevel();

        TravelCollection.instance.Initialize(currentTravelLevel, travelLevels.Count);
    }

    public void Build()
    {
        currentLevel.OpenBuildMode();
    }

    public void Close()
    {
        currentLevel.Close();
    }

    public void NewTravelLevel()
    {

        Destroy(currentLevel.gameObject);

        currentLevel = Instantiate(travelLevels[currentTravelLevel], background);
        currentLevel.InitializeLevel();
    }

    public void SaveNewLevel()
    {
        currentTravelLevel++;
        Debug.Log(currentLevel);
        if (currentTravelLevel == travelLevels.Count)
            currentTravelLevel = 0;
        PlayerPrefs.SetInt("TravelLvl", currentTravelLevel);
        Debug.Log(currentLevel + "   " + travelLevels.Count);
        for (int i = 0; i < travelLevels[currentTravelLevel].sights.Count; i++)
        {
            PlayerPrefs.SetInt("TravelSight" + i, 0);
        }
    }

    public void BlurBackground()
    {
        currentLevel.blurPanel.DOFade(1, 0.5f);

        currentLevel.HideOpenedSights();

    }

    public void DisBlurBackground()
    {
        currentLevel.blurPanel.DOFade(0, 0.5f);

        currentLevel.ShowOpenedSights();
    }

    public void ChangeTravelLevels(List<GameObject> travelLevelsNew)
    {
        for (int i = 0; i < travelLevels.Count; i++)
        {
            travelLevels[i] = travelLevelsNew[i].GetComponent<TravelLevel>();
        }
    }
}