using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class HighScores : MonoBehaviour 
{
    const string webURL = "https://focalboard.bellicreative.com/order_guru"; //  Website the keys are for

    public PlayerScore[] scoreList;
    public PlayerScore playerScore;
    DisplayHighscores myDisplay;
    [SerializeField] GameObject board;
    public static HighScores Instance; //Required for STATIC usability

    public ScrollRect scrollRect;
    public float currentPlayer;
    void Awake()
    {
        Instance = this; //Sets Static Instance
        myDisplay = GetComponent<DisplayHighscores>();
    }
    private void Start()
    {
        if (PlayerPrefs.GetString("id") != "")
        {
            if (PlayerPrefs.GetString("oldname") != PlayerPrefs.GetString("id") + PlayerPrefs.GetString("playerName"))
            {
                StartCoroutine(DatabaseUpdate(PlayerPrefs.GetString("oldname"), PlayerPrefs.GetInt("score"), PlayerPrefs.GetInt("profileImg")));
            }
            UploadScore(PlayerPrefs.GetString("oldname"), PlayerPrefs.GetInt("score"), PlayerPrefs.GetInt("profileImg"));
        }
    }
    public static void UploadScore(string username, int score, int iconNum)  //CALLED when Uploading new Score to WEBSITE
    {//STATIC to call from other scripts easily
        Instance.StartCoroutine(Instance.DatabaseUpload(username, score, iconNum)); //Calls Instance
    }

    public void UploadIcon(int iconNum)
    {
        string userame = PlayerPrefs.GetString("oldname");
        int score = PlayerPrefs.GetInt("score");
        Instance.StartCoroutine(Instance.DatabaseUpdate(userame, score, iconNum)); //Calls Instance
    }

    IEnumerator DatabaseUpload(string userame, int score, int iconNum) //Called when sending new score to Website
    {
        WWW www = new WWW(webURL + "/score_profile/" + userame + "/" + score);
        yield return www;
        if (string.IsNullOrEmpty(www.error))
        {
            print("Upload Successful");
            DownloadScores(() => { });
        }
        else print("Error uploading" + www.error);
    }
    IEnumerator DatabaseUpdate(string username, int score, int icon) //update
    {
        string name = PlayerPrefs.GetString("id") + PlayerPrefs.GetString("playerName");
        if (PlayerPrefs.GetString("playerName") == "") name = PlayerPrefs.GetString("id") + "Oýunçy";
        WWW www = new WWW($"{webURL}/update_profile/{username}/{name}/{icon}");
        yield return www;

        if (!string.IsNullOrEmpty(www.error))
        {
            throw new InvalidOperationException($"something went wrong: {www.error}");
        }

        if (string.IsNullOrEmpty(www.error))
        {
            PlayerPrefs.SetString("oldname", name);
            print($"Upload {username} Successful");
            DownloadScores(() => { });
        }
        else print("Error uploading" + www.error);
    }

    public void DownloadScores(Action OnComplete)
    {
        StartCoroutine(DatabaseDownload(OnComplete));
    }

    IEnumerator DatabaseDownload(Action OnComplete)
    {
        string usname = PlayerPrefs.GetString("playerName");
        if (PlayerPrefs.GetString("playerName") == "")
        {
            usname = "Oýunçy";
        }
        WWW www = new WWW($"{webURL}/get/{PlayerPrefs.GetString("id") + usname}"); //Gets top 100
        yield return www;
        if (string.IsNullOrEmpty(www.error))
        {
            var json = JObject.Parse(www.text);
            scoreList = new PlayerScore[json["data"].Count()];
            for (int i = 0; i < json["data"].Count(); i++)
            {
                string username = (string)json["data"][i]["name"];
                int score = int.Parse((string)json["data"][i]["score"]);
                string iconValue = (string)json["data"][i]["icon_id"];
                int iconNum = 0;
                if (int.TryParse(iconValue, out int icon))
                {
                    iconNum = icon;
                }
                scoreList[i] = new PlayerScore(username, score, iconNum);
            }
            if (json["own_list"].Count() != 0)
            {
                playerScore.index = int.Parse((string)json["own_list"][0]["user_index"]);
                playerScore.username = (string)json["own_list"][0]["name"];
                playerScore.score = int.Parse((string)json["own_list"][0]["score"]);
                playerScore.iconNum = int.Parse((string)json["own_list"][0]["icon_id"]);
            }
            if (PlayerPrefs.GetString("id") == "")
            {
                PlayerPrefs.SetString("id", (int.Parse((string)json["total_count"][0]["count"]) + 1).ToString() + "_");
                PlayerPrefs.SetString("oldname", PlayerPrefs.GetString("id")+"Oýunçy");
                string name = PlayerPrefs.GetString("id") + PlayerPrefs.GetString("playerName");
                if (PlayerPrefs.GetString("playerName") == "")
                {
                    name = PlayerPrefs.GetString("id") + "Oýunçy";
                }
                playerScore.index = (int.Parse((string)json["total_count"][0]["count"]) + 1);
                playerScore.username = name;
                playerScore.score = PlayerPrefs.GetInt("score");
                playerScore.iconNum = PlayerPrefs.GetInt("profileImg");
                StartCoroutine(Add(name, PlayerPrefs.GetInt("score"), PlayerPrefs.GetInt("profileImg")));
            }
            myDisplay.SetScoresToMenu(scoreList, playerScore);
            Control.instance.noconnection = false;
            OnComplete();
        }
        else
        {
            Control.instance.noconnection = true;
            print("Error downloading" + www.error);
        }
    }
    public IEnumerator Add(string userame, int score, int iconNum)
    {
        WWW www = new WWW(webURL + "/add/" + userame + "/" + score + "/" + iconNum);
        yield return www;

        if (string.IsNullOrEmpty(www.error))
        {
            print("Upload Successful");
            DownloadScores(() => { });
        }
        else print("Error uploading" + www.error);
    }
}

public struct PlayerScore //Creates place to store the variables for the name and score of each player
{
    public string username;
    public int score;
    public int iconNum;
    public int index;

    public PlayerScore(string _username, int _score, int _iconNum, int _index = 0)
    {
        username = _username;
        score = _score;
        iconNum = _iconNum;
        index = _index;
    }
}
