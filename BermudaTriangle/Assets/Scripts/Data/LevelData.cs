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
    public BackgroundData background;

    [FoldoutGroup("Main Tabs/Background Tab/Preview Settings")]
    [SerializeField] private static bool backgroundAutoPreview = false;

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

    [FoldoutGroup("Main Tabs/Background Tab/Preview Settings")]
    [Toggle("backgroundAutoPreview"), GUIColor(0.7f, 0.7f, 1)]
    private void ToggleAutoPreview()
    {
        LevelManager.Instance.LoadLevelBackground(this, false);
    }

    // [DisableIf("Toggle")]
    // [HorizontalGroup("Split", 0.5f)]
    // [Button(ButtonSizes.Large), GUIColor(0.4f, 0.8f, 1)]
    // private void FanzyButton1()
    // {
    //     this.Toggle = !this.Toggle;
    // }

    // [HideIf("Toggle")]
    // [VerticalGroup("Split/right")]
    // [Button(ButtonSizes.Large), GUIColor(0, 1, 0)]
    // private void FanzyButton2()
    // {
    //     this.Toggle = !this.Toggle;
    // }



    private void SetNameFromFile()
    {
        levelName = name;
    }

    private void OnValidate()
    {
        if (backgroundAutoPreview && Time.realtimeSinceStartup > 3f)
        {
            PreviewBackgroundInScene();
        }
    }
#endif
}
