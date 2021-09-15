using UnityEngine;
using System.Collections;
using Sirenix.OdinInspector;

[RequireComponent(typeof(LineRenderer))]
public class RadiusDisplay : MonoBehaviour
{
    public int segmentsPerRadiusUnit = 20;
    public int defaultRadius = 1;

    private float radius;
    private int segments;
    
    public LineRenderer line;

    void Awake()
    {
        Init();
    }

    private void Init()
    {
        line.useWorldSpace = false;
        line.loop = true;
        SetRadius(defaultRadius);
    }

    public void SetRadius(float newRadius)
    {
        radius = newRadius;
        segments = segmentsPerRadiusUnit * (int)radius;
        line.positionCount = segments;

        UpdatePoints();
    }

    private void UpdatePoints()
    {
        float x;
        float y;

        float angle = 20f;

        for (int i = 0; i < segments; i++)
        {
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;
            y = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;

            line.SetPosition(i, new Vector3(x, y, 0));

            angle += (360f / segments);
        }
    }

    [Button]
    void TestPointsInEditor()
    {
        Init();
    }
}