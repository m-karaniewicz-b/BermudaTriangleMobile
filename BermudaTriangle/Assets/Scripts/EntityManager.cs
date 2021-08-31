using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoBehaviour
{
    private Rect playArea;

    public List<EnemyTypeData> availableEnemyTypes;

    public MovingTarget movingTargetPrefab;

    private float midPointMaxOffsetX = 0;
    private float midPointMaxOffsetY = 5f;

    private float targetStartDelay = 2f;
    private float spawnCooldown = 5f;
    private int simultaneousSpawnCount = 1;

    private float lastSpawnTime = -Mathf.Infinity;

    int score;

    private void Start()
    {
        playArea = GameManager.playArea;
    }

    private void Update()
    {
        score = GameManager.moneyTotal;
        if (Time.time - lastSpawnTime > spawnCooldown / (1 + score / 10f))
        {
            for (int i = 0; i < simultaneousSpawnCount; i++)
            {
                CreateNewMovingTarget();
            }
            lastSpawnTime = Time.time;
        }
    }

    private void CreateNewMovingTarget()
    {
        Vector2[] linePos = GenerateTargetLine();
        Vector2 start = linePos[0];
        Vector2 end = linePos[1];

        //GetNewMovingTargetFromPool().Init(start, end, targetStartDelay / (1 + score / 10f), baseMovingTargetSpeed * (1 + score / 10f));
        GetNewMovingTargetFromPool().Init(start, end, targetStartDelay, availableEnemyTypes[Random.Range(0, availableEnemyTypes.Count)]);
    }

    private Vector2[] GenerateTargetLine()
    {
        float startOffset = 2;

        Vector2 originPoint = playArea.center;

        Vector2 midPoint = new Vector2(
            originPoint.x + UnityEngine.Random.Range(-1f, 1f) * midPointMaxOffsetX,
            originPoint.y + UnityEngine.Random.Range(-1f, 1f) * midPointMaxOffsetY);

        Vector2 screenEdgePoint = GetRandomEdgePointFromRect(playArea);
        Vector2 edgeToMid = midPoint - screenEdgePoint;

        //FIX THIS
        float thisShouldBeLongEnough = 2 * Mathf.Sqrt((Mathf.Pow(Camera.main.orthographicSize * 2, 2) + Mathf.Pow(Camera.main.orthographicSize * 2 * Camera.main.aspect, 2)));

        Vector2 endPoint = edgeToMid * thisShouldBeLongEnough;
        Vector2 screenEdgePointOffsetted = screenEdgePoint - startOffset * edgeToMid.normalized;

        Debug.DrawLine(screenEdgePointOffsetted, endPoint, Color.red);

        Vector2[] ret = { screenEdgePointOffsetted, endPoint };
        return ret;
    }
    private MovingTarget GetNewMovingTargetFromPool()
    {
        MovingTarget ret = null;

        if (MovingTarget.respawnableMovingTargets.Count > 0)
        {
            ret = MovingTarget.respawnableMovingTargets[0];
            MovingTarget.respawnableMovingTargets.Remove(ret);
        }
        else
        {
            ret = Instantiate(movingTargetPrefab);
        }

        return ret;
    }

    private static Vector2 GetRandomEdgePointFromRect(Rect rect)
    {
        Vector2 edgePoint = Vector2.zero;

        if (UnityEngine.Random.value > 0.5f)
        {
            edgePoint.x = UnityEngine.Random.Range(-rect.width / 2, rect.width / 2);
            edgePoint.y = ((UnityEngine.Random.value > 0.5f) ? rect.height : -rect.height) / 2;
        }
        else
        {
            edgePoint.x = ((UnityEngine.Random.value > 0.5f) ? rect.width : -rect.width) / 2;
            edgePoint.y = UnityEngine.Random.Range(-rect.height / 2, rect.height / 2);
        }

        return edgePoint;
    }

}
