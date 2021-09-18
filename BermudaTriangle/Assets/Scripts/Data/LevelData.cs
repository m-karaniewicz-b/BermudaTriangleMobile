using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "New level", menuName = "Data/LevelData")]
public class LevelData : ScriptableObject
{
    [Space(15f)]
    [InlineButton("SetNameFromFile", "Set from file")]
    public string levelName;

    [TabGroup("Main Tabs","Background Tab")]
    [HideLabel]
    public BackgroundData background = new BackgroundData();

    [FoldoutGroup("Main Tabs/Background Tab/Preview Settings")]
    [SerializeField] private bool BackgroundAutoPreview = true;

    [TabGroup("Main Tabs","Enemy Spawns")]
    public List<SpawnGroup> spawnGroups;


#if UNITY_EDITOR

    //[HideIf("BackgroundAutoPreview")]
    [FoldoutGroup("Main Tabs/Background Tab/Preview Settings")]
    [Button(ButtonSizes.Large), GUIColor(0.7f, 0.7f, 1)]
    private void PreviewBackgroundInScene()
    {
        LevelManager.Instance.LoadLevelBackground(this, false);
    }

    private void SetNameFromFile()
    {
        levelName = name;
    }

    private void OnValidate()
    {
        if (BackgroundAutoPreview && Time.realtimeSinceStartup > 3f)
        {
            PreviewBackgroundInScene();
        }
    }
#endif
}
