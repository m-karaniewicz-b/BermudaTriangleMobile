using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else Destroy(gameObject);
    }

    public void UIPauseMenuToggle()
    {
        GameManager.instance.SetPause(!GameManager.instance.pauseState);
    }

    public void UIRestartGame()
    {
        UIPauseMenuToggle();
        GameManager.instance.RestartGame();
    }




}
