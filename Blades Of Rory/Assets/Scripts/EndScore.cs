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
        keyboard = FindObjectOfType<Keyboard>();
        racemanager = FindObjectOfType<Racemanager>();
        //characterSelection= FindObjectOfType<CharacterSelection>();
    }
    public void Update()
    {

        //playerName = characterSelection.playerName;
        // score= characterSelection.score;
        //endScore.text = playerName + "\nYou Scored : " + score.ToString();
        score = racemanager.score;
        keyboard.score = score;
    }
    public void SubmitScore()
    {
        playerName = keyboard.nameText.text;
        submitScoreEvent.Invoke(playerName, score);
    }
}
