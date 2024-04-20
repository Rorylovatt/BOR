using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class AudioManager : MonoBehaviour
{
    // Start is called before the first frame update
    PlayerMovement playerMovement;
    Racemanager racemanager;
    public AudioSource[] music = new AudioSource[1];
    public AudioSource[] fx = new AudioSource[10];
    public AudioSource[] trips = new AudioSource[3];
    private int tripSelector;
    private float tripTimer, musicTimer;
    private bool musicReset, tripReset, musicWobbler;
    public AudioClip countdown, go, countdownStart;
    void Start()
    {
        playerMovement = FindAnyObjectByType<PlayerMovement>();
        racemanager = FindAnyObjectByType<Racemanager>();
    }

    // Update is called once per frame
    void Update()
    {
        Music();
        IceSkateFX();
        IdleIceFX();
        BoostTrip();
    }
    public void Music()
    {
        if (racemanager.countDownStart == true)
        {
            if (racemanager.countDownTimer <= 5 && racemanager.countDownTimer > 3)
            {
                if (!fx[6].isPlaying)
                {
                    fx[6].Play();
                }
            }
            if (musicReset)
            {
                music[0].Stop();
            }
            if (!racemanager.raceFinish)
            {
                musicReset = true;
            }
            if ((racemanager.countDownTimer <= 3 && racemanager.countDownTimer > 2.5f) || (racemanager.countDownTimer <= 2 && racemanager.countDownTimer > 1.5f) || (racemanager.countDownTimer <= 1 && racemanager.countDownTimer > 0.5f))
            {
                if (!fx[4].isPlaying)
                {
                    fx[4].Play();
                }
            }
            if ((racemanager.countDownTimer <= 2.1f && racemanager.countDownTimer > 2.01f) || (racemanager.countDownTimer <= 1.1 && racemanager.countDownTimer > 1.01f))
            {
                fx[4].Stop();
            }
            if (racemanager.countDownTimer <= 0 && racemanager.countDownTimer > -0.1f)
            {
                if (!fx[5].isPlaying)
                {
                    fx[5].Play();
                }
            }
        }
        if (racemanager.raceStart == true)
        {
            if (!music[1].isPlaying)
            {
                music[1].Play();
            }
            if (playerMovement.outOfBounds && !playerMovement.boost)
            {
                if (music[1].pitch > 0.85f)
                {
                    music[1].pitch -= 0.2f * Time.deltaTime;
                    musicWobbler = false;
                }
                else
                {
                    if (music[1].pitch < 0.75f && !musicWobbler)
                    {
                        musicWobbler = true;
                    }
                    if (music[1].pitch > 0.85f && musicWobbler)
                    {
                        musicWobbler = false;
                    }
                    if(musicWobbler)
                    {
                        music[1].pitch += 0.2f * Time.deltaTime;
                    }
                    else
                    {
                        music[1].pitch -= 0.2f * Time.deltaTime;

                    }
                }
            }
            else
            {
                musicWobbler = false;
                if (music[1].pitch < 1f)
                {
                    music[1].pitch += 0.5f * Time.deltaTime;
                }
                if (music[1].pitch > 1f)
                {
                    music[1].pitch = 1f;   
                }
            }
        }
        if (racemanager.raceFinish)
        {
            if (musicReset)
            {
                musicTimer = 5;
                musicReset = false;
            }
            musicTimer -= Time.deltaTime;
            music[1].Stop();
            if (racemanager.score <= 50000)
            {
                if (!music[2].isPlaying)
                {
                    music[2].Play();
                }
                if (!fx[7].isPlaying)
                {
                    fx[7].Play();
                }
            }
            if (racemanager.score > 50000)
            {
                if (!music[3].isPlaying)
                {
                    music[3].Play();
                }
                if (!fx[8].isPlaying)
                {
                    fx[8].Play();
                }
            }
            if (musicTimer < 0 && !musicReset)
            {
                musicTimer = 0;
                music[2].Stop();
                music[3].Stop();
                fx[8].Stop();
                fx[7].Stop();
                if (!music[0].isPlaying)
                {
                    music[0].Play();
                }
            }
        }
    }
    public void BoostTrip()
    {
        if (playerMovement.boost)
        {
            if (!fx[3].isPlaying)
            {
                fx[3].Play();
            }
        }
        if (playerMovement.maxSpeedReached)
        {
            if (!trips[tripSelector].isPlaying)
            {
                trips[tripSelector].Play();
            }
            tripReset = true;
        }
        else
        {
            if (tripReset)
            {
                tripSelector += 1;
                tripReset = false;
                if (tripSelector > 3)
                {
                    tripSelector = 0;
                }
            }
        }
    }
    public void IceSkateFX()
    {
        if (racemanager.raceStart && !racemanager.raceFinish)
        {
            if (playerMovement.leftFootControl)
            {
                if (!fx[0].isPlaying)
                {
                    fx[0].Play();
                }
            }
            else if (playerMovement.leftFootControlRelease || playerMovement.maxSpeedReached)
            {
                fx[0].Stop();
            }
            if (playerMovement.rightFootControl)
            {
                if (!fx[1].isPlaying)
                {
                    fx[1].Play();
                }
            }
            else if (playerMovement.rightFootControlRelease || playerMovement.maxSpeedReached)
            {
                fx[1].Stop();
            }
        }


    }
    public void IdleIceFX()
    {
        if (playerMovement.speed > 0.01f)
        {
            if (!fx[2].isPlaying)
            {
                fx[2].Play();
            }
        }
        else
        {
            fx[2].Stop();
        }
        fx[2].pitch = 0.5f + (playerMovement.speed / 15);
    }
}
