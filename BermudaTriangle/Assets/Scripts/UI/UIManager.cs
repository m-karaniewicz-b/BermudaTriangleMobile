using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    [Header("References")]
    public TextMeshProUGUI moneyText;

    public GameObject PauseMenuParent;
    public GameObject LifeDisplayParent;

    [Header("Colors")]
    public Color fullLifeContainerColor;
    public Color emptyLifeContainerColor;

    private List<Image> lifeContainers = new List<Image>();

    private void Awake()
    {
        GameManager.OnGameSessionStart += UIReset;
        GameManager.OnMoneyModified += UpdateMoneyDisplay;
        GameManager.OnLivesIsActiveModified += SetLivesDisplayActive;
        GameManager.OnLivesCurrentModified += UpdateLivesCurrentDisplay;
        GameManager.OnLivesMaxModified += UpdateLivesMaxDisplay;

        PauseMenuParent.SetActive(false);

        foreach (Transform entry in LifeDisplayParent.transform)
        {
            Image img = entry.GetComponent<Image>();
            if (img != null) lifeContainers.Add(img);

        }

        foreach (Image entry in lifeContainers)
        {
            entry.gameObject.SetActive(false);
        }
    }

    private void UIReset()
    {
        UpdateMoneyDisplay(0);
        UpdateLivesCurrentDisplay(0);
        UpdateLivesMaxDisplay(0);
        SetLivesDisplayActive(false);

    }

    private void UpdateMoneyDisplay(int newScore)
    {
        moneyText.text = newScore.ToString();
        //hiScoreText.text = hiScore.ToString();
    }

    private void UpdateLivesCurrentDisplay(int newCount)
    {
        for (int i = 0; i < lifeContainers.Count; i++)
        {
            lifeContainers[i].color = (i < newCount) ? fullLifeContainerColor : emptyLifeContainerColor;
        }
    }

    private void UpdateLivesMaxDisplay(int newCount)
    {
        for (int i = 0; i < lifeContainers.Count; i++)
        {
            lifeContainers[i].gameObject.SetActive(i < newCount);
        }
    }

    private void SetLivesDisplayActive(bool active)
    {
        LifeDisplayParent.SetActive(active);
    }

    public void UIQuitGame()
    {
#if (UNITY_EDITOR)
        UnityEditor.EditorApplication.isPlaying = false;
#elif (UNITY_ANDROID)
        Application.Quit();
#endif

    }
}
