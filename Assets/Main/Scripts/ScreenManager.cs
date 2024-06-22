using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ScreenManager : MonoBehaviour
{
    public static ScreenManager Instance;
    public ScreenOrieantation _currentOrientation = ScreenOrieantation.None;
    public Action<ScreenOrieantation> OnScreenChange;

    [Header("Screen Resolutions")]
    public CanvasData baseCanvas;
    public CanvasData LandscapeCanvas;
    public CanvasData PortraitCanvas;

    private float oldWidth;
    private float oldHeight;

    private void Awake()
    {
        Instance = this;
    }
    
    void Start()
    {
        OnScreenChange += ChangeCanvasScaler;
        oldWidth = Screen.width;
        oldHeight = Screen.height;

        StartCoroutine(UIController.instance.LoadingPanelOnStart());
    }

    void Update()
    {
        if (Screen.width > Screen.height)
        {
            // if (Screen.width != oldWidth)
            // {
            //     oldWidth = Screen.width;
            //     OnScreenChange?.Invoke(_currentOrientation);
            //     return;
            // }
            //
            // if (_currentOrientation != ScreenOrieantation.Landscape)
            // {
            //     TravelController.instance.ChangeTravelLevels(TravelController.instance.landscapePrefab);
            //     TravelController.instance.TravelLevelStart();
            //     
            //     _currentOrientation = ScreenOrieantation.Landscape;
            //
            //     OnScreenChange?.Invoke(_currentOrientation);
            //     Utils.CopyRectTransform(baseCanvas.background, LandscapeCanvas.background);
            //     Utils.CopyRectTransform(baseCanvas.gamePlay, LandscapeCanvas.gamePlay);
            //     Utils.CopyRectTransform(baseCanvas.forwardUI, LandscapeCanvas.forwardUI);
            // }
        }
        else
        {
            if (Screen.height != oldHeight)
            {
                oldHeight = Screen.height;
                OnScreenChange?.Invoke(_currentOrientation);
                return;
            }
            
            if (_currentOrientation != ScreenOrieantation.Portrait)
            {
                TravelController.instance.ChangeTravelLevels(TravelController.instance.portraitPrefab);
                TravelController.instance.TravelLevelStart();
                
                _currentOrientation = ScreenOrieantation.Portrait;

                OnScreenChange?.Invoke(_currentOrientation);
                Utils.CopyRectTransform(baseCanvas.background, PortraitCanvas.background);
                Utils.CopyRectTransform(baseCanvas.gamePlay, PortraitCanvas.gamePlay);
                Utils.CopyRectTransform(baseCanvas.forwardUI, PortraitCanvas.forwardUI);
            }
        }
    }

    private void ChangeCanvasScaler(ScreenOrieantation screenOrintation)
    {
        if (screenOrintation == ScreenOrieantation.Portrait)
        {
            baseCanvas.background.GetComponent<Canvas>().planeDistance = 6f;
            baseCanvas.background.GetComponent<Canvas>().planeDistance = 6f;
            baseCanvas.background.GetComponent<Canvas>().planeDistance = 6f;
            
            baseCanvas.background.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(1080, 2400);
            baseCanvas.gamePlay.GetChild(1).GetComponent<RectTransform>().sizeDelta = new Vector2(1080, 2400);
            baseCanvas.forwardUI.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(1080, 2400);
            
            float scaleFactor = ((Screen.width / 1080f) > (Screen.height / 2400f))
                ? (Screen.height / 2400f)
                : (Screen.width / 1080f);
            
            baseCanvas.background.GetComponent<CanvasScaler>().scaleFactor = scaleFactor;
            baseCanvas.gamePlay.GetComponent<CanvasScaler>().scaleFactor = scaleFactor;
            baseCanvas.forwardUI.GetComponent<CanvasScaler>().scaleFactor = scaleFactor;
        }
        else
        {
            baseCanvas.background.GetComponent<Canvas>().planeDistance = 0.3f;
            baseCanvas.background.GetComponent<Canvas>().planeDistance = 0.3f;
            baseCanvas.background.GetComponent<Canvas>().planeDistance = 0.3f;
            
            baseCanvas.background.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(1920, 1080);
            baseCanvas.gamePlay.GetChild(1).GetComponent<RectTransform>().sizeDelta = new Vector2(1920, 1080);
            baseCanvas.forwardUI.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(1920, 1080);

            float scaleFactor = ((Screen.width / 1920f) > (Screen.height / 1080f))
                ? (Screen.height / 1080f)
                : (Screen.width / 1920f);
            
            baseCanvas.background.GetComponent<CanvasScaler>().scaleFactor = scaleFactor;
            baseCanvas.gamePlay.GetComponent<CanvasScaler>().scaleFactor = scaleFactor;
            baseCanvas.forwardUI.GetComponent<CanvasScaler>().scaleFactor = scaleFactor;
        }
    }
}

public enum ScreenOrieantation
{
    None,
    Landscape,
    Portrait
}

[Serializable]
public class CanvasData
{
    public Transform background;
    public Transform gamePlay;
    public Transform forwardUI;
}