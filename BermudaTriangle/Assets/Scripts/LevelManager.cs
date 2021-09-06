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
    public SpriteRenderer backgroundSR;
    public SpriteRenderer borderSR;

    [Header("Levels")]
    public LevelData[] levelList;

    public static Action<LevelData> OnLevelLoaded;

    private void Awake()
    {
        GameManager.OnGameSessionStart += LoadStartingLevel;
        GameManager.OnLevelStart += LoadLevelNext;
    }

    public void ForceSetLevelMaterials(LevelData level)
    {
        backgroundSR.material = level.backgroundMat;
        borderSR.material = level.borderMat;
    }

    private void LoadLevelBackground(LevelData newLevel)
    {
        //TODO? Interpolation
        ForceSetLevelMaterials(newLevel);

    }

    private void LoadStartingLevel()
    {
        LoadLevel(0);
    }

    public void LoadLevel(int id)
    {
        currentLevelData = levelList[id];
        currentLevelID = id;
        LoadLevelBackground(currentLevelData);

        OnLevelLoaded?.Invoke(currentLevelData);
    }

    public void LoadLevelNext()
    {
        if (currentLevelID < levelList.Length-1)
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
