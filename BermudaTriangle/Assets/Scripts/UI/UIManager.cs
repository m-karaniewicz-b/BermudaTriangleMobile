using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [Header("References")]
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI upgradeMenuMoneyText;

    public GameObject PauseMenuParent;
    public GameObject LifeDisplayParent;
    public GameObject UpgradeMenuParent;
    public GameObject GameOverMenuParent;

    [Header("Colors")]
    public Color fullLifeContainerColor;
    public Color emptyLifeContainerColor;

    private List<Image> lifeContainers;

    private void Awake()
    {
        GameManager.Instance.OnGameSessionStart += UIReset;
        GameManager.Instance.OnGameSessionStart += HideGameOverMenu;
        GameManager.Instance.OnMoneyModified += UpdateMoneyDisplay;
        GameManager.Instance.OnLifeSystemIsActiveModified += SetLivesDisplayActive;
        GameManager.Instance.OnLivesCurrentModified += UpdateLivesCurrentDisplay;
        GameManager.Instance.OnLifeContainersModified += UpdateLivesMaxDisplay;
        GameManager.Instance.OnLevelEndLost += ShowGameOverMenu;
        GameManager.Instance.OnUpgradeMenuStart += ShowUpgradeMenu;
        GameManager.Instance.OnUpgradeMenuEnd += HideUpgradeMenu;

        PauseMenuParent.SetActive(false);
        UpgradeMenuParent.SetActive(false);
        GameOverMenuParent.SetActive(false);

        lifeContainers = new List<Image>();
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

    private void UpdateMoneyDisplay(int newValue)
    {
        moneyText.text = newValue.ToString();
        upgradeMenuMoneyText.text = newValue.ToString();
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

    public void ShowUpgradeMenu()
    {
        UpgradeMenuParent.SetActive(true);
    }

    public void ShowGameOverMenu()
    {
        GameOverMenuParent.SetActive(true);
    }

    public void HideUpgradeMenu()
    {
        UpgradeMenuParent.SetActive(false);
    }

    public void HideGameOverMenu()
    {
        GameOverMenuParent.SetActive(false);
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
