using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
//using Newtonsoft.Json;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering;

public class PlayerLogin : MonoBehaviour
{
    public TextMeshProUGUI text;
    //public InputField 
    public TMP_InputField emailInput, passwordInput, nameInput;
    public Keyboard keyboard;
    public string testEmail, testPassword;
    public bool emailSwap, login, createAccount;
    public Button selectFirst;

    public GameObject inputMenu, buttons;

    [SerializeField]
    private List<TextMeshProUGUI> names;

    [SerializeField]
    private List<TextMeshProUGUI> scores;
    private void Start()
    {
        selectFirst.Select();
        testPassword = "REW123";
        //keyboard = FindAnyObjectByType<Keyboard>();
        emailInput.text = testEmail;
        passwordInput.text = testPassword;
    }
    private void Update()
    {
        if(login)
        {
            if (!emailSwap)
            {
                testEmail = "rorylovatt@hotmail.co.uk";
                emailInput.text = testEmail;


            }
            if (emailSwap)
            {
                testEmail = "21440992@stu.mmu.ac.uk";
                emailInput.text = testEmail;


            }
        }

    }
    #region LogIn / Create Account
    #region LogIn
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
        //string name = null;
        //if (result.InfoResultPayload.PlayerProfile != null)
        //{
        //    name = result.InfoResultPayload.PlayerProfile.DisplayName;
        //}
        //emailInput.gameObject.SetActive(false);
        //passwordInput.gameObject.SetActive(false);
        //nameInput.gameObject.SetActive(true);
        //keyboard.displayName = true;
        //keyboard.password = false;
    }
    #endregion
    #region CreateAccount
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
            emailInput.gameObject.SetActive(false);
            passwordInput.gameObject.SetActive(false);
            nameInput.gameObject.SetActive(true);
            keyboard.displayName = true;
            keyboard.password = false;
        }
    }
    public void SubmitName()
    {
        var request = new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = nameInput.text,
        };
        PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnDisplayNameUpdate, OnError);

    }
    void OnDisplayNameUpdate(UpdateUserTitleDisplayNameResult result)
    {
        keyboard.loginScreen.SetActive(false);
    }
    #endregion



    void OnError(PlayFabError error)
    {
        if (keyboard.entered)
        {
            text.text = error.ErrorMessage;
        }
    }

    #endregion


    #region Buttons
    public void OnButtonLogIn()
    {
        login = true;
        createAccount = false;
        inputMenu.SetActive(true);
        buttons.SetActive(false);
    }
    public void OnButtonCreateAccount()
    {
        createAccount = true;
        login = false;
        inputMenu.SetActive(true);
        buttons.SetActive(false);
    }
    public void EnterButton()
    {
        if (keyboard.email)
        {
            keyboard.email = false;
            keyboard.password = true;
        }
        if (keyboard.password)
        {
            keyboard.entered = true;
        }
        if (login && keyboard.password)
        {
            LoginButton();
        }
        if (createAccount && keyboard.password)
        {
            RegisterButton();
        }
        if(createAccount && keyboard.displayName)
        {
            SubmitName();
        }
    }

    public void ResetPasswordButton()
    {

    }
    #endregion


    #region Leaderboard
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

        foreach (var item in result.Leaderboard)
        {
            names[item.Position].text = item.DisplayName;
            scores[item.Position].text = item.StatValue.ToString().Substring(1, item.StatValue.ToString().Length - 4)
                + ":" + item.StatValue.ToString().Substring(item.StatValue.ToString().Length - 3, 3);

        }
    }
    #endregion


}
