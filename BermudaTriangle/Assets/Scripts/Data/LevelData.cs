using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName ="New level",menuName = "Data/LevelData")]
public class LevelData : ScriptableObject
{
    public string levelName;

    [Header("Background")]
    public BackgroundData background = new BackgroundData();
    private bool sceneAutoPreview = false;

    [Header("Enemy Spawn Groups")]
    public List<SpawnGroup> spawnGroups;

    //[Button(ButtonSizes.Large), GUIColor(0.7f, 0.7f, 1)]
    private void PreviewBackgroundInScene()
    {
        FindObjectOfType<LevelManager>().LoadLevelBackground(this,false);
    }

    [Button(ButtonSizes.Large), GUIColor(0.7f, 0.7f, 1)]
    private void SetNameFromFile()
    {
        levelName = name;
    }

    private void OnValidate()
    {
        if(sceneAutoPreview)
        {
            PreviewBackgroundInScene();
        }
    }


}
