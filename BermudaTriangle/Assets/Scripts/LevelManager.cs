using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    internal LevelData currentLevelData;

    public SpriteRenderer backgroundSR;
    public SpriteRenderer[] borderSR;

    [AssetList(AutoPopulate = true, Path = "LevelData")]
    public LevelData[] levelList;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void SetLevelMaterials(LevelData level)
    {
        backgroundSR.material = level.backgroundMat;
        foreach (SpriteRenderer item in borderSR)
        {
            item.material = level.borderMat;
        }

    }

    public void LoadLevel(LevelData newLevel)
    {
        //?Interpolation
        SetLevelMaterials(newLevel);
    }

    public void LoadLevelNext()
    {

    }
}
