using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class PlayAnim : MonoBehaviour
{
    public float delay = 0f;
    public float duration = 1f;
    public bool autoplay = true;

    private SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        if (autoplay) StartAnimation();
    }

    [Button]
    private void StartAnimation()
    {
        StartCoroutine(TransitionCoroutine(duration));
    }

    private IEnumerator TransitionCoroutine(float duration)
    {
        yield return new WaitForSeconds(delay);

        Material mat = sr.material;
        SetProgress(mat, 0);
        
        float timer = 0;

        while (timer < duration)
        {
            SetProgress(mat, timer / duration);
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        yield return null;
    }


    private void SetProgress(Material mat, float value)
    {
        mat.SetFloat("Progress", value);
    }

}
