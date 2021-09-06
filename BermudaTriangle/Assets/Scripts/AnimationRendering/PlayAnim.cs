using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnim : MonoBehaviour
{
    public float duration = 1f;

    private SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();

        StartCoroutine(TransitionCoroutine(duration));
    }

    private IEnumerator TransitionCoroutine(float duration)
    {
        Material mat = sr.material;
        SetProgress(mat, 0);
        
        float timer = 0;

        while (timer < duration)
        {
            SetProgress(mat, timer / duration);
            timer += Time.unscaledDeltaTime;
            yield return new WaitForEndOfFrame();
        }

        UnityEditor.EditorApplication.isPlaying = false;

        yield return null;
    }


    private void SetProgress(Material mat, float value)
    {
        mat.SetFloat("Progress", value);
    }

}
