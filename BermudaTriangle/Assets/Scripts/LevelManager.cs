using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class LevelManager : MonoBehaviour
{
    private LevelData currentLevelData;
    private int currentLevelID;

    public SpriteRenderer backgroundSR;
    public SpriteRenderer borderSR;

    public LevelData[] levelList;

    private void Awake()
    {
        GameManager.OnGameSessionStart += LoadStartingLevel;
        GameManager.OnLevelComplete += LoadLevelNext;
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
    }

    public void LoadLevelNext()
    {
        if (currentLevelID < levelList.Length)
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
