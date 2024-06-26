using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class SettingButton : MonoBehaviour
{
    public static SettingButton Instance;

    private const int SOUND_ON_POS = 88;
    [Header("Sound & Vibration")]
    [SerializeField] private RectTransform handleSound;
    [SerializeField] private RectTransform handleVibration;
    [SerializeField] private bool soundOn;
    [SerializeField] public bool vibrationOn;
    [SerializeField] private Sprite onToggle;
    [SerializeField] private Sprite offToggle;

    [Header("Languages")]
    [SerializeField] private Transform sizeFilter;
    [SerializeField] private LanguagesData lng;
    [SerializeField] private Image languagesUI;
    [SerializeField] private Sprite[] languagesUISprites;
    private int isLng;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        isLng = PlayerPrefs.GetInt("Languages", 0);
        ChangeLanguages();
        
        soundOn = PlayerPrefs.GetInt("SoundOnOff", 1) == 1 ? true : false;
        vibrationOn = PlayerPrefs.GetInt("VibrationOnOff", 1) == 1 ? true : false;

        TurnOnOffSound();
        TurnOnOffViberation();
    }

    public void TurnOnOffSound()
    {
        if (soundOn) 
        {
            handleSound.DOAnchorPosX(SOUND_ON_POS, 0.2f);
            handleSound.GetComponent<Image>().sprite = onToggle;

            foreach (Sound s in AudioManager.instance.sounds)
                s.source.volume = 1;

            soundOn = false;
            PlayerPrefs.SetInt("SoundOnOff", 1);
        }
        else
        {
            handleSound.DOAnchorPosX(-SOUND_ON_POS, 0.2f);
            handleSound.GetComponent<Image>().sprite = offToggle;

            foreach (Sound s in AudioManager.instance.sounds)
                s.source.volume = 0;

            soundOn = true;
            PlayerPrefs.SetInt("SoundOnOff", 0);
        }
    }

    public void TurnOnOffViberation()
    {
        if (vibrationOn)
        {
            handleVibration.DOAnchorPosX(SOUND_ON_POS, 0.2f);
            handleVibration.GetComponent<Image>().sprite = onToggle;

            vibrationOn = false;
            PlayerPrefs.SetInt("VibrationOnOff", 1);
        }
        else
        {
            handleVibration.DOAnchorPosX(-SOUND_ON_POS, 0.2f);
            handleVibration.GetComponent<Image>().sprite = offToggle;

            vibrationOn = true;
            PlayerPrefs.SetInt("VibrationOnOff", 0);
        }
    }

    public void ClickRightBtn()
    {
        AudioManager.instance.Play("Button");
        if (isLng + 1 < 3)
            isLng++;
        else
            isLng = 0;
        ChangeLanguages();
    }
    
    public void ClickLeftBtn()
    {
        AudioManager.instance.Play("Button");
        if (isLng - 1 >= 0)
            isLng--;
        else
            isLng = 2;
        ChangeLanguages();
    }
    
    public void ChangeLanguages()
    {
        languagesUI.sprite = languagesUISprites[isLng];
        PlayerPrefs.SetInt("Languages", isLng);
        switch (isLng)
        {
            case 0: ChangeLanguageToTurkmen();
                break;
            case 1: ChangeLanguageToEnglish();
                break;
            case 2: ChangeLanguageToRussian();
                break;
        }
    }

    public void ChangeLanguageToTurkmen()
    {
        for (int i = 0; i < sizeFilter.childCount; i++)
        {
            if (sizeFilter.GetChild(i).GetComponent<TownUICollection>().isBlocked)
                sizeFilter.GetChild(i).GetComponent<TownUICollection>().townImage.sprite = lng.townData_tkm[i].cityOff;
            else
                sizeFilter.GetChild(i).GetComponent<TownUICollection>().townImage.sprite = lng.townData_tkm[i].cityOn;
        }

        lng.fontAsset = lng.turkmenFont;
        lng.heartText = lng.heartText_tkm;
        lng.placeHolderText = lng.placeholder_tkm;
        lng.placeHolder.text = lng.placeholder_tkm;
        lng.loadingPanelText.text = lng.loadingPanelText_tkm;
        ResourcesData.instance.CheckCountDown(lng.heartText_tkm, lng.turkmenFont);
        lng.play.sprite = lng.play_tkm;
        lng.build.sprite = lng.build_tkm;
        lng.getStarUI.sprite = lng.getStarUI_tkm;
        lng.getStarLeaveBtn.sprite = lng.getStarLeaveBtn_tkm;
        lng.collectionHeader.sprite = lng.collectionHeader_tkm;
        lng.leaderboardHeader.sprite = lng.leaderboardHeader_tkm;
        lng.leaderboardIcon.sprite = lng.leaderboardIcon_tkm;
        lng.changeProfilePanel.sprite = lng.changeProfilePanel_tkm;
        lng.saveProfileBtn.sprite = lng.saveProfileBtn_tkm;
        lng.changeNamePanel.sprite = lng.changeNamePanel_tkm;
        lng.changeNameBtn.sprite = lng.changeNameBtn_tkm;
        lng.settingsHeader.sprite = lng.settingsHeader_tkm;
        lng.settingsPanel.sprite = lng.settingsPanel_tkm;
        lng.noInternetPanel.sprite = lng.noInternetPanel_tkm;
        lng.noInternetBtn.sprite = lng.noInternetBtn_tkm;
        lng.tryAgain.sprite = lng.tryAgain_tkm;
        lng.tryAgainBtn.sprite = lng.tryAgainBtn_tkm;
        lng.keepPlaying.sprite = lng.keepPlaying_tkm;
        lng.playOnBtn.sprite = lng.playOnBtn_tkm;
        lng.giveUpBtn.sprite = lng.giveUpBtn_tkm;
        lng.leaveUI.sprite = lng.leaveUI_tkm;
        lng.leaveBtn.sprite = lng.leaveBtn_tkm;
        lng.winTitle.sprite = lng.winTitle_tkm;
        lng.winContinueBtn.sprite = lng.winContinueBtn_tkm;
        lng.undo.sprite = lng.undo_tkm;
        lng.mix.sprite = lng.mix_tkm;
        lng.returnTile.sprite = lng.returnTile_tkm;
        lng.magic.sprite = lng.magic_tkm;
        lng.extraPlace.sprite = lng.extraPlace_tkm;
        lng.townCompletePanelBtn.text = lng.townCompletePanelBtn_tkm;
        lng.townCompletePanelBtn.font = lng.townCompletePanelBtnFont_tkm;
        lng.rule_1.sprite = lng.rule_1_tkm;
        lng.rule_2.sprite = lng.rule_2_tkm;
        lng.rule_3.sprite = lng.rule_3_tkm;
        lng.rule_4.sprite = lng.rule_4_tkm;
        lng.rule_5.sprite = lng.rule_5_tkm;
        lng.rule_6.sprite = lng.rule_6_tkm;
        lng.rule_7.sprite = lng.rule_7_tkm;
    }

    public void ChangeLanguageToRussian()
    {
        for (int i = 0; i < sizeFilter.childCount; i++)
        {  
            if (sizeFilter.GetChild(i).GetComponent<TownUICollection>().isBlocked)
                sizeFilter.GetChild(i).GetComponent<TownUICollection>().townImage.sprite = lng.townData_rus[i].cityOff;
            else
                sizeFilter.GetChild(i).GetComponent<TownUICollection>().townImage.sprite = lng.townData_rus[i].cityOn;
        }

        lng.fontAsset = lng.russianFont;
        lng.heartText = lng.heartText_rus;
        lng.placeHolderText = lng.placeholder_rus;
        lng.placeHolder.text = lng.placeholder_rus;
        lng.loadingPanelText.text = lng.loadingPanelText_rus;
        ResourcesData.instance.CheckCountDown(lng.heartText_rus, lng.russianFont);
        lng.play.sprite = lng.play_rus;
        lng.build.sprite = lng.build_rus;
        lng.getStarUI.sprite = lng.getStarUI_rus;
        lng.getStarLeaveBtn.sprite = lng.getStarLeaveBtn_rus;
        lng.collectionHeader.sprite = lng.collectionHeader_rus;
        lng.leaderboardHeader.sprite = lng.leaderboardHeader_rus;
        lng.leaderboardIcon.sprite = lng.leaderboardIcon_rus;
        lng.changeProfilePanel.sprite = lng.changeProfilePanel_rus;
        lng.saveProfileBtn.sprite = lng.saveProfileBtn_rus;
        lng.changeNamePanel.sprite = lng.changeNamePanel_rus;
        lng.changeNameBtn.sprite = lng.changeNameBtn_rus;
        lng.settingsHeader.sprite = lng.settingsHeader_rus;
        lng.settingsPanel.sprite = lng.settingsPanel_rus;
        lng.noInternetPanel.sprite = lng.noInternetPanel_rus;
        lng.noInternetBtn.sprite = lng.noInternetBtn_rus;
        lng.tryAgain.sprite = lng.tryAgain_rus;
        lng.tryAgainBtn.sprite = lng.tryAgainBtn_rus;
        lng.keepPlaying.sprite = lng.keepPlaying_rus;
        lng.playOnBtn.sprite = lng.playOnBtn_rus;
        lng.giveUpBtn.sprite = lng.giveUpBtn_rus;
        lng.leaveUI.sprite = lng.leaveUI_rus;
        lng.leaveBtn.sprite = lng.leaveBtn_rus;
        lng.winTitle.sprite = lng.winTitle_rus;
        lng.winContinueBtn.sprite = lng.winContinueBtn_rus;
        lng.undo.sprite = lng.undo_rus;
        lng.mix.sprite = lng.mix_rus;
        lng.returnTile.sprite = lng.returnTile_rus;
        lng.magic.sprite = lng.magic_rus;
        lng.extraPlace.sprite = lng.extraPlace_rus;
        lng.townCompletePanelBtn.text = lng.townCompletePanelBtn_rus;
        lng.townCompletePanelBtn.font = lng.townCompletePanelBtnFont_rus;
        lng.rule_1.sprite = lng.rule_1_rus;
        lng.rule_2.sprite = lng.rule_2_rus;
        lng.rule_3.sprite = lng.rule_3_rus;
        lng.rule_4.sprite = lng.rule_4_rus;
        lng.rule_5.sprite = lng.rule_5_rus;
        lng.rule_6.sprite = lng.rule_6_rus;
        lng.rule_7.sprite = lng.rule_7_rus;
    }

    public void ChangeLanguageToEnglish()
    {
        for (int i = 0; i < sizeFilter.childCount; i++)
        {
            if (sizeFilter.GetChild(i).GetComponent<TownUICollection>().isBlocked)
                sizeFilter.GetChild(i).GetComponent<TownUICollection>().townImage.sprite = lng.townData_eng[i].cityOff;
            else
                sizeFilter.GetChild(i).GetComponent<TownUICollection>().townImage.sprite = lng.townData_eng[i].cityOn;
        }

        lng.fontAsset = lng.turkmenFont;
        lng.heartText = lng.heartText_eng;
        lng.placeHolderText = lng.placeholder_eng;
        lng.placeHolder.text = lng.placeholder_eng;
        lng.loadingPanelText.text = lng.loadingPanelText_eng;
        ResourcesData.instance.CheckCountDown(lng.heartText_eng, lng.englishFont);
        lng.play.sprite = lng.play_eng;
        lng.build.sprite = lng.build_eng;
        lng.getStarUI.sprite = lng.getStarUI_eng;
        lng.getStarLeaveBtn.sprite = lng.getStarLeaveBtn_eng;
        lng.collectionHeader.sprite = lng.collectionHeader_eng;
        lng.leaderboardHeader.sprite = lng.leaderboardHeader_eng;
        lng.leaderboardIcon.sprite = lng.leaderboardIcon_eng;
        lng.changeProfilePanel.sprite = lng.changeProfilePanel_eng;
        lng.saveProfileBtn.sprite = lng.saveProfileBtn_eng;
        lng.changeNamePanel.sprite = lng.changeNamePanel_eng;
        lng.changeNameBtn.sprite = lng.changeNameBtn_eng;
        lng.settingsHeader.sprite = lng.settingsHeader_eng;
        lng.settingsPanel.sprite = lng.settingsPanel_eng;
        lng.noInternetPanel.sprite = lng.noInternetPanel_eng;
        lng.noInternetBtn.sprite = lng.noInternetBtn_eng;
        lng.tryAgain.sprite = lng.tryAgain_eng;
        lng.tryAgainBtn.sprite = lng.tryAgainBtn_eng;
        lng.keepPlaying.sprite = lng.keepPlaying_eng;
        lng.playOnBtn.sprite = lng.playOnBtn_eng;
        lng.giveUpBtn.sprite = lng.giveUpBtn_eng;
        lng.leaveUI.sprite = lng.leaveUI_eng;
        lng.leaveBtn.sprite = lng.leaveBtn_eng;
        lng.winTitle.sprite = lng.winTitle_eng;
        lng.winContinueBtn.sprite = lng.winContinueBtn_eng;
        lng.undo.sprite = lng.undo_eng;
        lng.mix.sprite = lng.mix_eng;
        lng.returnTile.sprite = lng.returnTile_eng;
        lng.magic.sprite = lng.magic_eng;
        lng.extraPlace.sprite = lng.extraPlace_eng;
        lng.townCompletePanelBtn.text = lng.townCompletePanelBtn_eng;
        lng.townCompletePanelBtn.font = lng.townCompletePanelBtnFont_eng;
        lng.rule_1.sprite = lng.rule_1_eng;
        lng.rule_2.sprite = lng.rule_2_eng;
        lng.rule_3.sprite = lng.rule_3_eng;
        lng.rule_4.sprite = lng.rule_4_eng;
        lng.rule_5.sprite = lng.rule_5_eng;
        lng.rule_6.sprite = lng.rule_6_eng;
        lng.rule_7.sprite = lng.rule_7_eng;
    }
}
