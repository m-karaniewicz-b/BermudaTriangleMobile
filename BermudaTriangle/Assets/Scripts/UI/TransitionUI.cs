using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionUI : MonoBehaviour
{
    public List<Material> transitionMats = new List<Material>();

    public const float TRANSITION_DURATION = 1f;

    private SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        //sr.enabled = false;

        GameManager.OnUpgradeMenuTransitionOut += InitTransition;
        GameManager.OnLevelEndComplete += InitTransition;
    }

    public void InitTransition()
    {
        //sr.material = transitionMats[Random.Range(0, transitionMats.Count)];
        StartCoroutine(TransitionCoroutine(TRANSITION_DURATION));
    }

    private IEnumerator TransitionCoroutine(float duration)
    {
        Material mat = sr.material;
        SetProgress(mat, 0);
        
        //sr.enabled = true;

        float timer = 0;

        while (timer < duration)
        {
            mat.SetFloat("Progress", timer / duration);
            //SetProgress(mat, timer / duration);
            timer += Time.unscaledDeltaTime;
            yield return new WaitForEndOfFrame();
        }

        //sr.enabled = false;

        yield return null;
    }


    private void SetProgress(Material mat, float value)
    {
        mat.SetFloat("Progress", value);
    }

}
