using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionSpriteUI : MonoBehaviour
{
    public Sprite[] sprites;
    public float duration = 1f;
    //public float fps = 30;

    private SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();

        GameManager.OnUpgradeMenuTransitionOut += InitTransition;
        //GameManager
        GameManager.OnLevelEndComplete += InitTransition;
    }

    public void InitTransition()
    {
        StopAllCoroutines();
        StartCoroutine(AnimateSpritesCoroutine(sprites, duration));
    }

    private IEnumerator AnimateSpritesCoroutine(Sprite[] animationSprites, float duration)
    {
        Sprite[] spr = animationSprites;

        float length = spr.Length;
        float tick = duration / length;
        float buffer = 0;
        int currentFrame = 0;

        while (currentFrame < length)
        {
            while (buffer > tick)
            {
                buffer -= tick;
                currentFrame++;
            }

            if (currentFrame >= length) break;

            buffer += Time.unscaledDeltaTime;

            sr.sprite = spr[currentFrame];

            yield return new WaitForEndOfFrame();
        }

        sr.sprite = null;

        yield return null;
    }

}
