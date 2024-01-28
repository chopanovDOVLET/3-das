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
    [SerializeField] private Image languages;
    [SerializeField] public bool isLanguageTurkmen;
    [SerializeField] private Sprite languagesTurkmen;
    [SerializeField] private Sprite languagesRussian;
    private int isLanguage;

    private void Start()
    {
        Instance = this;

        isLanguageTurkmen = PlayerPrefs.GetInt("Languages", 1) == 1 ? true : false;

        if (isLanguageTurkmen)
            ChangeLanguageToTurkmen();
        else
            ChangeLanguageToRussian();

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

    // public void ChangeLanguagesRightBtn()
    // {
    //     if (isLanguage + 1 < 3)
    //         isLanguage++;
    //     else
    //         isLanguage = 0;
    // }
    //
    // public void ChangeLanguagesLeftBtn()
    // {
    //     if (isLanguage - 1 > 0)
    //         isLanguage--;
    //     else
    //         isLanguage = 2;
    // }
    public void ChangeLanguages()
    {
        // switch (isLng)
        // {
        //     case 0: ChangeLanguageToRussian();
        //         break;
        //     case 1: ChangeLanguageToTurkmen();
        //         break;
        //     case 2: ChangeLanguageToEnglish();
        //         break;
        // }
        
        if (isLanguageTurkmen) 
            ChangeLanguageToRussian();
        else 
            ChangeLanguageToTurkmen();
    }

    public void ChangeLanguageToTurkmen()
    {
        languages.sprite = languagesTurkmen;
        isLanguageTurkmen = true;
        PlayerPrefs.SetInt("Languages", 1);

        for (int i = 0; i < sizeFilter.childCount; i++)
        {
            //UIController.instance.CityName(lng.townData_tkm[TravelController.instance.currentTravelLevel].cityName);
            if (sizeFilter.GetChild(i).GetComponent<TownUICollection>().isBlocked)
                sizeFilter.GetChild(i).GetComponent<TownUICollection>().townImage.sprite = lng.townData_tkm[i].cityOff;
            else
                sizeFilter.GetChild(i).GetComponent<TownUICollection>().townImage.sprite = lng.townData_tkm[i].cityOn;
        }

        lng.fontAsset = lng.turkmenFont;
        lng.heartText = lng.heartText_tkm;
        ResourcesData.instance.CheckCountDown(lng.heartText_tkm, lng.turkmenFont);
        lng.play.sprite = lng.play_tkm;
        lng.build.sprite = lng.build_tkm;
        lng.getStarUI.sprite = lng.getStarUI_tkm;
        lng.getStarLeaveBtn.sprite = lng.getStarLeaveBtn_tkm;
        lng.collectionHeader.sprite = lng.collectionHeader_tkm;
        lng.settingsHeader.sprite = lng.settingsHeader_tkm;
        lng.settingsPanel.sprite = lng.settingsPanel_tkm;
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
    }

    public void ChangeLanguageToRussian()
    {
        languages.sprite = languagesRussian;
        isLanguageTurkmen = false;
        PlayerPrefs.SetInt("Languages", 0);

        //UIController.instance.CityName(lng.townData_rus[TravelController.instance.currentTravelLevel].cityName);
        for (int i = 0; i < sizeFilter.childCount; i++)
        {  
            if (sizeFilter.GetChild(i).GetComponent<TownUICollection>().isBlocked)
                sizeFilter.GetChild(i).GetComponent<TownUICollection>().townImage.sprite = lng.townData_rus[i].cityOff;
            else
                sizeFilter.GetChild(i).GetComponent<TownUICollection>().townImage.sprite = lng.townData_rus[i].cityOn;
        }

        lng.fontAsset = lng.russianFont;
        lng.heartText = lng.heartText_rus;
        ResourcesData.instance.CheckCountDown(lng.heartText_rus, lng.russianFont);
        lng.play.sprite = lng.play_rus;
        lng.build.sprite = lng.build_rus;
        lng.getStarUI.sprite = lng.getStarUI_rus;
        lng.getStarLeaveBtn.sprite = lng.getStarLeaveBtn_rus;
        lng.collectionHeader.sprite = lng.collectionHeader_rus;
        lng.settingsHeader.sprite = lng.settingsHeader_rus;
        lng.settingsPanel.sprite = lng.settingsPanel_rus;
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
    }

    // public void ChangeLanguageToEnglish()
    // {
    //     
    // }
}
