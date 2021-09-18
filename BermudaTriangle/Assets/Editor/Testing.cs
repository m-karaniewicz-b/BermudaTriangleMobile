using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector.Editor;
using Sirenix.OdinInspector;
using UnityEditor;

public class Testing : OdinEditorWindow
{
    [MenuItem("Custom/Testing Window")]
    private static void OpenWindow()
    {
        GetWindow<Testing>().Show();
    }

    [Button]
    private float CalculateEuclideanDistance(Vector2 p, Vector2 q)
    {
        return Helpers.EuclideanDistance(p, q);
    }

}
