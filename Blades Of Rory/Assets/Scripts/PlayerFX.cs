using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PlayerFX : MonoBehaviour
{
    // Start is called before the first frame update
    public float fxMultiplyer;
    PlayerMovement playerMovement;
    public Volume volume;
    ChromaticAberration aberration;
    LensDistortion lensDistortion;
    public ParticleSystem[] blades = new ParticleSystem[2];
    public CallbackOrderAttribute postProcess;
    public float chromaAberFloat, lensDistFloat;
    private bool decel, fxFinish;
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        volume.profile.TryGet(out aberration);
        volume.profile.TryGet(out lensDistortion);

        chromaAberFloat = 0.1f;
        lensDistFloat = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        Particles();
        CameraFX();
    }
    private void Particles()
    {
        for (int i = 0; i < blades.Length; i++)
        {
            blades[i].startLifetime = playerMovement.speed / 5;
        }
    }
    private void CameraFX()
    {
        aberration.intensity.Override(chromaAberFloat);
        lensDistortion.intensity.Override(lensDistFloat);

        if (playerMovement.boost && !fxFinish)
        {
            CameraFXMultiplyer(2f, 2f, 4f);
        }
        if(!playerMovement.boost)
        {
            fxFinish = false;
        }
    }
    private void CameraFXMultiplyer(float top, float multiplyerMulitplyer, float boostDivider)
    {
        if (!decel)
        {
            chromaAberFloat += fxMultiplyer * multiplyerMulitplyer;
            lensDistFloat -= (fxMultiplyer * multiplyerMulitplyer) / boostDivider;
            if (chromaAberFloat > top)
            {
                decel = true;
            }
        }
        if (decel)
        {
            chromaAberFloat -= fxMultiplyer * (multiplyerMulitplyer / 2);
            lensDistFloat += (fxMultiplyer * (multiplyerMulitplyer / 2)) / boostDivider;
            if (chromaAberFloat <= 0.1f)
            {
                decel = false;
                fxFinish = true;
            }
        }
    }
    //private void EffectsReverse(float amount, float attributeFloat, float multiplyer)
    //{
    //    if (!decel)
    //    {
    //        FX = true;
    //        attributeFloat += multiplyer;
    //        if (attributeFloat > amount)
    //        {
    //            decel = true;
    //        }
    //    }
    //    if (decel)
    //    {
    //        attributeFloat -= multiplyer;
    //        if (attributeFloat < min)
    //        {
    //            FX = false;
    //            playerMovement.perfectBool = false;
    //            decel = false;
    //        }
    //    }
    //}
}
