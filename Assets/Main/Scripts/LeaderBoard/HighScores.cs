using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class HighScores : MonoBehaviour
{
    private const string webURL = "https://focalboard.bellicreative.com/order_uch";

    public PlayerScore[] playerScoreList;
    public PlayerScore playerScore;

    public bool isOpenedLeaderboard;
    
    [SerializeField] DisplayHighscores myDisplay;
    public static HighScores Instance;

    void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        string id = PlayerPrefs.GetString("id"); // All players count
        string oldName = PlayerPrefs.GetString("oldName"); // Player's name when play offline
        string playerName = PlayerPrefs.GetString("playerName"); // Player's name when play online
        int score = PlayerPrefs.GetInt("score"); // Player's score
        int icon = PlayerPrefs.GetInt("profileIndex"); // Profile images index
        
        if (id != "")
        {
            if (oldName != id + playerName)
                StartCoroutine(DatabaseUpdate(oldName, icon));
            
            UploadScore(oldName, score);
        }
    }
    
    public static void UploadScore(string username, int score) 
    {
        Instance.StartCoroutine(Instance.DatabaseUpload(username, score));
    }

    IEnumerator DatabaseUpload(string username, int score)
    {
        WWW www = new WWW(webURL + "/score_profile/" + username + "/" + score);
        yield return www;
        
        if (string.IsNullOrEmpty(www.error))
        {
            isOpenedLeaderboard = true;
            print("Upload Successful");
            DownloadScores(() => { });
        }
        else
        {
            isOpenedLeaderboard = false;
            print("Error data uploading" + www.error);
        }
    }
    
    public IEnumerator DatabaseUpdate(string oldName, int icon)
    {
        string newName = GetPlayerNewName(PlayerPrefs.GetString("id"));
        WWW www = new WWW($"{webURL}/update_profile/{oldName}/{newName}/{icon}");
        yield return www;

        if (string.IsNullOrEmpty(www.error))
        {
            isOpenedLeaderboard = true;
            print($"Upload {oldName} Successful");
            PlayerPrefs.SetString("oldName", newName);
            DownloadScores(() => { });
        }
        else
        {
            isOpenedLeaderboard = false;
            print("Error data updating" + www.error);
        }
    }

    public void DownloadScores(Action OnComplete)
    {
        StartCoroutine(DatabaseDownload(OnComplete));
    }

    IEnumerator DatabaseDownload(Action OnComplete)
    {
        string userName = GetPlayerNewName("");

        WWW www = new WWW($"{webURL}/get/{PlayerPrefs.GetString("id") + userName}");
        yield return www;
        
        if (string.IsNullOrEmpty(www.error))
        {
            var json = JObject.Parse(www.text);
            playerScoreList = new PlayerScore[json["data"].Count()];
            
            for (int i = 0; i < json["data"].Count(); i++) // Get players' data and set to LIST 
            {
                string playerName = (string)json["data"][i]["name"];
                int score = int.Parse((string)json["data"][i]["score"]); 
                string iconIndex = (string)json["data"][i]["icon_id"]; 
                int icon = int.TryParse(iconIndex, out icon) ? icon : 1;

                playerScoreList[i] = new PlayerScore(playerName, score, icon); // Set players' data to LIST      
            }
            
            if (json["own_list"].Count() != 0) // Get Player's data to STRUCT (current Player)
            {
                playerScore.index = int.Parse((string)json["own_list"][0]["user_index"]);
                playerScore.username = (string)json["own_list"][0]["name"];
                playerScore.score = int.Parse((string)json["own_list"][0]["score"]);
                playerScore.icon = int.Parse((string)json["own_list"][0]["icon_id"]);
            }
            
            if (PlayerPrefs.GetString("id") == "")
            {
                string totalPlayersCount = $"{int.Parse((string)json["total_count"][0]["count"]) + 1}_";
                PlayerPrefs.SetString("id", totalPlayersCount);
                
                string id = PlayerPrefs.GetString("id");
                PlayerPrefs.SetString("oldName", id + "Player");
                
                string newName = GetPlayerNewName(id);

                playerScore.index = (int.Parse((string)json["total_count"][0]["count"]) + 1);
                playerScore.username = newName;
                playerScore.score = PlayerPrefs.GetInt("score");
                playerScore.icon = PlayerPrefs.GetInt("profileIndex");
                
                StartCoroutine(Add(playerScore.username, playerScore.score, playerScore.icon));
            }
            
            myDisplay.SetScoresToMenu(playerScoreList, playerScore);
            OnComplete();
            isOpenedLeaderboard = true; // Hide "No Network Connection Panel"
            print("Succsess downloading");
        }
        else
        {
            isOpenedLeaderboard = false; // Show "No Network Connection Panel"
            print("Error downloading" + www.error);
        }
    }
    
    public IEnumerator Add(string username, int score, int icon) // Add new Player to DataBase
    {
        WWW www = new WWW(webURL + "/add/" + username + "/" + score + "/" + icon);
        yield return www;
        
        if (string.IsNullOrEmpty(www.error))
        {
            Debug.Log("Add ");
            print("Upload Player's data Successful");
            DownloadScores(() => { });
        }
        else 
            print("Error Player's data uploading" + www.error);
    }

    private string GetPlayerNewName(string id) // Get Player's name with ID or without ID
    {
        string username;
        if (PlayerPrefs.GetString("playerName") == "") 
            username = id + "Player";
        else 
            username = id + PlayerPrefs.GetString("playerName");
        return username;
    }
}

public struct PlayerScore //Create place to store the variables for data of each player
{
    public string username;
    public int score;
    public int icon;
    public int index;

    public PlayerScore(string _username, int _score, int _icon, int _index = 0)
    {
        username = _username;
        score = _score;
        icon = _icon;
        index = _index;
    }
}