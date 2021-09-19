using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Centroid : Singleton<Centroid>
{
    private int currentPointCount;

    [Header("References")]
    [SerializeField] private GameObject controlPointPrefab;
    [SerializeField] private GameObject centroidObject;
    [SerializeField] private LineRenderer lineRend;

    [SerializeField] private Transform[] controlPoints;

    private void Update()
    {
        float sumX = 0;
        float sumY = 0;
        for (int i = 0; i < controlPoints.Length; i++)
        {
            sumX += controlPoints[i].position.x;
            sumY += controlPoints[i].position.y;
        }
        Vector2 centroid = new Vector2(sumX / (controlPoints.Length), sumY / (controlPoints.Length));

        centroidObject.transform.position = centroid;

        UpdateLineRenderer();

    }

    public void InitControlPoints(int controlPointCount)
    {
        ClearControlPoints();

        controlPoints = new Transform[controlPointCount];

        Vector2 offset = new Vector3(0, 3);
        for (int i = 0; i < controlPointCount; i++)
        {
            float angle = i * 360 / controlPointCount;

            Vector2 pos = Quaternion.AngleAxis(angle, Vector3.forward) * offset;

            controlPoints[i] = Instantiate(controlPointPrefab, pos, Quaternion.identity, transform).transform;
            controlPoints[i].name = $"Control Point {i + 1}";
        }

        currentPointCount = controlPointCount;

        UpdateLineRenderer();
    }

    public int GetControlPointCount()
    {
        return currentPointCount;
    }

    private void UpdateLineRenderer()
    {
        lineRend.positionCount = controlPoints.Length;
        for (int i = 0; i < controlPoints.Length; i++)
        {
            lineRend.SetPosition(i, controlPoints[i].transform.position);
        }
    }

    private void ClearControlPoints()
    {
        if (controlPoints == null) return;

        foreach (Transform entry in controlPoints)
        {
            if (entry != null)
                Destroy(entry.gameObject);
        }

        controlPoints = null;
    }

#if UNITY_EDITOR
    //private void ClearControlPointsEditor()
    //{
    //    if (controlPoints == null) return;

    //    int len = controlPoints.Length;
    //    for (int i = len - 1; i >= 0; i--)
    //    {
    //        if (controlPoints[i] != null)
    //            DestroyImmediate(controlPoints[i].gameObject);
    //    }

    //    controlPoints = null;
    //}

    //[Button]
    //private void UpdatePointsInEditor()
    //{
    //    ClearControlPointsEditor();

    //    InitControlPoints(startingPointCount);
    //}
#endif
}
