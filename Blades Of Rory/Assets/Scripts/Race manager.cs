using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Racemanager : MonoBehaviour
{
    PlayerMovement playerMovement;
    private bool raceStart, raceFinish;
    public float timeElapsed;
    public GameObject[] checkPoints = new GameObject[4];
    public bool[] checkPointHit = new bool[4];
    public int laps = 1;
    public Text timerText;

    // Start is called before the first frame update
    void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
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
        timerText.text = "Time : " + timeElapsed.ToString();

    }
}
