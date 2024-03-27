using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System;


public class Keyboard : MonoBehaviour
{
    public TextMeshProUGUI nameText, scoreText, emailText, passwordText;
    public TMP_InputField emailInput, passwordInput;
    public int score;
    public Button selectFirst;
    public bool active, email, password, entered;
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
        if(loginScreen.activeInHierarchy)
        {
            if (email)
            {
                emailInput.image.color = new Color(0, 0.85f, 1f);
                passwordInput.image.color = Color.white;
            }
            if (!email)
            {
                emailInput.image.color = Color.white;
                passwordInput.image.color = new Color(0, 0.85f, 1f);
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
            if(email)
            {
                emailInput.text += EventSystem.current.currentSelectedGameObject.name.ToString();
            }
            if(!email)
            {
                passwordInput.text += EventSystem.current.currentSelectedGameObject.name.ToString();
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
                if(emailInput.text.Length > 0)
                {
                    emailInput.text = emailInput.text.Substring(0, emailInput.text.Length - 1);
                }
            }
            if (!email)
            {
                if (passwordInput.text.Length > 0)
                {
                    passwordInput.text = passwordInput.text.Substring(0, passwordInput.text.Length - 1);
                }
            }
        }
    }
    public void OnEnterEmailPassword()
    {
        if (email)
        {
            email = false;
        }
        if(!email)
        {
            entered = true;
        }

    }
    public void OnBack()
    {
        if(!email)
        {
            email = true;
        }
    }
}
