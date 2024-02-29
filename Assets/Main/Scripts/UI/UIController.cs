using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using DG.Tweening;
using Febucci.Attributes;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Video;
using UnityEngine.XR;

public class UIController : MonoBehaviour
{
    public static UIController instance;

    [Header("Buttons")]
    public UIPart playBtn;
    public UIPart buildBtn;
    public RectTransform buildBackBtn;
    public Transform ExitButton;
    public UIPart collectionBtn;

    [Header("Busters Buttons")]
    public Button undo;
    public Button mix;
    public Button returnTile;
    public Button magic;
    public Button extraPlace;

    [Header("Busters")]
    public List<Image> undoImages;
    public List<Image> mixImages;
    public List<Image> returnTileImages;
    public List<Image> magicImages;
    public List<Image> extraPlaceImages;
    public TextMeshProUGUI undoSize;
    public TextMeshProUGUI mixSize;
    public TextMeshProUGUI returnTileSize;
    public TextMeshProUGUI magicSize;
    public TextMeshProUGUI extraPlaceSize;

    [Header("UI Parts")]
    public UIPart mainGameDownSide;
    public UIPart Items;
    public UIPart hubUpSide;
    public TextMeshProUGUI levelTxt, buildTxt, levelIndexText;

    [Header("Tutorials")]
    public GameObject GamePlayTutorial;
    public SpriteRenderer MainTutBackground;
    public Image TravelTutBackground;
    [HideInInspector] public Transform Rule;
    public List<Transform> RulesList = new List<Transform>();
    [HideInInspector] public GameObject Hand;
    public List<GameObject> HandList = new List<GameObject>();

    [Header("Panels")]
    public GameObject MainGamePanel;
    public RectTransform collectorRect;
    public RawImage tile;

    public int minScale, maxScale;
    public float minTile, maxTile;

    [Header("Build")]
    [SerializeField] RectTransform cityName;
    [SerializeField] Image cityNameTxt;
    [SerializeField] Image TownCompletedPanel;
    [SerializeField] RectTransform TownCompleted;
    [SerializeField] RectTransform continueTravelPanel;
    [SerializeField] ParticleSystem[] TownCompletedEffect;
    [SerializeField] Image StarEarnPanel;
    [SerializeField] Transform StarEarn;
    [SerializeField] Image SettingsButton;

    [Header("Loading Panel")]
    [SerializeField] GameObject loadingPanel;
    [SerializeField] Image loadingBackground1, loadingBackground2;
    [SerializeField] RectTransform loadingTxt;
    [SerializeField] GameObject loading;
    [SerializeField] VideoPlayer video;

    [Header("BuyBuster")]
    public List<GameObject> bustersIcon = new List<GameObject>();
    public TextMeshProUGUI nameTxt;
    public TextMeshProUGUI priceTxt;
    public TextMeshProUGUI amountTxt;
    public Image ShopPanel;
    public RectTransform Shop;
    public RectTransform shopCoin;
    public TextMeshProUGUI shopCoinTxt;
    [SerializeField] Transform spawnPoint;
    [SerializeField] List<GameObject> buyItems = new List<GameObject>();
    [SerializeField] List<Transform> buyItemsMovePos = new List<Transform>();

    [Header("Win")]
    [SerializeField] Image winPanel;
    [SerializeField] Transform winStar;
    [SerializeField] Transform winCoin;
    [SerializeField] Transform winCoinPos;
    [SerializeField] TextMeshProUGUI winCoinText;
    [SerializeField] Transform continueBtn;
    [SerializeField] GameObject StarCollectEffect;
    [SerializeField] GameObject CoinCollectEffect;
    [SerializeField] Transform starMovePos, coinMovePos;
    [SerializeField] Transform Shine;

    [Header("Lose")]
    [SerializeField] Image losePanel;
    [SerializeField] Transform playOnPanel;
    [SerializeField] Transform loseTryAgainPanel;
    [SerializeField] Transform loseExitPanel;
    public TextMeshProUGUI playOnPrice;

    private Buster currentBuyBuster;

    private Tweener tweener;

    private int playOnCount = 0;

    private bool firstPlay = true;

    private float playTime;
    const int MAX_EARN_SCORE = 100;

    [HideInInspector]
    public bool lockBusters = false;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        OpenHub();
        HideMainGame();

