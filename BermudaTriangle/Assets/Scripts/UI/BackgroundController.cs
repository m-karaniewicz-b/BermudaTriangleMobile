﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : Singleton<BackgroundController>
{
    [SerializeField] private SpriteRenderer targetSR;

    private BackgroundData currentBG;

    private Vector2 baseNoiseScrollCounter;
    private Vector2 baseNoiseScrollSpeed;

    private void Awake()
    {
        baseNoiseScrollCounter = Vector2.zero;
    }

    private void Update()
    {
        UpdateTimeControlledParameters();
    }

    public void ChangeBackground(BackgroundData targetBackground, bool transition)
    {
        if (targetBackground == null)
        {
            Debug.LogError("Missing target background");
            return;
        }

        if (transition)
        {
            StopAllCoroutines();
            StartCoroutine(BackgroundTransitionSequence(targetBackground));
        }
        else
        {
            ApplyBackgroundData(targetBackground, false);
            currentBG = targetBackground;
        }
    }

    private IEnumerator BackgroundTransitionSequence(BackgroundData targetBG)
    {
        float duration = 3f;

        const float midpoint = 0.33f;

        bool textureSet = false;

        float timer = 0;
        while (timer < duration)
        {
            float progress = Mathf.SmoothStep(0, 1, timer / duration);

            ApplyBackgroundData(BackgroundData.Lerp(currentBG, targetBG, progress));

            if (progress >= midpoint && textureSet == false)
            {
                textureSet = true;
                ApplyBackgroundTexture(targetBG.overlayTexture);
            }

            float textureStrength;
            if (timer < midpoint * duration)
                textureStrength = Mathf.SmoothStep(currentBG.overlayTextureStrength, 0, timer / (duration * midpoint));
            else
                textureStrength = Mathf.SmoothStep(0, targetBG.overlayTextureStrength, (timer - duration * midpoint) / (duration * (1 - midpoint)));

            ApplyBackgroundTextureStrength(textureStrength);

            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        ApplyBackgroundData(targetBG);
        currentBG = targetBG;

        yield return null;
    }

    private void UpdateTimeControlledParameters()
    {
        baseNoiseScrollCounter.x += Time.deltaTime * baseNoiseScrollSpeed.x;
        baseNoiseScrollCounter.y += Time.deltaTime * baseNoiseScrollSpeed.y;

        targetSR.material.SetVector("_BaseNoiseScrollOffsetInput", baseNoiseScrollCounter);
    }

    private void ApplyBackgroundTexture(Texture2D tex)
    {
        targetSR.material.SetTexture("_OverlayTexture", tex);
    }

    private void ApplyBackgroundTextureStrength(float strength)
    {
        targetSR.material.SetFloat("_OverlayTextureStrength", strength);
    }

    private void ApplyBackgroundData(BackgroundData data, bool transition = true)
    {
        baseNoiseScrollSpeed = data.baseNoiseScrollSpeed;

        if (Application.isPlaying)
        {
            targetSR.material.SetColor("_Color1", data.color1);
            targetSR.material.SetColor("_Color2", data.color2);
            targetSR.material.SetColor("_Color3", data.color3);
            targetSR.material.SetFloat("_Crossover1to2", data.crossover.x);
            targetSR.material.SetFloat("_Crossover2to3", data.crossover.y);
            targetSR.material.SetFloat("_BaseNoiseScale", data.baseNoiseScale);
            targetSR.material.SetFloat("_CircleDistortionRadius", data.circleDistortionRadius);
            targetSR.material.SetFloat("_CircleDistortionHardiness", data.circleDistortionHardiness);

            if (!transition)
            {
                targetSR.sharedMaterial.SetFloat("_OverlayTextureStrength", data.overlayTextureStrength);
                targetSR.sharedMaterial.SetTexture("_OverlayTexture", data.overlayTexture);
            }

        }
        else
        {
            targetSR.sharedMaterial.SetColor("_Color1", data.color1);
            targetSR.sharedMaterial.SetColor("_Color2", data.color2);
            targetSR.sharedMaterial.SetColor("_Color3", data.color3);
            targetSR.sharedMaterial.SetFloat("_Crossover1to2", data.crossover.x);
            targetSR.sharedMaterial.SetFloat("_Crossover2to3", data.crossover.y);
            targetSR.sharedMaterial.SetFloat("_BaseNoiseScale", data.baseNoiseScale);
            targetSR.sharedMaterial.SetFloat("_CircleDistortionRadius", data.circleDistortionRadius);
            targetSR.sharedMaterial.SetFloat("_CircleDistortionHardiness", data.circleDistortionHardiness);

            targetSR.sharedMaterial.SetFloat("_OverlayTextureStrength", data.overlayTextureStrength);
            targetSR.sharedMaterial.SetTexture("_OverlayTexture", data.overlayTexture);
        }

    }

    public static float SigmoidLogistic(float value, float max, float curve)
    {
        return max / (1 + Mathf.Pow(Mathf.Epsilon, -curve * (value)));
    }


}

