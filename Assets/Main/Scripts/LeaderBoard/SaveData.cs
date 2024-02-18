using TMPro;
using UnityEngine;

public class SaveData : MonoBehaviour
{
    public static SaveData instance;
    public TextMeshProUGUI myScore;
    public TextMeshProUGUI myName;
    public int currentScore;
    public int iconNum=0;
    private void Awake()
    {
        if (instance == null)
            instance = this;
    }
    private void Start()
    {
        
    }

    private void Update()
    {
        //myScore.text = $"Score: {PlayerPrefs.GetInt("highscore")}";
    }
    public void SendScore()
    {
        //Debug.Log(97888);
        //if (currentScore > PlayerPrefs.GetInt("highscore"))
        {
            //Debug.Log(344);
            //PlayerPrefs.SetInt("highscore", currentScore);
            string name = PlayerPrefs.GetString("playerName");
            int score = PlayerPrefs.GetInt("score");
            int iconNum = PlayerPrefs.GetInt("profileImg");
            if (name == "") name = "Oýunçy";
            HighScores.UploadScore(PlayerPrefs.GetString("id") + name, score, iconNum);
        }
    }
}
