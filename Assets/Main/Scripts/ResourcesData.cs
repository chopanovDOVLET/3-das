using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class ResourcesData : MonoBehaviour
{

    public static ResourcesData instance;

    public TextMeshProUGUI coinTxt, starTxt, heartTxt, heartTimerTxt;

    [HideInInspector]
    public int _coin, _star, _heart, _timer;

    public int maxTimer;

    private float time;

    private bool countDown;

    [Header("Busters")]
    public Sprite addBusterUI;
    public Sprite amountBusterUI;
    public int UndoSize;
    public int MixSize;
    public int ReturnTileSize;
    public int MagicSize;
    public int ExtraPlaceSize;
    [SerializeField] TextMeshProUGUI UndoSizeTxt;
    [SerializeField] TextMeshProUGUI MixSizeTxt;
    [SerializeField] TextMeshProUGUI ReturnTileSizeTxt;
    [SerializeField] TextMeshProUGUI MagicSizeTxt;
    [SerializeField] TextMeshProUGUI ExtraPlaceSizeTxt;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {

        SetOfflineTime();

        InitializeCoin();
        InitializeStar();
        InitializeHeart();
        InitializeBusters();
    }

    private void Update()
    {
        if (countDown)
        {
            time -= Time.deltaTime;
            Timer();
        }
    }

    #region Resources
    public void InitializeCoin()
    {
        _coin = PlayerPrefs.GetInt("Coin", 10000);
        coinTxt.text = _coin.ToString();
    }

    public void InitializeStar()
    {
        _star = PlayerPrefs.GetInt("Star", 100);
        starTxt.text = _star.ToString();
    }

    public void InitializeHeart()
    {
        _heart = PlayerPrefs.GetInt("Heart", 5);
        heartTxt.text = _heart.ToString();

        if (_heart == 5)
            countDown = false;
        else
            countDown = true;

        CheckCountDown(LanguagesData.Instance.heartText, LanguagesData.Instance.fontAsset);
    }

    public void AddCoin(int coin)
    {
        PlayerPrefs.SetInt("Coin", _coin + coin);
        InitializeCoin();
    }

    public void RemoveCoin(int coin)
    {
        PlayerPrefs.SetInt("Coin", _coin - coin);
        InitializeCoin();
    }

    public void AddStar(int star)
    {
        PlayerPrefs.SetInt("Star", _star + star);
        InitializeStar();
    }

    public void RemoveStar(int star)
    {
        PlayerPrefs.SetInt("Star", _star - star);
        InitializeStar();
    }

    public void AddHeart(int heart)
    {
        PlayerPrefs.SetInt("Heart", _heart + heart);
        InitializeHeart();
    }

    public void RemoveHeart(int heart)
    {
        PlayerPrefs.SetInt("Heart", _heart - heart);
        InitializeHeart();
    }
    #endregion

    #region Busters
    private void InitializeBusters()
    {
        UndoSize = PlayerPrefs.GetInt("Undo", 3);
        MixSize = PlayerPrefs.GetInt("Mix", 3);
        ReturnTileSize = PlayerPrefs.GetInt("ReturnTile", 3);
        MagicSize = PlayerPrefs.GetInt("Magic", 3);
        ExtraPlaceSize = PlayerPrefs.GetInt("ExtraPlace", 3);

        if (UndoSize == 0)
        {
            UndoSizeTxt.rectTransform.parent.GetComponent<RectTransform>().DOAnchorPosY(65f, 0f);
            UndoSizeTxt.rectTransform.parent.GetComponent<Image>().sprite = addBusterUI;
            UndoSizeTxt.text = "";
        }
        else
        {
            UndoSizeTxt.rectTransform.parent.GetComponent<RectTransform>().DOAnchorPosY(-65f, 0f);
            UndoSizeTxt.rectTransform.parent.GetComponent<Image>().sprite = amountBusterUI;
            UndoSizeTxt.text = UndoSize.ToString();
        }

        if (MixSize == 0)
        {
            MixSizeTxt.rectTransform.parent.GetComponent<RectTransform>().DOAnchorPosY(65f, 0f);
            MixSizeTxt.rectTransform.parent.GetComponent<Image>().sprite = addBusterUI;
            MixSizeTxt.text = "";
        }
        else
        {
            MixSizeTxt.rectTransform.parent.GetComponent<RectTransform>().DOAnchorPosY(-65f, 0f);
            MixSizeTxt.rectTransform.parent.GetComponent<Image>().sprite = amountBusterUI;
            MixSizeTxt.text = MixSize.ToString();
        }

        if (ReturnTileSize == 0)
        {
            ReturnTileSizeTxt.rectTransform.parent.GetComponent<RectTransform>().DOAnchorPosY(65f, 0f);
            ReturnTileSizeTxt.rectTransform.parent.GetComponent<Image>().sprite = addBusterUI;
            ReturnTileSizeTxt.text = "";
        }
        else
        {
            ReturnTileSizeTxt.rectTransform.parent.GetComponent<RectTransform>().DOAnchorPosY(-65f, 0f);
            ReturnTileSizeTxt.rectTransform.parent.GetComponent<Image>().sprite = amountBusterUI;
            ReturnTileSizeTxt.text = ReturnTileSize.ToString();
        }

        if (MagicSize == 0)
        {
            MagicSizeTxt.rectTransform.parent.GetComponent<RectTransform>().DOAnchorPosY(65f, 0f);
            MagicSizeTxt.rectTransform.parent.GetComponent<Image>().sprite = addBusterUI;
            MagicSizeTxt.text = "";
        }
        else
        {
            MagicSizeTxt.rectTransform.parent.GetComponent<RectTransform>().DOAnchorPosY(-65f, 0f);
            MagicSizeTxt.rectTransform.parent.GetComponent<Image>().sprite = amountBusterUI;
            MagicSizeTxt.text = MagicSize.ToString();
        }

        if (ExtraPlaceSize == 0)
        {
            ExtraPlaceSizeTxt.rectTransform.parent.GetComponent<RectTransform>().DOAnchorPosY(65f, 0f);
            ExtraPlaceSizeTxt.rectTransform.GetComponentInParent<Image>().sprite = addBusterUI;
            ExtraPlaceSizeTxt.text = "";
        }
        else
        {
            ExtraPlaceSizeTxt.rectTransform.parent.GetComponent<RectTransform>().DOAnchorPosY(-65f, 0f);
            ExtraPlaceSizeTxt.rectTransform.parent.GetComponent<Image>().sprite = amountBusterUI;
            ExtraPlaceSizeTxt.text = ExtraPlaceSize.ToString();
        }
    }

    public void AddUndo(int undo)
    {
        PlayerPrefs.SetInt("Undo", UndoSize + undo);
        InitializeBusters();
    }

    public void RemoveUndo(int undo)
    {
        PlayerPrefs.SetInt("Undo", UndoSize - undo);
        InitializeBusters();
    }

    public void AddMix(int mix)
    {
        PlayerPrefs.SetInt("Mix", MixSize + mix);
        InitializeBusters();
    }

    public void RemoveMix(int mix)
    {
        PlayerPrefs.SetInt("Mix", MixSize - mix);
        InitializeBusters();
    }

    public void AddReturnTile(int returnTile)
    {
        PlayerPrefs.SetInt("ReturnTile", ReturnTileSize + returnTile);
        InitializeBusters();
    }

    public void RemoveReturnTile(int returnTile)
    {
        PlayerPrefs.SetInt("ReturnTile", ReturnTileSize - returnTile);
        InitializeBusters();
    }

    public void AddMagic(int magic)
    {
        PlayerPrefs.SetInt("Magic", MagicSize + magic);
        InitializeBusters();
    }

    public void RemoveMagic(int magic)
    {
        PlayerPrefs.SetInt("Magic", MagicSize - magic);
        InitializeBusters();
    }

    public void AddExtraPlace(int extraPlace)
    {
        PlayerPrefs.SetInt("ExtraPlace", ExtraPlaceSize + extraPlace);
        InitializeBusters();
    }

    public void RemoveExtraPlace(int extraPlace)
    {
        PlayerPrefs.SetInt("ExtraPlace", ExtraPlaceSize - extraPlace);
        InitializeBusters();
    }

    #endregion


    #region Timer

    private void SetOfflineTime()
    {
        _heart = PlayerPrefs.GetInt("Heart", 0);

        if (_heart != 5)
        {
            float goneTime = GetDifferenceBetweenLastLogin();
            time = PlayerPrefs.GetFloat("LastTime", 0);
            _heart = PlayerPrefs.GetInt("Heart", 0);

            if (goneTime == -1)
                return;

            int earnHeart = 0;

        newWave:

            if (time - goneTime < 0)
            {
                earnHeart++;
                goneTime -= time;
                time = maxTimer;
                if (earnHeart + _heart == 5)
                {
                    AddHeart(earnHeart);
                    return;
                }

                goto newWave;
            }
            else
            {
                time -= goneTime;
            }

            AddHeart(earnHeart);
        }
    }

    private float GetDifferenceBetweenLastLogin()
    {
        if (PlayerPrefs.GetString("LastLogin", "") == "")
            return -1;
        DateTime lastLogin = DateTime.Parse(PlayerPrefs.GetString("LastLogin", DateTime.Now.ToString()));
        TimeSpan interval = DateTime.Now - lastLogin;
        return interval.Seconds;
    }

    private void Timer()
    {
        if (time < 0)
        {
            time = 0;
            AddHeart(1);
        }

        float minutes = Mathf.FloorToInt(time / 60);
        float seconds = Mathf.FloorToInt(time % 60);
        
        if (countDown)
        {
            heartTimerTxt.font = LanguagesData.Instance.turkmenFont;
            heartTimerTxt.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    public void CheckCountDown(string text, TMP_FontAsset fontAsset)
    {
        
        if (countDown && time == 0)
            time = maxTimer;
        else
        {
            heartTimerTxt.font = fontAsset;
            heartTimerTxt.text = text;
        }      
    }

    #endregion


    private void OnApplicationQuit()
    {
        string lastLogin = DateTime.Now.ToString();
        PlayerPrefs.SetString("LastLogin", lastLogin);
        PlayerPrefs.SetFloat("LastTime", time);
    }
}