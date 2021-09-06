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

        GameManager.OnExitMenu += InitTransition;
        GameManager.OnLevelComplete += InitTransition;
    }

    public void InitTransition()
    {
        StartCoroutine(AnimateSpritesCoroutine(sprites, duration));
    }

    private IEnumerator AnimateSpritesCoroutine(Sprite[] animationSprites, float duration)
    {
        Sprite[] spr = animationSprites;

        /*
        int currentFrame = 0;
        for (int i = 0; i < sprites.Length; i++)
        {
            currentFrame = i;

            sr.sprite = sprites[frame];

            frame += (playBackwards ? -1 : 1);

            if (frame == sprites.Length && !playBackwards) frame -= sprites.Length;
            else if (frame - 1 < 0 && playBackwards) frame += sprites.Length;

            yield return new WaitForSecondsRealtime(1 / framesPerSecond);
        }
        */
        float length = spr.Length;
        float tick = duration / length;
        float buffer = 0;
        int currentFrame = 0;

        while ((currentFrame+2) * tick < duration)
        {
            while (buffer > tick)
            {
                buffer -= tick;
                currentFrame++;
            }

            buffer += Time.unscaledDeltaTime;

            sr.sprite = spr[currentFrame];

            yield return new WaitForEndOfFrame();
        }

        sr.sprite = null;

        yield return null;
    }

}
