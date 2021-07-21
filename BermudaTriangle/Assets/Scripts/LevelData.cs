using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName ="New level",menuName = "Data/LevelData")]
public class LevelData : ScriptableObject
{

    [Button(ButtonSizes.Medium), GUIColor(0.4f, 0.4f, 1)]
    private void PreviewInScene()
    {
        FindObjectOfType<LevelManager>().SetLevelMaterials(this);
    }

    public string levelName;

    [InlineEditor(InlineEditorModes.SmallPreview)]
    public Material backgroundMat;
    [InlineEditor(InlineEditorModes.SmallPreview)]
    public Material borderMat;



}
