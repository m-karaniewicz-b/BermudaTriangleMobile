using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
public class BackgroundData
{
    public Color color1 = Color.white;
    public Color color2 = Color.white;
    public Color color3 = Color.white;

    [MinMaxSlider(0, 1, true)]
    public Vector2 crossover = new Vector2(0.5f, 0.7f);

    [Range(0, 1)]
    public float circleDistortionRadius = 0.7f;
    public float circleDistortionHardiness = 0.8f;

    public float baseNoiseScale = 10;
    public Vector2 baseNoiseScrollSpeed = new Vector2(1, 1);

    public float overlayTextureStrength = 1;
    public Texture2D overlayTexture;

    public BackgroundData() { }

    public static BackgroundData Lerp(BackgroundData start, BackgroundData end, float value)
    {
        BackgroundData bg = new BackgroundData();

        bg.color1 = Color.Lerp(start.color1, end.color1, value);
        bg.color2 = Color.Lerp(start.color2, end.color2, value);
        bg.color3 = Color.Lerp(start.color3, end.color3, value);

        bg.crossover = Vector2.Lerp(start.crossover, end.crossover, value);
        //bg.crossover = new Vector2(Mathf.Lerp(start.crossover2to3, end.crossover2to3, value);
        //bg.crossover2to3 = Mathf.Lerp(start.crossover2to3, end.crossover2to3, value);
        //bg.crossover1to2 = Mathf.Lerp(start.crossover1to2, end.crossover1to2, value);

        bg.baseNoiseScale = Mathf.Lerp(start.baseNoiseScale, end.baseNoiseScale, value);
        bg.baseNoiseScrollSpeed = Vector2.Lerp(start.baseNoiseScrollSpeed, end.baseNoiseScrollSpeed, value);

        bg.overlayTextureStrength = Mathf.Lerp(start.overlayTextureStrength, end.overlayTextureStrength, value);

        bg.circleDistortionRadius = Mathf.Lerp(start.circleDistortionRadius, end.circleDistortionRadius, value);
        bg.circleDistortionHardiness = Mathf.Lerp(start.circleDistortionHardiness, end.circleDistortionHardiness, value);

        return bg;
    }
}