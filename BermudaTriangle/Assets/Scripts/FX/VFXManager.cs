using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXManager : MonoBehaviour
{
    public static VFXManager instance;

    private static Transform particleParent;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        particleParent = new GameObject().transform;
        particleParent.name = "ParticleParent";
    }

    public ParticleSystem hitVFX;
    public ParticleSystem missVFX;
    public ParticleSystem dropVFX;
    public ParticleSystem clickEmptyVFX;

    public Sprite spriteRadialGradientReverse;

    public static void SpawnParticleOneshot(ParticleSystem particle, Vector2 position)
    {
        if (particle == null) return;
        ParticleSystem p = Instantiate(particle, position, Quaternion.identity, particleParent);
        var main = p.main;
        main.loop = false;
        main.stopAction = ParticleSystemStopAction.Destroy;
        p.Play();
    }

    public static void SpawnParticleExplosionOneShot(ParticleSystem particle, Vector2 position, float radius)
    {
        if (particle == null) return;
        ParticleSystem p = Instantiate(particle, position, Quaternion.identity, particleParent);
        var main = p.main;
        main.loop = false;
        main.stopAction = ParticleSystemStopAction.Destroy;

        var shape = p.shape;
        shape.radius = radius;
        p.Play();
    }

    public static void SpawnParticleExplosionOneShot(ParticleSystem particle, Vector2 position, 
        float radius, float speedMult, float emissionMult, Color colorMin, Color colorMax)
    {
        if (particle == null) return;
        ParticleSystem p = Instantiate(particle, position, Quaternion.identity, particleParent);
        var main = p.main;
        main.loop = false;
        main.stopAction = ParticleSystemStopAction.Destroy;

        main.startSpeed = main.startSpeed.constant * speedMult;
        ParticleSystem.MinMaxGradient startColor = new ParticleSystem.MinMaxGradient(colorMin, colorMax);
        main.startColor = startColor;

        var emission = p.emission;
        var burst = emission.GetBurst(0);
        burst.count = burst.count.constant * emissionMult;

        var shape = p.shape;
        shape.radius = radius;
        p.Play();
    }

    public void SpawnSpriteOneShot(Sprite sprite, Vector2 position , float scale, Color color, float fadeDuration)
    {
        SpriteRenderer sr = new GameObject().AddComponent<SpriteRenderer>();
        sr.transform.parent = particleParent;
        sr.transform.position = position;
        sr.transform.localScale = new Vector3(scale,scale,0);
        sr.sprite = sprite;

        StartCoroutine(FadeCoroutine(sr, color, new Color(color.r, color.g, color.b, 0), fadeDuration, true));
    }

    private static IEnumerator FadeCoroutine(SpriteRenderer targetSR, Color startColor, Color endColor,
        float fadeDuration, bool destroyOnFinish = true)
    {
        float timer = 0;
        while (timer < fadeDuration)
        {
            targetSR.color = Color.Lerp(startColor, endColor, timer / fadeDuration);
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        if (destroyOnFinish) Destroy(targetSR.gameObject);

        yield return null;
    }

    public static void DestroyAllParticles()
    {
        Destroy(particleParent.gameObject);
        particleParent = new GameObject().transform;
        particleParent.name = "ParticleParent";
    }


}