        StartCoroutine(LoadingPanelOnStart());
    }

    public void OpenHub()
    {
        playBtn.gameObject.SetActive(true);
        buildBtn.gameObject.SetActive(true);
    }

    public void HideMainGame()
    {
        mainGameDownSide.rectTransform.anchoredPosition = new Vector2(mainGameDownSide.startPos.x, mainGameDownSide.startPos.y - 1500);
        Items.rectTransform.anchoredPosition = new Vector2(Items.startPos.x + 1500, Items.startPos.y);
        collectionBtn.rectTransform.DOAnchorPos(new Vector2(collectionBtn.startPos.x - 750, collectionBtn.startPos.y ), 0.5f);

    }

    public void HideMainGameSmooth()
    {
        if (ItemController.instance.levels[ItemController.instance.currentLvl].GetComponent<Level>().hasTutorial)
        {
            Hand.GetComponent<SpriteRenderer>().DOFade(0.0f, 0.2f);
            Rule.DOScale(Vector3.zero, 0.2f);
            Rule.GetComponent<Image>().DOFade(0.0f, 0.2f).OnComplete(() =>
            {
                MainTutBackground.DOFade(0.0f, 0.35f);   
                GamePlayTutorial.SetActive(false);
                Hand.SetActive(false);
                
            });
        }
        mainGameDownSide.rectTransform.DOAnchorPos(new Vector2(mainGameDownSide.startPos.x, mainGameDownSide.startPos.y - 1500), 0.5f);
        Items.rectTransform.DOAnchorPos(new Vector2(Items.startPos.x + 1500, Items.startPos.y), 0.5f);
        hubUpSide.rectTransform.DOAnchorPos(new Vector2(hubUpSide.startPos.x, hubUpSide.startPos.y), 0.5f);
    }

    public void OpenMainGame()
    {
        MainGamePanel.SetActive(true);
        if (ItemController.instance.levels[ItemController.instance.currentLvl].GetComponent<Level>().hasTutorial)
        {
            switch (ItemController.instance.currentLvl)
            {
                case 0:  
                    Rule = RulesList[0];
                    Hand = HandList[0];
                    break;
                case 3: 
                    Rule = RulesList[3];
                    Hand = HandList[3];
                    break;
                case 13: 
                    Rule = RulesList[4];
                    Hand = HandList[4];
                    break;
                case 23:
                    Rule = RulesList[5];
                    Hand = HandList[5];
                    break;
                case 45:
                    Rule = RulesList[6];
                    Hand = HandList[6];
                    break;
            }
            GamePlayTutorial.SetActive(true);
            MainTutBackground.DOFade(0.95f, 0.35f).OnComplete(() =>
            {
                Rule.GetComponent<Image>().DOFade(1f, 0.3f);
                Rule.DOScale(Vector3.one, 0.3f).OnComplete(() =>
                {
                    Hand.SetActive(true);
                    Hand.GetComponent<SpriteRenderer>().DOFade(1f, 0.7f);
                });
            });
        }

        DisableReturnBuster();
        DisableUndoBuster();

        hubUpSide.rectTransform.DOAnchorPos(new Vector2(hubUpSide.startPos.x, hubUpSide.startPos.y + 1500), 0.5f);
        ExitButton.DOScale(Vector3.one, 0.5f);

        mainGameDownSide.rectTransform.DOAnchorPos(new Vector2(mainGameDownSide.startPos.x, mainGameDownSide.startPos.y), 0.5f);
        Items.rectTransform.DOAnchorPos(new Vector2(Items.startPos.x, Items.startPos.y), 0.5f).OnComplete(() => ItemController.instance.SaveItemsInStart());
    }

    public void HideHub()
    {
        playBtn.rectTransform.DOAnchorPos(new Vector2(playBtn.startPos.x, playBtn.startPos.y - 1500), 0.5f);
        buildBtn.rectTransform.DOAnchorPos(new Vector2(buildBtn.startPos.x, buildBtn.startPos.y - 1500), 0.5f);
        //collectionBtn.rectTransform.DOAnchorPos(new Vector2(collectionBtn.startPos.x - 1500, collectionBtn.startPos.y ), 0.5f);

        //AudioManager.instance.Stop("Menu Music");
    }

    public void ShowHub()
    {
        
        
        //AudioManager.instance.Stop("Background Music");
        if (firstPlay) AudioManager.instance.Play("Menu Music");
        firstPlay = false;

        playBtn.rectTransform.DOAnchorPos(new Vector2(playBtn.startPos.x, playBtn.startPos.y), 0.5f);
        buildBtn.rectTransform.DOAnchorPos(new Vector2(buildBtn.startPos.x, buildBtn.startPos.y), 0.5f);
        //collectionBtn.rectTransform.DOAnchorPos(new Vector2(collectionBtn.startPos.x, collectionBtn.startPos.y), 0.5f);
        TravelController.instance.DisBlurBackground();
    }

    #region Busters

    public void AddExtraPlace(ParticleSystem particle)
    {
        collectorRect.DOSizeDelta(new Vector2(maxScale, collectorRect.sizeDelta.y), 0.5f).OnUpdate(() => ItemCollector.instance.CorrectPlaces()).OnComplete(() =>
        {
            ItemCollector.instance.CorrectPlaces();
            particle.Play();
            DisableExtraPlaceBuster();
        });

        float f = minTile;

        DOTween.To(() => f, x => f = x, maxTile, 0.5f).OnUpdate(() => tile.uvRect = new Rect(tile.uvRect.x, tile.uvRect.y, f, tile.uvRect.height));
    }

    public void RemoveExtraPlace()
    {
        collectorRect.sizeDelta = new Vector2(minScale, collectorRect.sizeDelta.y);
        tile.uvRect = new Rect(tile.uvRect.x, tile.uvRect.y, minTile, tile.uvRect.height);
        BusterController.instance.extraUsed = false;
        ItemCollector.instance.maxPlaceCount = 7;
        EnableExtraPlaceBuster();
    }

    public void DisableAllBustersVisible()
    {
        DisableAllBustersUnvisible();
    }

    public void EnableAllBustersVisible()
    {
        EnableAllBustersUnvisible();
    }

    public void DisableAllBustersUnvisible()
    {
        undo.interactable = false;
        mix.interactable = false;
        returnTile.interactable = false;
        magic.interactable = false;
        extraPlace.interactable = false;
    }

    public void EnableAllBustersUnvisible()
    {
        if (lockBusters)
            return;

        undo.interactable = true;
        mix.interactable = true;
        returnTile.interactable = true;
        magic.interactable = true;
        extraPlace.interactable = true;
    }

    public void EnableUndoBuster()
    {
        if ((ItemController.instance.currentLvl + 1) < 6)
            return;


        undoSize.color = Color.white;

        BusterController.instance._undoLock = false;

        foreach (var item in undoImages)
        {
            item.color = Color.white;
        }
    }

    public void EnableMixBuster()
    {
        if ((ItemController.instance.currentLvl + 1) < 7)
            return;

        mixSize.color = Color.white;

        BusterController.instance._mixLock = false;

        foreach (var item in mixImages)
        {
            item.color = Color.white;
        }
    }

    public void EnableTileReturnBuster()
    {
        if ((ItemController.instance.currentLvl + 1) < 8)
            return;
        returnTileSize.color = Color.white;

        BusterController.instance._reLock = false;

        foreach (var item in returnTileImages)
        {
            item.color = Color.white;
        }
    }

    public void EnableMagicBuster()
    {
        if ((ItemController.instance.currentLvl + 1) < 9)
            return;
        magicSize.color = Color.white;

        BusterController.instance._magLock = false;

        foreach (var item in magicImages)
        {
            item.color = Color.white;
        }
    }

    public void EnableExtraPlaceBuster()
    {
        if ((ItemController.instance.currentLvl + 1) < 10)
            return;
        extraPlaceSize.color = Color.white;

        BusterController.instance._extLock = false;

        foreach (var item in extraPlaceImages)
        {
            item.color = Color.white;
        }
    }

    public void DisableUndoBuster()
    {
        undoSize.color = Color.gray;

        BusterController.instance._undoLock = true;

        foreach (var item in undoImages)
        {
            item.color = Color.gray;
        }
    }

    public void DisableMixBuster()
    {
        mixSize.color = Color.gray;

        BusterController.instance._mixLock = true;

        foreach (var item in mixImages)
        {
            item.color = Color.gray;
        }
    }

    public void DisableReturnBuster()
    {
        returnTileSize.color = Color.gray;

        BusterController.instance._reLock = true;

        foreach (var item in returnTileImages)
        {
            item.color = Color.gray;
        }
    }

    public void DisableMagicBuster()
    {
        magicSize.color = Color.gray;

        BusterController.instance._magLock = true;

        foreach (var item in magicImages)
        {
            item.color = Color.gray;
        }
    }

    public void DisableExtraPlaceBuster()
    {
        extraPlaceSize.color = Color.gray;

        BusterController.instance._extLock = true;

        foreach (var item in extraPlaceImages)
        {
            item.color = Color.gray;
        }
    }

    #endregion

    public void Play()
    {
        if (ResourcesData.instance._heart == 0)
            return;
        
        AudioManager.instance.Play("Button");

        //AudioManager.instance.Play("Background Music");
        
        playTime = Time.time; // Start record time 
        
        HideHub();
        OpenMainGame();
        TravelController.instance.BlurBackground();
    }

    public void Build()
    {
        if (ItemController.instance.currentLvl == 3 && PlayerPrefs.GetInt("_OpenSightTut") != 1)
        {
            PlayerPrefs.SetInt("_enterBuildTut", 1);
            playBtn.GetComponent<Transform>().GetChild(0).GetComponent<Button>().enabled = true;
            playBtn.GetComponent<Transform>().GetChild(0).GetChild(2).GetComponent<Image>().DOFade(0f, 0.35f);
            playBtn.GetComponent<Transform>().GetChild(0).GetChild(2).gameObject.SetActive(false);
            
            HandList[1].GetComponent<SpriteRenderer>().DOFade(0.0f, 0.2f);
            RulesList[1].DOScale(Vector3.zero, 0.2f);
            RulesList[1].GetComponent<Image>().DOFade(0.0f, 0.2f).OnComplete(() =>
            {
                GamePlayTutorial.SetActive(true);
                MainTutBackground.DOFade(0.0f, 0.35f);
                TravelTutBackground.DOFade(0.95f, 0.35f).OnComplete(() =>
                {
                    RulesList[2].GetComponent<Image>().DOFade(1f, 0.3f);
                    RulesList[2].DOScale(Vector3.one, 0.3f).OnComplete(() =>
                    {
                        HandList[2].SetActive(true);
                        HandList[2].GetComponent<SpriteRenderer>().DOFade(1f, 0.7f);
                    });
                });
                HandList[1].SetActive(false);

            });
        }
        TravelController.instance.Build();
        SettingsButton.transform.DOScale(Vector3.zero, 0.5f);
        //SettingsButton.gameObject.SetActive(false);
        buildBackBtn.gameObject.SetActive(true);
        buildBackBtn.transform.localScale = Vector3.zero;
        collectionBtn.rectTransform.DOAnchorPos(new Vector2(collectionBtn.startPos.x, collectionBtn.startPos.y), 0.5f);
        AudioManager.instance.Play("Button");

        buildBackBtn.transform.DOScale(Vector3.one, 0.35f);

        cityName.DOScale(Vector3.one, 0.35f);
        HideHub();
    }

    public void OpenStarEarn()
    {
        StarEarnPanel.gameObject.SetActive(true);
        StarEarnPanel.DOFade(0.95f, 0.35f);
        StarEarn.DOScale(Vector3.one, 0.35f);
    }

    public void StarEarnPlay()
    {
        SettingsButton.transform.DOScale(Vector3.one, 0.5f);
        StarEarnPanel.DOFade(0.0f, 0.35f).OnComplete(() => StarEarnPanel.gameObject.SetActive(false));
        StarEarn.DOScale(Vector3.zero, 0.35f);
        cityName.DOScale(Vector3.zero, 0.35f);
        buildBackBtn.transform.DOScale(Vector3.zero, 0.35f);
        collectionBtn.rectTransform.DOAnchorPos(new Vector2(collectionBtn.startPos.x - 750, collectionBtn.startPos.y), 0.5f);
        TravelController.instance.Close();
        TravelController.instance.BlurBackground();

        AudioManager.instance.Play("Button");

        OpenMainGame();
    }

    public void CloseStarEarn()
    {
        AudioManager.instance.Play("Button");

        StarEarnPanel.DOFade(0.0f, 0.35f).OnComplete(() => StarEarnPanel.gameObject.SetActive(false));
        StarEarn.DOScale(Vector3.zero, 0.35f);
    }

    public void CloseBuild()
    {
        if (ItemController.instance.currentLvl == 3)
        {
            HandList[2].GetComponent<SpriteRenderer>().DOFade(0.0f, 0.2f);
            RulesList[2].DOScale(Vector3.zero, 0.2f);
            RulesList[2].GetComponent<Image>().DOFade(0.0f, 0.2f).OnComplete(() =>
            {
                TravelTutBackground.DOFade(0f, 0.35f).OnComplete(() =>
                {
                    GamePlayTutorial.gameObject.SetActive(false);
                    HandList[2].SetActive(false);
                });

            });
        }
        
        AudioManager.instance.Play("Button");
        SettingsButton.transform.DOScale(Vector3.one, 0.5f);
        //SettingsButton.gameObject.SetActive(true);
        ShowHub();
        cityName.DOScale(Vector3.zero, 0.35f);
        buildBackBtn.transform.DOScale(Vector3.zero, 0.35f);
        collectionBtn.rectTransform.DOAnchorPos(new Vector2(collectionBtn.startPos.x - 750, collectionBtn.startPos.y), 0.5f);
        TravelController.instance.Close();
    }

    public void OpenLosePanel()
    {
        losePanel.gameObject.SetActive(true);
        losePanel.DOFade(0.95f, 0.35f);
        ExitButton.DOScale(Vector3.zero, 0.35f);

        OpenPlayOnPanel();
    }

    private void OpenPlayOnPanel()
    {
        playOnCount++;

        playOnPrice.text = (playOnCount * 250).ToString();

        shopCoinTxt.text = ResourcesData.instance._coin.ToString();
        shopCoin.DOScale(Vector3.one, 0.35f);
        tweener = playOnPanel.DOScale(Vector3.one, 0.35f);
    }

    private void ClosePlayOnPanel(ButtonList buttonList)
    {
        ItemCollector.instance.matchEnded = false;

        tweener.Kill();
        playOnPanel.DOScale(Vector3.zero, 0.35f);
        shopCoin.DOScale(Vector3.zero, 0.35f);

        ExitButton.DOScale(Vector3.one, 0.35f);

        BusterController.instance.ReturnTile(true);

        ExitButton.DOScale(Vector3.one, 0.35f);
        losePanel.DOFade(0f, 0.35f).OnComplete(() =>
        {
            foreach (var item in buttonList.buttons)
            {
                item.enabled = true;
            }
            losePanel.gameObject.SetActive(false);
        });
    }

    public void OpenGiveUpPanel(ButtonList buttonList)
    {

        AudioManager.instance.Play("Button");

        foreach (var item in buttonList.buttons)
        {
            item.enabled = false;
        }

        tweener.Kill();
        playOnPanel.DOScale(Vector3.zero, 0.35f);
        shopCoin.DOScale(Vector3.zero, 0.35f);

        loseTryAgainPanel.DOScale(Vector3.one, 0.35f).OnComplete(() =>
        {
            foreach (var item in buttonList.buttons)
            {
                item.enabled = true;
            }
        });
    }

    public void TryAgain()
    {

        AudioManager.instance.Play("Button");

        mainGameDownSide.rectTransform.DOAnchorPos(new Vector2(mainGameDownSide.startPos.x, mainGameDownSide.startPos.y - 1500), 0.5f);
        Items.rectTransform.DOAnchorPos(new Vector2(Items.startPos.x + 1500, Items.startPos.y), 0.5f).OnComplete(() =>
        {
            playOnCount = 0;
            RemoveExtraPlace();
            ItemController.instance.RestartLevel();

            mainGameDownSide.rectTransform.DOAnchorPos(new Vector2(mainGameDownSide.startPos.x, mainGameDownSide.startPos.y), 0.5f);
            Items.rectTransform.DOAnchorPos(new Vector2(Items.startPos.x, Items.startPos.y), 0.5f).OnComplete(() => ItemController.instance.SaveItemsInStart());

            ExitButton.DOScale(Vector3.one, 0.5f);
        });

        loseTryAgainPanel.DOScale(Vector3.zero, 0.35f);
        losePanel.DOFade(0f, 0.35f).OnComplete(() => losePanel.gameObject.SetActive(false));
    }

    public void OpenMainGameExit()
    {
        AudioManager.instance.Play("Button");

        losePanel.gameObject.SetActive(true);

        ExitButton.DOScale(Vector3.zero, 0.35f);
        loseExitPanel.DOScale(Vector3.one, 0.35f);
        losePanel.DOFade(0.95f, 0.35f);
    }

    public void CloeMainGameExit()
    {
        AudioManager.instance.Play("Button");

        ExitButton.DOScale(Vector3.one, 0.35f);
        loseExitPanel.DOScale(Vector3.zero, 0.35f);
        losePanel.DOFade(0f, 0.35f);
    }

    public void LeaveMainGameExit()
    {
        loseExitPanel.DOScale(Vector3.zero, 0.35f);

        AudioManager.instance.Play("Button");

        losePanel.DOFade(0f, 0.35f).OnComplete(() => losePanel.gameObject.SetActive(false));

        HideMainGameSmooth();
        ShowHub();
        StartCoroutine(RestartLevel());
    }

    public void CloseLosePanel()
    {
        AudioManager.instance.Play("Button");

        loseTryAgainPanel.transform.DOScale(Vector3.zero, 0.35f);

        losePanel.DOFade(0f, 0.35f).OnComplete(() => losePanel.gameObject.SetActive(false));

        HideMainGameSmooth();
        ShowHub();
        StartCoroutine(RestartLevel());
    }

    public void OpenWinPanel()
    {
        // Player Score calculator
        playTime = Time.time - playTime; // Stop record time
        playTime *= ItemController.instance.currnetLevel.difficulty;
        int playTimeLimit = ItemController.instance.currnetLevel.items.Count * 2;
        int earnScorePerSec = MAX_EARN_SCORE / playTimeLimit;
        int availableScore = MAX_EARN_SCORE - earnScorePerSec * (int)playTime;
        int score = (playTime <= playTimeLimit) ? availableScore : 10;
        PlayerPrefs.SetInt("score", PlayerPrefs.GetInt("score") + score);
        SaveData.Instance.SendScore();
        
        winPanel.gameObject.SetActive(true);

        ExitButton.DOScale(Vector3.zero, 0.25f);

        winPanel.DOFade(0.98f, 0.5f);

        winStar.DOScale(Vector3.one, 0.5f);

        Shine.DOScale(Vector3.one, 0.5f);

        winCoin.DOScale(Vector3.one, 0.5f);
        winCoinText.transform.DOScale(Vector3.one, 0.5f);
        continueBtn.transform.DOScale(Vector3.one, 0.5f);

        AudioManager.instance.Play("Win");
    }

    public void CloseWinPanel()
    {
        AudioManager.instance.Play("Button");

        winStar.DOScale(Vector3.zero, 0.35f);
        winCoin.DOScale(Vector3.zero, 0.35f);
        Shine.DOScale(Vector3.zero, 0.35f);
        winCoinText.transform.DOScale(Vector3.zero, 0.35f);
        continueBtn.transform.DOScale(Vector3.zero, 0.35f);

        winPanel.DOFade(0f, 0.35f).OnComplete(() => winPanel.gameObject.SetActive(false));

        HideMainGameSmooth();
        ShowHub();

        StartCoroutine(NewLevel());
    }

    public void CityName(Sprite name)
    {
        cityNameTxt.sprite = name;
    }

    WaitForSeconds delay1 = new WaitForSeconds(1f);
    WaitForSeconds delay2 = new WaitForSeconds(2f);
    WaitForSeconds delay3 = new WaitForSeconds(3f);
    WaitForSeconds delay025 = new WaitForSeconds(0.25f);
    WaitForSeconds delay05 = new WaitForSeconds(0.5f);
    WaitForSeconds delay075 = new WaitForSeconds(0.75f);
    WaitForSeconds delay03 = new WaitForSeconds(0.3f);

    public IEnumerator NewTravelLevel()
    {
        hubUpSide.rectTransform.DOAnchorPos(new Vector2(hubUpSide.startPos.x, hubUpSide.startPos.y + 1500), 1f);
        buildBackBtn.transform.DOScale(Vector3.zero, 0.25f);
        cityName.DOScale(Vector3.zero, 0.25f);
        collectionBtn.rectTransform.DOAnchorPos(new Vector2(collectionBtn.startPos.x - 750, collectionBtn.startPos.y), 0.5f);
        yield return delay1;

        TownCompleted.DOScale(Vector3.one, 0.5f);
        TownCompletedPanel.gameObject.SetActive(true);
        Color color = Color.black;
        color.a = 0;

        TownCompletedPanel.color = color;
        TownCompletedPanel.DOFade(0.65f, 0.5f);
        TownCompletedEffect[0].Play();
        TownCompletedEffect[1].Play();
        TownCompletedEffect[2].Play();
        TownCompletedEffect[3].Play();

        StartCoroutine(FireworkSound());

        yield return delay05;
        continueTravelPanel.DOScale(Vector3.one, 0.5f);

    }

    IEnumerator FireworkSound()
    {
        AudioManager.instance.Play("Firework Music");

        for (int i = 0; i < 4; i++)
        {
            AudioManager.instance.Play("Firework");

            yield return delay03;
        }
    }

    public void CloseNewTravelPanel(ButtonList buttonList)
    {

        AudioManager.instance.Play("Button");
        SettingsButton.transform.DOScale(Vector3.one, 0.5f);
        //SettingsButton.gameObject.SetActive(true);

        foreach (var item in buttonList.buttons)
        {
            item.enabled = false;
        }

        continueTravelPanel.DOScale(Vector3.zero, 0.25f);
        TownCompletedPanel.DOFade(0f, 0.25f);

        TownCompleted.DOScale(Vector3.zero, 0.25f).OnComplete(() => TownCompletedPanel.gameObject.SetActive(false));

        StartCoroutine(LoadingPanel(buttonList));
    }

    private IEnumerator LoadingPanel(ButtonList buttonList)
    {
        yield return delay05;

        foreach (var item in buttonList.buttons)
        {
            item.enabled = true;
        }

        loadingPanel.gameObject.SetActive(true);
        loadingBackground1.DOFade(1, 0.75f);
        loadingBackground2.DOFade(1, 0.75f);

        yield return delay05;

        //loading.transform.DOScale(Vector3.one, 0.25f);

        loadingTxt.DOScale(Vector3.one, 0.5f);
        
        //while
        yield return delay2;

        TravelController.instance.NewTravelLevel();

        yield return delay2;

        loadingBackground1.DOFade(0, 0.75f).OnComplete(() => loadingPanel.gameObject.SetActive(false));
        loadingBackground2.DOFade(0, 0.75f);

        loading.transform.DOScale(Vector3.zero, 0.25f);
        loadingTxt.DOScale(Vector3.zero, 0.25f);

        yield return delay075;
        ShowHub();
        hubUpSide.rectTransform.DOAnchorPos(new Vector2(hubUpSide.startPos.x, hubUpSide.startPos.y), 0.5f);
        Debug.Log("Gelayyyyy");
        TravelCollection.instance.Initialize(TravelController.instance.currentTravelLevel, TravelController.instance.travelLevels.Count);
        Debug.Log(TravelController.instance.currentTravelLevel + "---" + TravelController.instance.travelLevels.Count);

    }

    private IEnumerator LoadingPanelOnStart()
    {
        loadingPanel.gameObject.SetActive(true);
        loadingBackground1.DOFade(1, 0.001f);
        loadingBackground2.DOFade(1, 0.001f);

        HideHub();
        hubUpSide.rectTransform.DOAnchorPos(new Vector2(hubUpSide.startPos.x, hubUpSide.startPos.y + 1500), 0.001f);

        yield return delay05;

        loading.transform.DOScale(Vector3.one, 0.25f);
        loadingTxt.DOScale(Vector3.one, 0.5f);

        yield return delay3;
        loadingBackground1.DOFade(0, 0.75f).OnComplete(() => loadingPanel.gameObject.SetActive(false));
        loadingBackground2.DOFade(0, 0.75f);

        loading.transform.DOScale(Vector3.zero, 0.25f);
        loadingTxt.DOScale(Vector3.zero, 0.25f);

        yield return delay075;
        ShowHub();
        hubUpSide.rectTransform.DOAnchorPos(new Vector2(hubUpSide.startPos.x, hubUpSide.startPos.y), 0.5f);
    }

    IEnumerator NewLevel()
    {

        ResourcesData.instance.AddStar(1);
        ResourcesData.instance.AddCoin(10);
        playOnCount = 0;

        StartCoroutine(WinEffect());

        yield return delay075;

        RemoveExtraPlace();
        ItemController.instance.NewLevel();
        
        if (ItemController.instance.currentLvl == 3 && PlayerPrefs.GetInt("_enterBuildTut") != 1)
        {
            GamePlayTutorial.SetActive(true);
            playBtn.GetComponent<Transform>().GetChild(0).GetChild(2).gameObject.SetActive(true);
            playBtn.GetComponent<Transform>().GetChild(0).GetComponent<Button>().enabled = false;
            playBtn.GetComponent<Transform>().GetChild(0).GetChild(2).GetComponent<Image>().DOFade(0.95f, 0.35f);
            MainTutBackground.DOFade(0.95f, 0.35f).OnComplete(() =>
            {
                RulesList[1].GetComponent<Image>().DOFade(1f, 0.3f);
                RulesList[1].DOScale(Vector3.one, 0.3f).OnComplete(() =>
                {
                    HandList[1].SetActive(true);
                    HandList[1].GetComponent<SpriteRenderer>().DOFade(1f, 0.7f);
                });
            });
        }
    }

    IEnumerator WinEffect()
    {

        List<GameObject> list = new List<GameObject>();
        GameObject s = null;

        for (int i = 0; i < 3; i++)
        {
            GameObject a = Instantiate(CoinCollectEffect, winCoinPos.position, CoinCollectEffect.transform.rotation, MainGamePanel.transform);
            a.transform.localScale = Vector3.zero;
            list.Add(a);
            a.transform.DOScale(Vector3.one * 35, 0.5f);
        }

        s = Instantiate(StarCollectEffect, winStar.transform.position, StarCollectEffect.transform.rotation, MainGamePanel.transform);
        s.transform.localScale = Vector3.zero;
        s.transform.DOScale(Vector3.one * 50, 0.5f);

        yield return delay025;

        s.transform.DOMove(starMovePos.position, .5f).OnComplete(() => AudioManager.instance.Play("Earn Star"));
        s.transform.DOScale(Vector3.one * 20, .5f).OnComplete(() => { s.transform.GetChild(0).SetParent(null); Destroy(s); starMovePos.DOScale(Vector3.one* 1.2f, 0.125f).OnComplete(() => starMovePos.DOScale(Vector3.one, 0.125f)); });

        for (int i = 0; i < list.Count; i++)
        {
            GameObject v = list[i];

            v.transform.DOMove(coinMovePos.position, .3f).OnComplete(() => AudioManager.instance.Play("Earn Coin"));
            v.transform.DOScale(Vector3.one * 20, .3f).OnComplete(() => { v.transform.GetChild(0).SetParent(null); Destroy(v); coinMovePos.DOScale(Vector3.one * 1.2f, 0.125f).OnComplete(() => coinMovePos.DOScale(Vector3.one, 0.125f)); });

            yield return delay025;
        }
    }

    IEnumerator RestartLevel()
    {
        playOnCount = 0;

        ResourcesData.instance.RemoveHeart(1);
        yield return delay05;
        RemoveExtraPlace();
        ItemController.instance.RestartLevel();
    }

    public void OpenBuyBuster(Buster buster)
    {
        shopCoinTxt.text = ResourcesData.instance._coin.ToString();
        currentBuyBuster = buster;

        nameTxt.text = buster.name;
        priceTxt.text = buster.price.ToString();
        amountTxt.text = buster.amount.ToString();

        bustersIcon[buster.id].SetActive(true);

        ExitButton.DOScale(Vector3.zero, 0.25f);

        ShopPanel.gameObject.SetActive(true);
        ShopPanel.DOFade(0.95f, 0.25f);
        shopCoin.DOScale(Vector3.one, 0.25f);
        Shop.DOScale(Vector3.one, 0.25f);
    }

    public void CloseBuyBuster()
    {
        foreach (var item in bustersIcon)
        {
            item.SetActive(false);
        }

        ShopPanel.DOFade(0f, 0.15f).OnComplete(() => ShopPanel.gameObject.SetActive(false));
        Shop.DOScale(Vector3.zero, 0.15f);
        shopCoin.DOScale(Vector3.zero, 0.15f);
        ExitButton.DOScale(Vector3.one, 0.25f);
    }

    public void Buy()
    {
        AudioManager.instance.Play("Button");

        if (ResourcesData.instance._coin < currentBuyBuster.price)
            return;
        else
            ResourcesData.instance.RemoveCoin(currentBuyBuster.price);

        shopCoinTxt.text = ResourcesData.instance._coin.ToString();

        if (currentBuyBuster.id == 0)
        {
            ResourcesData.instance.AddUndo(currentBuyBuster.amount);

        }
        else if (currentBuyBuster.id == 1)
        {
            ResourcesData.instance.AddMix(currentBuyBuster.amount);

        }
        else if (currentBuyBuster.id == 2)
        {
            ResourcesData.instance.AddReturnTile(currentBuyBuster.amount);

        }
        else if (currentBuyBuster.id == 3)
        {
            ResourcesData.instance.AddMagic(currentBuyBuster.amount);

        }
        else if (currentBuyBuster.id == 4)
        {
            ResourcesData.instance.AddExtraPlace(currentBuyBuster.amount);

        }

        StartCoroutine(BuyItemEffect(currentBuyBuster.id));

        CloseBuyBuster();
    }

    IEnumerator BuyItemEffect(int id)
    {
        GameObject itm = buyItems[id];

        for (int i = 0; i < 3; i++)
        {
            GameObject a = Instantiate(itm, spawnPoint.transform.position, itm.transform.rotation, MainGamePanel.transform);

            a.transform.GetChild(0).GetComponent<ParticleSystem>().Play();

            a.transform.localScale = Vector3.zero;
            a.transform.DOScale(Vector3.one * 40, 0.75f).OnComplete(() =>
            {

                a.transform.DOMove(buyItemsMovePos[id].position, .75f);
                a.transform.DOScale(Vector3.one * 20, .75f).OnComplete(() => { a.transform.GetChild(0).SetParent(null); Destroy(a); });
            });

            yield return delay025;
        }
    }

    public void PlayOn(ButtonList buttonList)
    {

        AudioManager.instance.Play("Button");

        int price = 250;


        price *= playOnCount;


        if (ResourcesData.instance._coin < price)
            return;
        else
        {
            foreach (var item in buttonList.buttons)
            {
                item.enabled = false;
            }

            ResourcesData.instance.RemoveCoin(price);
            ClosePlayOnPanel(buttonList);
        }
    }
}