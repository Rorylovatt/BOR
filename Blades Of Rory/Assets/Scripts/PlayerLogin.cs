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
    private void Start()
    {
        keyboard = FindAnyObjectByType<Keyboard>();
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
        }
    }
    void OnError(PlayFabError error)
    {
        if (keyboard.entered)
        {
            text.text = "Oops";
        }
    }
    public void LoginButton()
    {

    }

    public void ResetPasswordButton()
    {

    }
}
