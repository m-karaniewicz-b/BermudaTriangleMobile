using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Unity.EditorCoroutines.Editor;

public class EditorUpdate
{
    private void Update()
    {
        if (EditorSettings.Instance.updateTimeControlledBackgroundParametersInEditor)
        {
            BackgroundController.Instance.UpdateTimeControlledParametersEditor();
        }
    }

}
