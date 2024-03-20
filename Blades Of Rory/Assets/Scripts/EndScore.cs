using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.Events;

public class EndScore : MonoBehaviour
{
    public int score, streak;
    public string playerName;
    Racemanager racemanager;
    Keyboard keyboard;
    //public TextMeshProUGUI endScore;

    public UnityEvent<string, int> submitScoreEvent;
    //CharacterSelection characterSelection;

    private void Start()
    {
        keyboard = FindFirstObjectByType<Keyboard>();
        racemanager = FindObjectOfType<Racemanager>();
        //characterSelection= FindObjectOfType<CharacterSelection>();
    }
    public void Update()
    {
        score = racemanager.score;
        playerName = keyboard.name.text;
        //playerName = characterSelection.playerName;
       // score= characterSelection.score;
        //endScore.text = playerName + "\nYou Scored : " + score.ToString();
    }
    public void SubmitScore()
    {
        submitScoreEvent.Invoke(playerName, score);
    }
}
