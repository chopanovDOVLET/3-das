using DG.Tweening;
using QFSW.QC.Utilities;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class DisplayHighscores : MonoBehaviour
{
    public List<TextMeshProUGUI> rNames;
    public List<TextMeshProUGUI> rScores;
    public List<Image> rImages;
    public List<TextMeshProUGUI> rNums;
    public List<Sprite> sprites;
    List<Image> imgs = new List<Image>();
    [SerializeField] RectTransform content;
    public bool done = false;
    HighScores myScores;

    void Start() //Fetches the Data at the beginning
    {
        content.sizeDelta = new Vector2(content.sizeDelta.x, 0);
        for (int i = 0; i <= 100; i++)
        {
            rNames[i].text = "Garaşylýar...";
            imgs.Add(rNames[i].transform.parent.transform.GetComponent<Image>());
        }
        myScores = GetComponent<HighScores>();
        StartCoroutine("RefreshHighscores");
    }
    public void SetScoresToMenu(PlayerScore[] highscoreList, PlayerScore playerScore) //Assigns proper name and score for each text value
    {
        int countUser = highscoreList.Length;
        if (highscoreList.Length > 100)
            countUser = 100;
        int length = highscoreList.Length;
        if (playerScore.index > 100) length = 101;
        if (content.sizeDelta.y == 0) content.sizeDelta = new Vector2(content.sizeDelta.x, 360 * length);
        for (int i = 0; i < countUser; i++)
        {
            rNames[i].text = i + 1 + ". ";
            if (highscoreList.Length > i)
            {
                rScores[i].text = highscoreList[i].score.ToString();
                int index = highscoreList[i].username.IndexOf("_");
                string name = "";
                string id = "";
                for (int j = index + 1; j < highscoreList[i].username.Length; j++)
                {
                    name += highscoreList[i].username[j];
                }
                for (int j = 0; j < index + 1; j++)
                {
                    id += highscoreList[i].username[j];
                }
                rNames[i].text = name;
                int Num = highscoreList[i].iconNum;
                if (Num < 0 | Num > 5) Num = 0;
                rNums[i].text = (i + 1).ToString();
                rImages[i].sprite = sprites[Num];
                if (highscoreList[i].username == playerScore.username)
                {
                    imgs[i].DOFade(1, 0);
                    rNames[i].color = new Color32(96, 219, 0, 255);
                    rScores[i].color = new Color32(96, 219, 0, 255);
                    if (i < 5)
                        myScores.currentPlayer = 1f;
                    else
                        myScores.currentPlayer = 1f - (i + 1 - 5) * (1f / (countUser - 10 + 1));

                    if (!done)
                    {
                        myScores.scrollRect.verticalNormalizedPosition = myScores.currentPlayer;
                        done = true;
                    }

                }
            }
        }
        if (playerScore.index > 100)
        {
            rScores[100].text = playerScore.score.ToString();
            int index = playerScore.username.IndexOf("_");
            string name = "";
            string id = "";
            for (int j = index + 1; j < playerScore.username.Length; j++)
            {
                name += playerScore.username[j];
            }
            for (int j = 0; j < index + 1; j++)
            {
                id += playerScore.username[j];
            }
            rNames[100].text = name;
            int Num = playerScore.iconNum;
            if (Num < 0 | Num > 5) Num = 0;
            rNums[100].text = (playerScore.index).ToString();
            rImages[100].sprite = sprites[Num];
            imgs[100].DOFade(1, 0);
            rNames[100].color = new Color32(96, 219, 0, 255);
            rScores[100].color = new Color32(96, 219, 0, 255);
            myScores.currentPlayer = 0;
            if (!done)
            {
                myScores.scrollRect.verticalNormalizedPosition = myScores.currentPlayer;
                done = true;
            }
        }
    }
    void Update()
    {
        if(done & transform.localScale == Vector3.zero)
        {
            done = false;
        }
    }
    IEnumerator RefreshHighscores() //Refreshes the scores every 30 seconds
    {
        while (true)
        {
            if(MySelection.instance == null)
            {
                myScores.DownloadScores(() => { });
                if (myScores.scoreList != null)
                    yield return new WaitForSeconds(30);
                else
                {
                    yield return new WaitForSeconds(2);
                }
            }
            else
                yield return new WaitForSeconds(2);

        }
    }
}
