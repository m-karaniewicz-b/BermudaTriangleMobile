using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;

public class LevelManager : MonoBehaviour
{
    private LevelData currentLevelData;
    private int currentLevelID;

    [Header("References")]
    public BackgroundController backgroundController;

    [Header("Levels")]
    public LevelData[] levelList;

    public static Action<LevelData> OnLevelLoaded;

    private void Awake()
    {
        GameManager.OnGameSessionStart += LoadStartingLevel;
        GameManager.OnUpgradeMenuEnd += LoadLevelNext;
    }

    public void LoadLevelBackground(LevelData data, bool backgroundTransition = true)
    {
        backgroundController.ChangeBackground(data.background, backgroundTransition);
    }

    private void LoadStartingLevel()
    {
        LoadLevel(0, false);
    }

    public void LoadLevel(int id, bool backgroundTransition = true)
    {
        currentLevelData = levelList[id];
        currentLevelID = id;
        LoadLevelBackground(currentLevelData, backgroundTransition);

        OnLevelLoaded?.Invoke(currentLevelData);
    }

    public void LoadLevelNext()
    {
        if (currentLevelID < levelList.Length - 1)
        {
            LoadLevel(currentLevelID + 1);
        }
        else
        {
            LoadLevel(0);
            Debug.Log("Level loop");
        }
    }
}
