using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionSpriteUI : MonoBehaviour
{
    public Sprite[] sprites;
    public const float TRANSITION_DURATION = 1f;

    private SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();

        GameManager.Instance.OnUpgradeMenuTransitionOut += InitTransition;
        //GameManager
        GameManager.Instance.OnLevelEndComplete += InitTransition;
    }

    public void InitTransition()
    {
        StopAllCoroutines();
        StartCoroutine(AnimateSpritesCoroutine(sprites, TRANSITION_DURATION));
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
