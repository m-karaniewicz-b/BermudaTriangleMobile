using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer targetSR;

    private Vector2 scrollSpeed;
    private Vector2 scrollCounter;

    private BackgroundData currentBG;

    private void Update()
    {
        MoveScrollPosition();
    }

    public void ChangeBackground(BackgroundData targetBackground, bool transition)
    {
        if (transition)
        {
            StopAllCoroutines();
            StartCoroutine(BackgroundTransitionSequence(targetBackground, 2));
        }
        else
        {
            currentBG = targetBackground;
            ApplyBackgroundData(targetBackground);
        }
    }

    private IEnumerator BackgroundTransitionSequence(BackgroundData targetBG, float duration)
    {
        float timer = 0;
        while (timer < duration)
        {
            ApplyBackgroundData(BackgroundData.Lerp(currentBG, targetBG, timer / duration));
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        ApplyBackgroundData(targetBG);
        currentBG = targetBG;

        yield return null;
    }

    private void MoveScrollPosition()
    {
        scrollCounter.x += Time.deltaTime * scrollSpeed.x;
        scrollCounter.y += Time.deltaTime * scrollSpeed.y;
        targetSR.material.SetVector("_ScrollPosition", scrollCounter);
    }

    private void MoveScrollPositionEditor()
    {
        scrollCounter.x += Time.deltaTime * scrollSpeed.x;
        scrollCounter.y += Time.deltaTime * scrollSpeed.y;
        targetSR.sharedMaterial.SetVector("_ScrollPosition", scrollCounter);
    }

    private void ApplyBackgroundData(BackgroundData data)
    {
        scrollSpeed = data.scrollSpeed;

        if(Application.isPlaying)
        {
            targetSR.material.SetFloat("_Crossover1to2", data.crossover1to2);
            targetSR.material.SetFloat("_Crossover2to3", data.crossover2to3);
            targetSR.material.SetColor("_Color1", data.color1);
            targetSR.material.SetColor("_Color2", data.color2);
            targetSR.material.SetColor("_Color3", data.color3);
            targetSR.material.SetFloat("_BaseNoiseScale", data.baseNoiseScale);
            targetSR.material.SetFloat("_CircleDistortionRadius", data.circleDistortionRadius);
            targetSR.material.SetFloat("_CircleDistortionHardiness", data.circleDistortionHardiness);
        }
        else
        {
            targetSR.sharedMaterial.SetFloat("_Crossover1to2", data.crossover1to2);
            targetSR.sharedMaterial.SetFloat("_Crossover2to3", data.crossover2to3);
            targetSR.sharedMaterial.SetColor("_Color1", data.color1);
            targetSR.sharedMaterial.SetColor("_Color2", data.color2);
            targetSR.sharedMaterial.SetColor("_Color3", data.color3);
            targetSR.sharedMaterial.SetFloat("_BaseNoiseScale", data.baseNoiseScale);
            targetSR.sharedMaterial.SetFloat("_CircleDistortionRadius", data.circleDistortionRadius);
            targetSR.sharedMaterial.SetFloat("_CircleDistortionHardiness", data.circleDistortionHardiness);
        }

    }

}

