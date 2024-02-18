using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class LanguagesData : MonoBehaviour
{
    public static LanguagesData Instance {  get; private set; }

    [Header("Root")]
    public TMP_FontAsset fontAsset;
    public string heartText;
    public Image play;
    public Image build;
    public Image getStarUI;
    public Image getStarLeaveBtn;
    public Image collectionHeader;
    public Image settingsHeader;
    public Image settingsPanel;
    public Image tryAgain;
    public Image tryAgainBtn;
    public Image keepPlaying;
    public Image playOnBtn;
    public Image giveUpBtn;
    public Image leaveUI;
    public Image leaveBtn;
    public Image winTitle;
    public Image winContinueBtn;
    public Image undo;
    public Image mix;
    public Image returnTile;
    public Image magic;
    public Image extraPlace;
    public TextMeshProUGUI townCompletePanelBtn;
    public Image rule_1;
    public Image rule_2;
    public Image rule_3;
    public Image rule_4;
    public Image rule_5;
    public Image rule_6;
    public Image rule_7;
    
    
    [Header("Turkmen")]
    public List<CollectionLanguageData> townData_tkm = new List<CollectionLanguageData>();
    public TMP_FontAsset turkmenFont;
    public string heartText_tkm;
    public Sprite play_tkm;
    public Sprite build_tkm;
    public Sprite getStarUI_tkm;
    public Sprite getStarLeaveBtn_tkm;
    public Sprite collectionHeader_tkm;
    public Sprite settingsHeader_tkm;
    public Sprite settingsPanel_tkm;
    public Sprite tryAgain_tkm;
    public Sprite tryAgainBtn_tkm;
    public Sprite keepPlaying_tkm;
    public Sprite playOnBtn_tkm;
    public Sprite giveUpBtn_tkm;
    public Sprite leaveUI_tkm;
    public Sprite leaveBtn_tkm;
    public Sprite winTitle_tkm;
    public Sprite winContinueBtn_tkm;
    public Sprite undo_tkm;
    public Sprite mix_tkm;
    public Sprite returnTile_tkm;
    public Sprite magic_tkm;
    public Sprite extraPlace_tkm;
    public string townCompletePanelBtn_tkm;
    public TMP_FontAsset townCompletePanelBtnFont_tkm;
    public Sprite rule_1_tkm;
    public Sprite rule_2_tkm;
    public Sprite rule_3_tkm;
    public Sprite rule_4_tkm;
    public Sprite rule_5_tkm;
    public Sprite rule_6_tkm;
    public Sprite rule_7_tkm;

    [Header("Russian")]
    public List<CollectionLanguageData> townData_rus = new List<CollectionLanguageData>();
    public TMP_FontAsset russianFont;
    public string heartText_rus;
    public Sprite play_rus;
    public Sprite build_rus;
    public Sprite getStarUI_rus;
    public Sprite getStarLeaveBtn_rus;
    public Sprite collectionHeader_rus;
    public Sprite settingsHeader_rus;
    public Sprite settingsPanel_rus;
    public Sprite tryAgain_rus;
    public Sprite tryAgainBtn_rus;
    public Sprite keepPlaying_rus;
    public Sprite playOnBtn_rus;
    public Sprite giveUpBtn_rus;
    public Sprite leaveUI_rus;
    public Sprite leaveBtn_rus;
    public Sprite winTitle_rus;
    public Sprite winContinueBtn_rus;
    public Sprite undo_rus;
    public Sprite mix_rus;
    public Sprite returnTile_rus;
    public Sprite magic_rus;
    public Sprite extraPlace_rus;
    public string townCompletePanelBtn_rus;
    public TMP_FontAsset townCompletePanelBtnFont_rus;
    public Sprite rule_1_rus;
    public Sprite rule_2_rus;
    public Sprite rule_3_rus;
    public Sprite rule_4_rus;
    public Sprite rule_5_rus;
    public Sprite rule_6_rus;
    public Sprite rule_7_rus;
    
    

    private void Start()
    {
        Instance = this;
    }
}

[Serializable]
public class CollectionLanguageData
{
    public Sprite cityOn;
    public Sprite cityOff;
}