using System;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DisplayHighscores : MonoBehaviour
{
    private Color32 defaultColor;
    public static DisplayHighscores Instance;
    
    [SerializeField] HighScores myScores;
    [SerializeField] GameObject contentPref;
    [SerializeField] RectTransform content;
    public ScrollRect scrollRect;
    public float currentPlayer;
    
    [SerializeField] List<TextMeshProUGUI> playerNames;
    [SerializeField] List<TextMeshProUGUI> playerScores;
    [SerializeField] List<Image> playerIcons;
    [SerializeField] List<TextMeshProUGUI> playerPlaces;
    [SerializeField] List<Sprite> iconSprites;

    [Header("Leaderboard UI")] 
    [SerializeField] Sprite userBoxBarUI;
    [SerializeField] Sprite defaultUserBoxBarUI;
    [SerializeField] Sprite[] topUserPlaceUI;

    
    public bool done = false;

    private void Awake()
    {
        Instance = this;
    }

    void Start() //Fetches the Data at the beginning
    {
        defaultColor = new Color32(7, 20, 25, 255);
        for (int i = 0; i <= 100; i++)
        {
            var player = Instantiate(contentPref, content).transform;
            playerPlaces[i] = player.GetChild(0).GetComponent<TextMeshProUGUI>();
            playerIcons[i] = player.GetChild(1).GetComponent<Image>();
            playerNames[i] = player.GetChild(2).GetComponent<TextMeshProUGUI>();
            playerScores[i] = player.GetChild(3).GetComponent<TextMeshProUGUI>();

            playerPlaces[i].text = $"{i + 1}";
            playerNames[i].text = "...";
            if (i < 3)
            {
                var user = player.GetChild(4).GetComponent<Image>();
                user.gameObject.SetActive(true);
                user.sprite = topUserPlaceUI[i];
            }
        }
        content.sizeDelta = new Vector2(content.sizeDelta.x, 0);
        
        StartCoroutine(RefreshHighscores());
    }

    public void SetScoresToMenu(PlayerScore[] playerScoreList, PlayerScore playerScore)
    {
        int userCount = (playerScoreList.Length > 100) ? 100 : playerScoreList.Length;
        int length = (playerScore.index > 100) ? 101 : playerScoreList.Length;
        content.sizeDelta = (content.sizeDelta.y == 0) ? new Vector2(content.sizeDelta.x, 360 * length) : content.sizeDelta;
        
        for (int i = 0; i < userCount; i++)
        {
            playerScores[i].transform.parent.GetComponent<Image>().sprite = defaultUserBoxBarUI;
            playerScores[i].color = defaultColor;
            
            // Convert to players' score into Text
            playerScores[i].text = $"{playerScoreList[i].score}";

            // Convert to players' name into Text
            int index = playerScoreList[i].username.IndexOf("_");
            string username = playerScoreList[i].username;
            string id = "";
            for (int j = 0; j < index + 1; j++)
                id += username[j];
            playerNames[i].text = username.Remove(0, index + 1);

            // Convert to players' icon into Image
            int iconIndex = (playerScoreList[i].icon < 0 | playerScoreList[i].icon > 5) ? 0 : playerScoreList[i].icon;
            playerPlaces[i].text = $"{i + 1}";
            playerIcons[i].sprite = iconSprites[iconIndex];

            // Player's data color change to GREEN and Move to Player's data position 
            if (playerScoreList[i].username == playerScore.username)
            {
                currentPlayer = (i < 5) ? 1f : (1f - (i + 1 - 5) * (1f / (userCount - 10 + 1)));
                ChangeToGreen(i, currentPlayer);
            }
        }
        
        playerScores[100].transform.parent.gameObject.SetActive(false);
        playerScores[100].transform.parent.GetComponent<Image>().sprite = defaultUserBoxBarUI;
        playerScores[100].color = defaultColor;
        
        // Player's place bigger than 100 then set its data to 101 place
        if (playerScore.index > 99)
        {
            playerScores[100].transform.parent.gameObject.SetActive(true);
            playerScores[100].text = $"{playerScore.score}";

            // Convert to player's name into Text
            int index = playerScore.username.IndexOf("_");
            string username = playerScore.username;
            string id = "";
            for (int j = 0; j < index + 1; j++)
                id += username[j];
            playerNames[100].text = username.Remove(0, index + 1);

            // Convert to player's icon into Image
            int iconIndex = (playerScore.icon < 0 | playerScore.icon > 5) ? 0 : playerScore.icon;
            playerPlaces[100].text = $"{playerScore.index}";
            playerIcons[100].sprite = iconSprites[iconIndex];

            // Player's data color change to GREEN and Move to Player's data position 
            ChangeToGreen(100, 0);
        }
    }

    private void ChangeToGreen(int index, float currentPlayer)
    {
        playerScores[index].transform.parent.GetComponent<Image>().sprite = userBoxBarUI;
        playerScores[index].color = Color.white;
        scrollRect.verticalNormalizedPosition = currentPlayer;
    }

    public IEnumerator RefreshHighscores() //Refreshes the Leaderboard's data every 30 seconds
    {
       // while (true)
            if(/*MySelection.instance == null*/ true)
            {
                myScores.DownloadScores(() => { });
                if (myScores.playerScoreList != null)
                    yield return new WaitForSeconds(30);
                else
                    yield return new WaitForSeconds(2);
            }
            else
                yield return new WaitForSeconds(2);
            
    }
}
