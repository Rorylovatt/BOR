using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Cinemachine;

public class PlayerFX : MonoBehaviour
{
    // Start is called before the first frame update
    public float fxMultiplyer;
    PlayerMovement playerMovement;
    public Volume volume;
    ChromaticAberration aberration;
    LensDistortion lensDistortion;
    DepthOfField depthOfField;
    Vignette vignette;
    ColorAdjustments adjustments;
    public ParticleSystem[] blades = new ParticleSystem[2];
    public ParticleSystem speedLines;
    public float chromaAberFloat, lensDistFloat, shakeDecel, dofLength, vignetteIntensity, caPostExp, caContrast, outOfBoundsMultiplyer;
    private bool decel, fxFinish, boostFx, shakeBoost, shakeDecelBool;
    public CinemachineVirtualCamera virtualCamera;
    private float ampGain = 0;
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        volume.profile.TryGet(out aberration);
        volume.profile.TryGet(out lensDistortion);
        volume.profile.TryGet(out depthOfField);
        volume.profile.TryGet(out vignette);
        volume.profile.TryGet(out adjustments);

        chromaAberFloat = 0.1f;
        lensDistFloat = 0f;
        CinemachineBasicMultiChannelPerlin ampGain = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

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
            var mainBlades = blades[i].main;
            mainBlades.startLifetime = playerMovement.speed / 5;
        }
        var mainSpeed = speedLines.main;
        if (playerMovement.speed > 18 && !playerMovement.boost)
        {
            if (speedLines.isStopped)
            {
                speedLines.Play();
            }
            mainSpeed.startLifetime = 20;
            mainSpeed.simulationSpeed = 5;

        }
        else if (playerMovement.speed < 18 && !playerMovement.boost)
        {
            speedLines.Stop();
            //mainSpeed.simulationSpeed = 0;

        }
        if (playerMovement.boost)
        {
            if (speedLines.isStopped)
            {
                speedLines.Play();
            }
            mainSpeed.startLifetime = 20;
            mainSpeed.simulationSpeed = 10;

        }
    }
    public void CameraShake()
    {
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = ampGain;
        cinemachineBasicMultiChannelPerlin.m_FrequencyGain = ampGain;
        if (shakeBoost && !shakeDecelBool)
        {
            ampGain = 1f;
            shakeDecelBool = true;
        }
        if (ampGain > 0)
        {
            ampGain -= shakeDecel * Time.deltaTime;
        }
        if (ampGain < 0)
        {
            ampGain = 0;
        }
        if (playerMovement.maxSpeedReached)
        {
            ampGain = 0.5f;
        }

    }
    public void CameraShakeDecel(float decelMultiplyer)
    {

    }
    private void CameraFX()
    {
        CameraShake();
        aberration.intensity.Override(chromaAberFloat);
        lensDistortion.intensity.Override(lensDistFloat);
        depthOfField.focalLength.Override(dofLength);
        vignette.intensity.Override(vignetteIntensity);
        adjustments.postExposure.Override(caPostExp);
        adjustments.contrast.Override(caContrast);


        if (playerMovement.boost && !fxFinish)
        {
            boostFx = true;
            shakeBoost = true;

        }
        if (boostFx)
        {
            CameraFXMultiplyer(2f, 250f, 4f);
        }
        if (!playerMovement.boost)
        {
            shakeBoost = false;
            fxFinish = false;
            shakeDecelBool = false;
        }
        if (!playerMovement.outOfBounds)
        {
            if(caContrast > 12f)
            {
                caContrast -= outOfBoundsMultiplyer * Time.deltaTime;
                dofLength -= outOfBoundsMultiplyer * Time.deltaTime;
                vignetteIntensity -= outOfBoundsMultiplyer / 400 * Time.deltaTime;
                caPostExp += outOfBoundsMultiplyer / 100 * Time.deltaTime;
            }
            else
            {
                caContrast = 12f;
                dofLength = 63f;
                vignetteIntensity = 0.165f;
                caPostExp = 0.5f;

            }

        }
        if(playerMovement.outOfBounds && !playerMovement.boost)
        {
            if(caContrast < 42f)
            {
                caContrast += outOfBoundsMultiplyer * Time.deltaTime;
                dofLength += outOfBoundsMultiplyer * Time.deltaTime;
                vignetteIntensity += outOfBoundsMultiplyer / 400 * Time.deltaTime;
                caPostExp -= outOfBoundsMultiplyer / 100 * Time.deltaTime;
            }
            else
            {
                caContrast = 42f;
                dofLength = 93f;
                vignetteIntensity = 0.24f;
                caPostExp = 0.2f;
            }
        }

    }

    private void CameraFXMultiplyer(float top, float multiplyerMulitplyer, float boostDivider)
    {
        if (!decel)
        {
            chromaAberFloat += fxMultiplyer * multiplyerMulitplyer * Time.deltaTime;
            lensDistFloat -= (fxMultiplyer * multiplyerMulitplyer) / boostDivider * Time.deltaTime;
            if (chromaAberFloat > top)
            {
                decel = true;
            }
        }
        if (decel)
        {
            chromaAberFloat -= fxMultiplyer * (multiplyerMulitplyer / 2) * Time.deltaTime;
            lensDistFloat += (fxMultiplyer * (multiplyerMulitplyer / 2)) / boostDivider * Time.deltaTime;
            if (chromaAberFloat <= 0.1f)
            {
                decel = false;
                fxFinish = true;
                boostFx = false;
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
