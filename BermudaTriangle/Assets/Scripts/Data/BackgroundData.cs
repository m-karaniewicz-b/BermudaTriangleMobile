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

    public float baseNoiseScale = 10;
    public Vector2 baseNoiseScrollSpeed = new Vector2(1, 1);

    public float scalingNoiseScale = 10;
    public float scalingNoiseStrength = 1;
    public Vector2 scalingNoiseScrollSpeed = Vector2.zero;

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

        bg.baseNoiseScale = Mathf.Lerp(start.baseNoiseScale, end.baseNoiseScale, value);
        bg.baseNoiseScrollSpeed = Vector2.Lerp(start.baseNoiseScrollSpeed, end.baseNoiseScrollSpeed, value);

        bg.overlayTextureStrength = Mathf.Lerp(start.overlayTextureStrength, end.overlayTextureStrength, value);
        
        bg.scalingNoiseScale = Mathf.Lerp(start.scalingNoiseScale, end.scalingNoiseScale, value);
        bg.scalingNoiseStrength = Mathf.Lerp(start.scalingNoiseStrength, end.scalingNoiseStrength, value);
        bg.scalingNoiseScrollSpeed = Vector2.Lerp(start.scalingNoiseScrollSpeed, end.scalingNoiseScrollSpeed, value);
        
        return bg;
    }
}