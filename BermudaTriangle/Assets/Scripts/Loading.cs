using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Loading : MonoBehaviour
{
    public TextMeshProUGUI loadingText;

    void Start()
    {
        StartCoroutine(LoadSceneAsyncOperation(1));
    }

    IEnumerator LoadSceneAsyncOperation(int sceneIndex)
    {
        AsyncOperation gameLevel = SceneManager.LoadSceneAsync(sceneIndex);

        while (gameLevel.progress < 1)
        {
            loadingText.text = $"{Mathf.Floor(gameLevel.progress * 100)}%";

            yield return new WaitForEndOfFrame();
        }


    }

}
