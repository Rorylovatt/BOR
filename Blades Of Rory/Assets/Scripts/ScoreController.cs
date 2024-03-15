using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Dan.Main;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class ScoreController : MonoBehaviour
{
    [SerializeField]
    private List<TextMeshProUGUI> names;

    [SerializeField]
    private List<TextMeshProUGUI> scores;

    public TMP_InputField playerNameInput;

    GameObject gameManager;

    public int score, streak;

    public string playerName;

    private string publicLeaderboardKey = "63fc83ae2fc06a2f28ac2c111c6322b0095449714c07a8370d0650ae1bd72e99";

    // secret key 81835fcf421d835840a19bbe370da8ccb4b16a734abb5102e850867f36e8630d38b046b070fe341d9085365b9327d204a0e020de32871d8b371817c04cea5f4e1a62749ec9c5f2ae9fe39181af27c19f2175ff989ce3b258ab320f5312a176fcec6f790083c9496f560419f3ec7a2f7d0e5c7980e3269ed4a21f9b6535723d68


    private void Start()
    {
        gameManager = GameObject.Find("GameManager");
    }
    private void Update()
    {
        GetLeaderboard();

        //SetLeaderboardEntry(playerName, score);
        //SubmitScore();
        //LeaderboardCreator.UploadNewEntry(publicLeaderboardKey, playerName, score, " ");
    }
    public void GetLeaderboard()
    {
        LeaderboardCreator.GetLeaderboard(publicLeaderboardKey, ((msg) =>
        {
            for (int i = 0; i < names.Count; i++)
            {
                names[i].text = msg[i].Username;
                scores[i].text = msg[i].Score.ToString();
            }
        }));
    }
    public void SetLeaderboardEntry(string username, int score)
    {
        LeaderboardCreator.UploadNewEntry(publicLeaderboardKey, username, score, ((msg) =>
        {
            GetLeaderboard();
        }));
    }

    public void ExitClick()
    {
        Application.Quit();
    }
    public void ResetGame()
    {
        Destroy(gameManager);
        SceneManager.LoadScene(0);
    }
}
