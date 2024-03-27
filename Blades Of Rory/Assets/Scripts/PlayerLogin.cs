using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
//using Newtonsoft.Json;
using UnityEngine.UI;
using TMPro;

public class PlayerLogin : MonoBehaviour
{
    public TextMeshProUGUI text;
    //public InputField 
    public TMP_InputField emailInput, passwordInput;
    Keyboard keyboard;
    public string testEmail, testPassword;
    public bool emailSwap;
    private void Start()
    {
        testPassword = "REW123";
        keyboard = FindAnyObjectByType<Keyboard>();
        emailInput.text = testEmail;
        passwordInput.text = testPassword;
    }
    private void Update()
    {
        if (emailSwap)
        {
            testEmail = "rorylovatt@hotmail.co.uk";
            emailInput.text = testEmail;


        }
        if (!emailSwap)
        {
            testEmail = "21440992@stu.mmu.ac.uk";
            emailInput.text = testEmail;


        }
    }

    public void RegisterButton()
    {
        if (!keyboard.email)
        {
            var request = new RegisterPlayFabUserRequest
            {
                Email = emailInput.text,
                Password = passwordInput.text,
                RequireBothUsernameAndEmail = false
            };
            PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnError);
        }

    }
    void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        if (keyboard.entered)
        {
            text.text = "Success!";
            keyboard.loginScreen.SetActive(false);

        }
    }
    void OnError(PlayFabError error)
    {
        if (keyboard.entered)
        {
            text.text = error.ErrorMessage;
        }
    }
    public void LoginButton()
    {
        var request = new LoginWithEmailAddressRequest
        {
            Email = emailInput.text,
            Password = passwordInput.text
        };
        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnError);

    }
    public void OnLoginSuccess(LoginResult result)
    {
        text.text = "Login Success!";
        keyboard.loginScreen.SetActive(false);
    }
    public void ResetPasswordButton()
    {

    }
    public void SendLeaderboard(int score)
    {
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate> {
             new StatisticUpdate {
                StatisticName = "High Score", Value = score
            }
            }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderboardUpdate, OnError);

    }
    void OnLeaderboardUpdate(UpdatePlayerStatisticsResult result)
    {
        text.text = "LeaderboardSent";
    }
    public void GetLeaderboard()
    {
        var request = new GetLeaderboardRequest
        {
            StatisticName = "High Score",
            StartPosition = 0,
            MaxResultsCount = 10
        };
        PlayFabClientAPI.GetLeaderboard(request, OnLeaderboardGet, OnError);
    }
    void OnLeaderboardGet(GetLeaderboardResult result)
    {
        foreach(var item in result.Leaderboard)
        {
            Debug.Log(item.Position + " " + item.PlayFabId + " " + item.StatValue);
        }
    }
}
