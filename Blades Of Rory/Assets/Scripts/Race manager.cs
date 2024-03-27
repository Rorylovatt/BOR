using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using Cinemachine;

public class Racemanager : MonoBehaviour
{
    PlayerMovement playerMovement;
    private bool countDownStart;
    public bool raceFinish, raceStart, isWorking;
    public float timeElapsed, timeUntilMenu, cameraBlendTime, countDownTimer;
    public int score;
    public GameObject[] checkPoints = new GameObject[4];
    public bool[] checkPointHit = new bool[4];
    public int laps = 1;
    public Text timerText;
    public TextMeshProUGUI scoreText, lapText, countDownText;
    public GameObject highScoreMenu, inGameGUI;
    Keyboard keyboard;
    //public Camera mainCamera, startCamera, finishCamera;
    public CinemachineVirtualCamera mainCamera, startCamera, finishCamera;
    public AnimationCurve curve;
    public CinemachineBrain cineBrain;
    PlayerLogin playerLogin;

    // Start is called before the first frame update
    void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
        keyboard = FindObjectOfType<Keyboard>();
        playerLogin = FindObjectOfType<PlayerLogin>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!keyboard.loginScreen.activeInHierarchy)
        {
            isWorking = true;
            inGameGUI.SetActive(true);
            RaceCondition();
            UpdateGUI();
            CameraControl();
            score = (int)(timeElapsed * 1000f);
        }
    }
    public void UpdateGUI()
    {
        if (laps < 4)
        {
            lapText.text = "Lap " + laps;
        }
        else
        {
            lapText.text = "";
        }
        if (raceStart)
        {
            if (score > 1000)
            {
                scoreText.text = "Time : " + score.ToString().Substring(0, score.ToString().Length - 3) + "." + score.ToString().Substring(score.ToString().Length - 3, 3);
            }
            else if (score > 100)
            {
                scoreText.text = "Time : 0." + score.ToString().Substring(0, score.ToString().Length - 3) + score.ToString().Substring(score.ToString().Length - 3, 3);
            }
            else if (score > 10)
            {
                scoreText.text = "Time : 0.0" + score.ToString();
            }
            else if (score > 0)
            {
                scoreText.text = "Time : 0.00" + score.ToString();
            }
        }
    }
    public void RaceCondition()
    {
        if (!raceStart && Input.GetButtonDown("Jump"))
        {
            countDownStart = true;
            countDownText.text = "";
        }
        if(countDownStart == true)
        {
            countDownTimer -= Time.deltaTime;
            if(countDownTimer <= 3f && countDownTimer > 2.5f)
            {
                countDownText.text = "3";
            }
            if (countDownTimer <= 2f && countDownTimer > 1.5f)
            {
                countDownText.text = "2";
            }
            if (countDownTimer <= 1f && countDownTimer > 0.5f)
            {
                countDownText.text = "1";
            }
            if (countDownTimer <= 0f && countDownTimer > -0.5f)
            {
                countDownText.text = "GO!";
            }
            if(countDownTimer <= -0.5f)
            {
                countDownText.text = "";
            }

            if(countDownTimer <= 0)
            {
                raceStart = true;
            }
        }
        //if (playerMovement.speed > 0 && !raceStart)
        //{
        //    raceStart = true;
        //}
        if (raceStart && !raceFinish)
        {
            timeElapsed += Time.deltaTime;
        }
        if (checkPointHit[3] && laps < 4)
        {
            for (int i = 0; i < checkPointHit.Length; i++)
            {
                checkPointHit[i] = false;
            }
            laps++;
        }
        if (laps == 4)
        {
            raceFinish = true;
        }
        if (raceFinish)
        {
            playerLogin.SendLeaderboard(-score);
            if (timeUntilMenu > 0)
            {
                timeUntilMenu -= Time.deltaTime;

            }
            else
            {
                playerLogin.GetLeaderboard();

                highScoreMenu.SetActive(true);
                // keyboard.active = true; 
            }
        }
    }
    public void CameraControl()
    {
        cineBrain.m_DefaultBlend.m_Time = cameraBlendTime;
        if (countDownStart)
        {
            mainCamera.enabled = true;
            startCamera.enabled = false;
            finishCamera.enabled = false;
        }
        if (raceFinish)
        {
            finishCamera.enabled = true;
            mainCamera.enabled = false;
            startCamera.enabled = false;
        }
        if (!countDownStart && !raceFinish)
        {
            startCamera.enabled = true;
            finishCamera.enabled = false;
            mainCamera.enabled = false;
        }
        if (laps < 3)
        {
            cameraBlendTime = 2f;
        }
        else
        {
            cameraBlendTime = 5f;
        }
    }
}
