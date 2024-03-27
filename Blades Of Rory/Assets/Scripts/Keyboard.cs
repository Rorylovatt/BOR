using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System;


public class Keyboard : MonoBehaviour
{
    public TextMeshProUGUI nameText, scoreText, emailText, passwordText, displayNameText;
    public TMP_InputField emailInput, passwordInput, displayNameInput;
    public int score;
    public Button selectFirst;
    public bool active, email, password, displayName, entered;
    public GameObject loginScreen;
    // Start is called before the first frame update
    void Start()
    {
        selectFirst.Select();
        email = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!loginScreen.activeInHierarchy)
        {
            if (score > 1000)
            {
                scoreText.text = "Time : " + score.ToString().Substring(0, score.ToString().Length - 3) + "." + score.ToString().Substring(score.ToString().Length - 3, 3);
            }
        }
        if (loginScreen.activeInHierarchy)
        {
            if (email)
            {
                emailInput.image.color = new Color(0, 0.85f, 1f);
                passwordInput.image.color = Color.white;
                displayNameInput.image.color = Color.white;
            }
            if (password)
            {
                emailInput.image.color = Color.white;
                passwordInput.image.color = new Color(0, 0.85f, 1f);
                displayNameInput.image.color = Color.white;
            }
            if (displayName)
            {
                emailInput.image.color = Color.white;
                passwordInput.image.color = Color.white;
                displayNameInput.image.color = new Color(0, 0.85f, 1f);
            }
        }
    }
    public void OnClickAdd()
    {
        if (!loginScreen.activeInHierarchy)
        {
            if (nameText.text.Length < 12)
            {
                nameText.text += EventSystem.current.currentSelectedGameObject.name.ToString();
            }
        }
        if (loginScreen.activeInHierarchy)
        {
            if (email)
            {
                emailInput.text += EventSystem.current.currentSelectedGameObject.name.ToString().ToLower();
            }
            if (password)
            {
                passwordInput.text += EventSystem.current.currentSelectedGameObject.name.ToString();
            }
            if (displayName)
            {
                displayNameInput.text += EventSystem.current.currentSelectedGameObject.name.ToString();
            }
        }
    }
    public void OnClickDelete()
    {
        if (!loginScreen.activeInHierarchy)
        {
            if (nameText.text.Length > 0)
            {
                nameText.text = nameText.text.Substring(0, nameText.text.Length - 1);
            }
        }
        if (loginScreen.activeInHierarchy)
        {
            if (email)
            {
                if (emailInput.text.Length > 0)
                {
                    emailInput.text = emailInput.text.Substring(0, emailInput.text.Length - 1);
                }
            }
            if (password)
            {
                if (passwordInput.text.Length > 0)
                {
                    passwordInput.text = passwordInput.text.Substring(0, passwordInput.text.Length - 1);
                }
            }
            if (displayName)
            {
                if (displayNameInput.text.Length > 0)
                {
                    displayNameInput.text = displayNameInput.text.Substring(0, displayNameInput.text.Length - 1);
                }
            }

        }
    }
    public void OnEnterEmailPassword()
    {
        if (email)
        {
            email = false;
            password = true;
        }
        if (password)
        {
            entered = true;
            password = false;
            displayName = true;
        }

    }
    public void OnBack()
    {
        if (!email)
        {
            email = true;
        }
    }
}
/*
 rorylovatt@hotmail.co.uk
 REW123
 */
