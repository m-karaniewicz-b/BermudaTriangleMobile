using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName ="New level",menuName = "Data/LevelData")]
public class LevelData : ScriptableObject
{
    [Header("Background")]
    //public string levelName;

    //[InlineEditor(InlineEditorModes.SmallPreview)]
    public Material backgroundMat;
    //[InlineEditor(InlineEditorModes.SmallPreview)]
    public Material borderMat;

    [Header("Enemy Spawn Groups")]
    public List<SpawnGroup> spawnGroups;


    [Button(ButtonSizes.Medium), GUIColor(0.4f, 0.4f, 1)]
    private void PreviewMaterialsInScene()
    {
        FindObjectOfType<LevelManager>().ForceSetLevelMaterials(this);
    }

}
