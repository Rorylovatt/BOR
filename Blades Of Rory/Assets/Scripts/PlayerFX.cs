using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerFX : MonoBehaviour
{
    // Start is called before the first frame update
    PlayerMovement playerMovement;
    public ParticleSystem[] blades = new ParticleSystem[2];
    public CallbackOrderAttribute postProcess;
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < blades.Length; i++)
        {
            blades[i].startLifetime = playerMovement.speed / 5;
        }
    }
}
