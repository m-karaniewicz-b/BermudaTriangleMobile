using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BackgroundData
{
    public Vector2 scrollSpeed = new Vector2(1, 1);
    [Range(0, 1)]
    public float crossover1to2 = 0.5f;
    [Range(0, 1)]
    public float crossover2to3 = 0.7f;
    public Color color1 = Color.white;
    public Color color2 = Color.white;
    public Color color3 = Color.white;
    public float baseNoiseScale = 10;
    [Range(0, 1)]
    public float circleDistortionRadius = 0.7f;
    [Range(0, 1)]
    public float circleDistortionHardiness = 0.8f;

    public BackgroundData() { }

    public static BackgroundData Lerp(BackgroundData start, BackgroundData end, float value)
    {
        BackgroundData bg = new BackgroundData();
        bg.scrollSpeed = Vector2.Lerp(start.scrollSpeed, end.scrollSpeed, value);
        bg.crossover1to2 = Mathf.Lerp(start.crossover1to2, end.crossover1to2, value);
        bg.crossover2to3 = Mathf.Lerp(start.crossover2to3, end.crossover2to3, value);
        bg.color1 = Color.Lerp(start.color1, end.color1, value);
        bg.color2 = Color.Lerp(start.color2, end.color2, value);
        bg.color3 = Color.Lerp(start.color3, end.color3, value);
        bg.baseNoiseScale = Mathf.Lerp(start.baseNoiseScale, end.baseNoiseScale, value);
        bg.circleDistortionRadius = Mathf.Lerp(start.circleDistortionRadius, end.circleDistortionRadius, value);
        bg.circleDistortionHardiness = Mathf.Lerp(start.circleDistortionHardiness, end.circleDistortionHardiness, value);

        return bg;
    }
}