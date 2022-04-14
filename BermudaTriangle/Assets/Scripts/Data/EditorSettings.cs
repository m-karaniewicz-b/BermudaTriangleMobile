using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "EditorSettings", menuName = "Settings/Editor Settings")]
public class EditorSettings : ScriptableObject
{
    private static EditorSettings _instance;

    public static EditorSettings Instance
    {
        get
        {
            if(_instance==null)
            {
                string[] s = AssetDatabase.FindAssets("EditorSettingsAsset");
                _instance = AssetDatabase.LoadAssetAtPath<EditorSettings>(
                    AssetDatabase.GUIDToAssetPath(s[0])); 
                if(s.Length>1) Debug.LogError($"There is {s.Length} instances. Only a single instance of Editor Settings Asset should exist.");
                if(_instance==null)
                {
                    Debug.LogError("Editor Settings were not found.");
                    return null;
                }
                else return _instance;
            }
            else
            {
                return _instance;
            }
        }
    }
    
    public bool autoPreviewBackgroundData;
    public bool updateTimeControlledBackgroundParametersInEditor;

}
