using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Racemanager : MonoBehaviour
{
    PlayerMovement playerMovement;
    private bool raceStart;
    public bool raceFinish;
    public float timeElapsed, timeUntilMenu;
    public int score;
    public GameObject[] checkPoints = new GameObject[4];
    public bool[] checkPointHit = new bool[4];
    public int laps = 1;
    public Text timerText;
    public TextMeshProUGUI scoreText;
    public GameObject highScoreMenu;
    Keyboard keyboard;

    // Start is called before the first frame update
    void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
        keyboard = FindObjectOfType<Keyboard>();
    }

    // Update is called once per frame
    void Update()
    {
        if(playerMovement.speed > 0 &&  !raceStart) 
        {
            raceStart = true;
        }
        if(raceStart && !raceFinish)
        {
            timeElapsed += Time.deltaTime;
        }
        if (checkPointHit[3] && laps < 4)
        {
            for(int i = 0; i < checkPointHit.Length;  i++)
            {
                checkPointHit[i] = false;
            }
            laps++;
        }
        if(laps == 4)
        {
            raceFinish = true;
        }
        score = (int)(timeElapsed * 1000f);
        timerText.text = "Time : " + timeElapsed.ToString();
        if (raceStart)
        {
            if(score > 1000)
            {
                scoreText.text = "Score : " + score.ToString().Substring(0, score.ToString().Length - 3) + "." + score.ToString().Substring(score.ToString().Length - 3, 3);
            }
        }
        if(raceFinish) 
        {
            if(timeUntilMenu > 0)
            {
                timeUntilMenu -= Time.deltaTime;

            }
            else
            {
                highScoreMenu.SetActive(true);
               // keyboard.active = true; 
            }
        }
        

    }
}
