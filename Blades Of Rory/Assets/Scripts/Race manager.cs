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
    private bool firstRaceBool, sendScoreBool;
    public bool raceFinish, raceStart, isWorking, resetBool, buttonSelect, beatHighScore, highScoreUpdate, countDownStart;
    public float timeElapsed, timeUntilMenu, cameraBlendTime, countDownTimer, resetTimer;
    public int score, finalScore;
    public GameObject[] checkPoints = new GameObject[4];
    public bool[] checkPointHit = new bool[4];
    public int laps = 1;
    public Text timerText;
    public TextMeshProUGUI scoreText, lapText, countDownText, finalScoreText;
    public GameObject highScoreMenu, inGameGUI;
    public Keyboard keyboard;
    //public Camera mainCamera, startCamera, finishCamera;
    public CinemachineVirtualCamera mainCamera, startCamera, finishCamera;
    public AnimationCurve curve;
    public CinemachineBrain cineBrain;
    PlayerLogin playerLogin;
    public Button playAgain;
    public GameObject player;
    public Transform playerTransform;
    private float timeUntilMenuReset, countDownTimerReset, resetTimerReset;

    void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
        //keyboard = FindObjectOfType<Keyboard>();
        playerLogin = FindObjectOfType<PlayerLogin>();
        playAgain.Select();
        timeUntilMenuReset = timeUntilMenu;
        countDownTimerReset = countDownTimer;
        resetTimerReset = resetTimer;
        firstRaceBool = true;
    }

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
            if (resetBool)
            {
                resetTimer -= Time.deltaTime;
                if (resetTimer < 0)
                {
                    resetBool = false;
                    resetTimer = resetTimerReset;
                    countDownText.text = "Can you do any better " + playerLogin.usernameToDisplay + "?\nPress 'A' to start";
                }
            }
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
            else if (score >= 0)
            {
                scoreText.text = "Time : 0.00" + score.ToString();
            }
        }
    }
    public void RaceCondition()
    {
        if (!raceStart && !countDownStart && !resetBool && firstRaceBool)
        {
            countDownText.text = "Welcome " + playerLogin.usernameToDisplay + "\nPress 'A' to start";

        }
        if (!raceStart && Input.GetButtonDown("Jump") && !resetBool)
        {
            firstRaceBool = false;
            countDownStart = true;
            countDownText.text = "";
        }
        if (countDownStart == true)
        {
            countDownTimer -= Time.deltaTime;
            if (countDownTimer <= 3f && countDownTimer > 2.5f)
            {
                countDownText.text = "3";
            }
            else if (countDownTimer <= 2f && countDownTimer > 1.5f)
            {
                countDownText.text = "2";
            }
            else if (countDownTimer <= 1f && countDownTimer > 0.5f)
            {
                countDownText.text = "1";
            }
            else if (countDownTimer <= 0f && countDownTimer > -0.5f)
            {
                countDownText.text = "GO!";
            }
            else if (countDownTimer <= -0.5f)
            {
                countDownText.text = "";
            }
            else
            {
                countDownText.text = "";

            }

            if (countDownTimer <= 0)
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
            sendScoreBool = true;
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
            for (int i = 0; i < checkPointHit.Length; i++)
            {
                checkPointHit[i] = false;
            }
            raceFinish = true;
        }
        if (raceFinish)
        {
            timeElapsed += 0;
            inGameGUI.SetActive(false);
            if(timeUntilMenu < 1.5f && timeUntilMenu < 1f && !sendScoreBool)
            {
                playerLogin.GetLeaderboardAroundPlayer();

            }
            if (timeUntilMenu < 1 && timeUntilMenu < 0.5f)
            {
                if (playerLogin.currentHighScore >= score)
                {
                    highScoreUpdate = true;
                }
                if (playerLogin.currentHighScore < score)
                {
                    highScoreUpdate = false;
                }
                playerLogin.SendLeaderboard(-score);
                if (sendScoreBool)
                {
                    playerLogin.currentHighScore = score;
                    sendScoreBool = false;
                }
                //playerLogin.GetLeaderboard();
            }
            if (timeUntilMenu > 0)
            {
                timeUntilMenu -= Time.deltaTime;

            }
            else
            {
                playerLogin.GetLeaderboardAroundPlayer();
                highScoreMenu.SetActive(true);
                if (playerLogin.createAccountPublic)
                {
                    playerLogin.GetLeaderboard();
                    finalScoreText.text = "Not bad for a rookie! " + "\nTime : " + score.ToString().Substring(0, score.ToString().Length - 3) + ":" + score.ToString().Substring(score.ToString().Length - 3, 3)
                        + "\nCurrent rank : " + playerLogin.currentRank.ToString();
                }
                else
                {
                    if (highScoreUpdate)
                    {
                        playerLogin.GetLeaderboard();

                        finalScoreText.text = "You beat your best time!!" +
                            "\nTime : " + score.ToString().Substring(0, score.ToString().Length - 3) + ":" + score.ToString().Substring(score.ToString().Length - 3, 3)
                            + "\nCurrent rank : " + playerLogin.currentRank.ToString();

                    }
                    if (!highScoreUpdate)
                    {
                        playerLogin.GetLeaderboard();

                        finalScoreText.text = "You didn't beat your best time" +
                             "\nTime : " + score.ToString().Substring(0, score.ToString().Length - 3) + ":" + score.ToString().Substring(score.ToString().Length - 3, 3)
                            + "\nBest Time : " + playerLogin.currentHighScore.ToString().Substring(0, playerLogin.currentHighScore.ToString().Length - 3) +
                            ":" + playerLogin.currentHighScore.ToString().Substring(playerLogin.currentHighScore.ToString().Length - 3, 3)
                            + "\nHighest rank : " + playerLogin.currentRank.ToString();
                    }
                }

                // keyboard.active = true; 
            }
            if (!buttonSelect)
            {
                playAgain.Select();
                buttonSelect = true;
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
    public void OnButtonQuit()
    {
        Application.Quit();
    }
    public void OnButtonRestart()
    {
        inGameGUI.SetActive(true);
        player.transform.position = playerTransform.position;
        player.transform.rotation = playerTransform.rotation;
        startCamera.enabled = true;
        finishCamera.enabled = false;
        mainCamera.enabled = false;
        countDownStart = false;
        raceFinish = false;
        raceStart = false;
        laps = 1;
        timeUntilMenu = timeUntilMenuReset;
        countDownTimer = countDownTimerReset;
        highScoreMenu.SetActive(false);
        timeElapsed = 0;
        playerMovement.animator.SetBool("Win", false);
        playerMovement.animator.SetBool("Lose", false);
        scoreText.text = "Time : 0.000";
        resetBool = true;
        buttonSelect = false;
        playerLogin.createAccountPublic = false;
        playerMovement.perfectCounter = 0;
    }
}
