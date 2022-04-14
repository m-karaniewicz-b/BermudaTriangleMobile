using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

[CreateAssetMenu(fileName = "New level", menuName = "Data/LevelData")]
public class LevelData : ScriptableObject
{
    [Space(15f)]
    [InlineButton("SetNameFromFile", "Set from file")]
    public string levelName;

    [TabGroup("Main Tabs", "Background Tab")]
    [HideLabel]
    public BackgroundData background;

    [TabGroup("Main Tabs", "Enemy Spawns")]
    public List<SpawnGroup> spawnGroups;


#if UNITY_EDITOR

    [FoldoutGroup("Main Tabs/Background Tab/Preview Settings")]
    [HorizontalGroup("Main Tabs/Background Tab/Preview Settings/Horizontal", 0.3f)]
    [Button("@\"Auto Preview: \" + (EditorSettings.Instance.autoPreviewBackgroundData?\"On\":\"Off\")")]
    [PropertyOrder(11)]
    public void ToggleAutoPreview()
    {
        EditorSettings.Instance.autoPreviewBackgroundData =
        !EditorSettings.Instance.autoPreviewBackgroundData;
    }

    [FoldoutGroup("Main Tabs/Background Tab/Preview Settings")]
    [HorizontalGroup("Main Tabs/Background Tab/Preview Settings/Horizontal",0.7f)]
    [PropertyOrder(10)]
    [Button(ButtonSizes.Small), GUIColor(0.8f, 0.8f, 1)]
    private void UpdatePreviewBackgroundInScene()
    {
        LevelManager.Instance.LoadLevelBackground(this, false);
    }

    private void SetNameFromFile()
    {
        levelName = name;
    }

    private void OnValidate()
    {
        if (EditorSettings.Instance.autoPreviewBackgroundData)
        {
            UpdatePreviewBackgroundInScene();
        }
    }
#endif
}
