using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class ProfileController : MonoBehaviour
{
    public static ProfileController Instance { get; set; }
    
    [Header("Panels")] 
    [SerializeField] Image profilePanel;
    [SerializeField] RectTransform changeProfilePanel;
    [SerializeField] RectTransform changeNamePanel;
    [SerializeField] TMP_InputField inputField;
    [SerializeField] Image profileButton;
    [SerializeField] Transform closeBtn;
    [SerializeField] Image noInternetPanel;
    [SerializeField] Transform noInternetUI;
    [SerializeField] Transform closeNoInternetBtn;

    [Header("Player's Data")]
    [SerializeField] Image profilePicture;
    [SerializeField] TextMeshProUGUI playerName;
    [SerializeField] TextMeshProUGUI newPlayerName;
    [SerializeField] List<ButtonData> buttonsData;
    private int newProfileIndex;

    private void Awake()
    {
        Instance = this;
        
        buttonsData[PlayerPrefs.GetInt("profileIndex", 0)].btn.transform.GetChild(0).localScale = Vector3.one;
        foreach (var data in buttonsData)
        {
            data.btn.onClick.AddListener(delegate
            {
                ChangeProfilePicture(data.btn.GetComponent<Image>().sprite, data.btn.transform.GetChild(0), data.index);
            });
        }
    }

    private void Start()
    {
        playerName.text = PlayerPrefs.GetString("playerName", "Oýunçy");
        profilePicture.sprite = buttonsData[PlayerPrefs.GetInt("profileIndex", 0)].picture;
        profileButton.sprite = buttonsData[PlayerPrefs.GetInt("profileIndex", 0)].picture;
    }

    void ChangeProfilePicture(Sprite picture, Transform tick, int index)
    {
        newProfileIndex = index;
        profilePicture.sprite = picture;
        foreach (var data in buttonsData)
        {
            if (data.btn.transform.GetChild(0).localScale.x > 0)
            {
                data.btn.transform.GetChild(0).DOScale(Vector3.zero, 0.1f);
                break;
            }
        }
        tick.DOScale(Vector3.one, 0.2f);
    }

    public void ChangePlayerName()
    {
        inputField.text = "";
        closeBtn.DOScale(Vector3.zero, 0.25f);
        changeProfilePanel.DOScale(Vector3.zero, 0.35f).OnComplete(() =>
        {
            changeNamePanel.DOScale(Vector3.one, 0.25f);
        });
    }

    public void MoveUpPlayerNamePanel()
    {
        LanguagesData.Instance.placeHolder.text = "";
        changeNamePanel.DOLocalMoveY(325, 0.35f);
    }
    
    public void MoveDownPlayerNamePanel()
    {
        LanguagesData.Instance.placeHolder.text = LanguagesData.Instance.placeHolderText; 
        changeNamePanel.DOLocalMoveY(100, 0.35f);
    }

    public void ContinueWithNewPlayerName()
    {
        AudioManager.instance.Play("Button");
        string oldName = PlayerPrefs.GetString("oldName");
        int icon = PlayerPrefs.GetInt("profileIndex", 0);
        PlayerPrefs.SetString("playerName", newPlayerName.text);
        StartCoroutine(HighScores.Instance.DatabaseUpdate(oldName, icon));
        playerName.text = newPlayerName.text;
        changeNamePanel.DOScale(Vector3.zero, 0.35f).OnComplete(() =>
        {
            closeBtn.DOScale(Vector3.one, 0.25f);
            changeProfilePanel.DOScale(Vector3.one, 0.25f);
            changeNamePanel.DOLocalMoveY(100, 0.35f);
        });
        //isOpenedNamePanel = false;
    }
    
    public void CloseNewPlayerNamePanel()
    {
        AudioManager.instance.Play("Button");
        changeNamePanel.DOScale(Vector3.zero, 0.35f).OnComplete(() =>
        {
            closeBtn.DOScale(Vector3.one, 0.25f);
            changeProfilePanel.DOScale(Vector3.one, 0.25f);
            changeNamePanel.DOLocalMoveY(100, 0.35f);
        });
        //isOpenedNamePanel = false;
    }
    
    public void CloseProfilePanel()
    {
        AudioManager.instance.Play("Button");
        profileButton.transform.DOScale(Vector3.one, 0.35f);
        
        profilePicture.sprite = buttonsData[PlayerPrefs.GetInt("profileIndex", 0)].picture;
        buttonsData[newProfileIndex].btn.transform.GetChild(0).DOScale(Vector3.zero, 0.1f);
        buttonsData[PlayerPrefs.GetInt("profileIndex", 0)].btn.transform.GetChild(0).DOScale(Vector3.one, 0.2f);

        closeBtn.DOScale(Vector3.zero, 0.35f);
        changeProfilePanel.DOScale(Vector3.zero, 0.35f);
        profilePanel.DOFade(0f, 0.35f).OnComplete(() => profilePanel.gameObject.SetActive(false));
    }
    
    public void OpenProfilePanel()
    {
        AudioManager.instance.Play("Button");
        profileButton.transform.DOScale(Vector3.zero, 0.35f);
        
        profilePanel.gameObject.SetActive(true);
        closeBtn.DOScale(Vector3.one, 0.35f);
        profilePanel.DOFade(0.95f, 0.35f);
        changeProfilePanel.DOScale(Vector3.one, 0.35f);
    }

    public void SaveProfileData()
    {
        AudioManager.instance.Play("Button");
        string oldName = PlayerPrefs.GetString("oldName");
        StartCoroutine(HighScores.Instance.DatabaseUpdate(oldName, newProfileIndex));
        profileButton.sprite = buttonsData[newProfileIndex].picture;
        PlayerPrefs.SetInt("profileIndex", newProfileIndex);
        CloseProfilePanel();
    }

    public void OpenNoInternetPanel()
    {
        noInternetPanel.gameObject.SetActive(true);
        closeNoInternetBtn.DOScale(Vector3.one, 0.35f);
        noInternetPanel.DOFade(0.95f, 0.35f);
        noInternetUI.DOScale(Vector3.one, 0.35f);
    }
    
    public void CloseNoInternetPanel()
    {
        AudioManager.instance.Play("Button");
        closeNoInternetBtn.DOScale(Vector3.zero, 0.35f);
        noInternetUI.DOScale(Vector3.zero, 0.35f);
        noInternetPanel.DOFade(0f, 0.35f).OnComplete(() =>
        {
            noInternetPanel.gameObject.SetActive(false);
        });
        
    }

    [Serializable]
    public class ButtonData
    {
        public Button btn;
        public Sprite picture;
        public int index;
    }
    
}
