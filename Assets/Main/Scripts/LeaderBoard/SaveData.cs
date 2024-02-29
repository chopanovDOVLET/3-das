using TMPro;
using UnityEngine;

public class SaveData : MonoBehaviour
{
    public static SaveData Instance;

    private void Awake()
    {
        Instance = this;
    }
    
    public void SendScore()
    {
        string id = PlayerPrefs.GetString("id");
        string username = PlayerPrefs.GetString("playerName");
        int score = PlayerPrefs.GetInt("score");
        username = (username == "") ? "Oýunçy" : username;
        HighScores.UploadScore( id + username, score);
    }
}