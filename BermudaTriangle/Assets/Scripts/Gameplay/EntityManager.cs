using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager : Singleton<EntityManager>
{
    private Rect playArea;

    public List<MovingTarget> respawnableMovingTargets;
    //private Dictionary<EnemyTypeData, int> enemyTypeWeights;
    private List<SpawnGroup> spawnGroups;

    public MovingTarget movingTargetPrefab;

    internal EyeCenter theEye;

    private const float MID_POINT_MAX_OFFSET_X = 0;
    private const float MID_POINT_MAX_OFFSET_Y = 5f;
    private const float INITIAL_SPAWN_COOLDOWN = 5f;
    private const float MOVE_BACK_OUT_OF_SIGHT_OFFSET = 2;

    private float indicatorEndPointOffset;

    private float currentSpawnCooldown;
    private float lastSpawnTime;

    private void Awake()
    {
        theEye = FindObjectOfType<EyeCenter>();

        LevelManager.OnLevelLoaded += LoadSpawnGroupList;

        respawnableMovingTargets = new List<MovingTarget>();
        spawnGroups = new List<SpawnGroup>();
        lastSpawnTime = -Mathf.Infinity;

        currentSpawnCooldown = INITIAL_SPAWN_COOLDOWN;
    }

    private void Start()
    {
        playArea = GameManager.Instance.playArea;

        //Better idea for offset?
        indicatorEndPointOffset = 2 * Mathf.Sqrt(Mathf.Pow(Camera.main.orthographicSize * 2, 2) +
                    Mathf.Pow(Camera.main.orthographicSize * 2 * Camera.main.aspect, 2));
    }

    private void Update()
    {
        if (Time.time - lastSpawnTime > currentSpawnCooldown)
        {
            SpawnGroup spGroup = SelectRandomSpawnGroup();

            currentSpawnCooldown = spGroup.globalSpawnCooldown;

            EnemyTypeData selectedEnemyType = spGroup.enemyType;
            int count = spGroup.instanceCount;
            for (int i = 0; i < count; i++)
            {
                CreateEnemy(selectedEnemyType);
            }

            lastSpawnTime = Time.time;
        }
    }

    private void LoadSpawnGroupList(LevelData data)
    {
        spawnGroups = data.spawnGroups;
    }

    private SpawnGroup SelectRandomSpawnGroup()
    {
        List<SpawnGroup> allowedSpawnGroups = new List<SpawnGroup>();

        foreach (SpawnGroup entry in spawnGroups)
        {
            if(entry.minLevelScore <= GameManager.Instance.scoreCurrentLevel
                && entry.maxLevelScore > GameManager.Instance.scoreCurrentLevel)
            {
                allowedSpawnGroups.Add(entry);
            }
        }

        int sum = 0;

        foreach(SpawnGroup entry in allowedSpawnGroups)
        {
            sum += entry.weight;
        }

        int randomNumber = Random.Range(0,sum) + 1;

        int iter = 0;

        for (int i = 0; i < allowedSpawnGroups.Count; i++)
        {
            iter += spawnGroups[i].weight;
            if(iter >= randomNumber) return spawnGroups[i];
        }

        return null;
    }

    private void CreateEnemy(EnemyTypeData enemyType)
    {
        Vector2[] linePos = null;

        switch (enemyType.trajectory)
        {
            case EntityTrajectory.random:
                {
                    linePos = GenerateLineCrossingPlayArea();
                    break;
                }
            case EntityTrajectory.throughTheEye:
                {
                    linePos = GenerateLineThroughTheEye();
                    break;
                }
            default:
                {
                    break;
                }
        }

        if (linePos == null)
        {
            Debug.LogError("No trajectory determined.");
            return;
        }

        Vector2 start = linePos[0];
        Vector2 end = linePos[1];

        GetNewMovingTargetFromPool().Init(start, end, enemyType);

        //GetNewMovingTargetFromPool().Init(start, end, targetStartDelay / (1 + score / 10f), baseMovingTargetSpeed * (1 + score / 10f));
    }

    private Vector2[] GenerateLineCrossingPlayArea()
    {
        Vector2 originPoint = playArea.center;

        Vector2 midPoint = new Vector2(
            originPoint.x + UnityEngine.Random.Range(-1f, 1f) * MID_POINT_MAX_OFFSET_X,
            originPoint.y + UnityEngine.Random.Range(-1f, 1f) * MID_POINT_MAX_OFFSET_Y);

        Vector2 screenEdgePoint = GetRandomEdgePointFromRect(playArea);
        Vector2 edgeToMid = midPoint - screenEdgePoint;

        Vector2 endPoint = edgeToMid * indicatorEndPointOffset;
        Vector2 screenEdgePointOffsetted = screenEdgePoint - MOVE_BACK_OUT_OF_SIGHT_OFFSET * edgeToMid.normalized;

        Debug.DrawLine(screenEdgePointOffsetted, endPoint, Color.red);

        Vector2[] ret = { screenEdgePointOffsetted, endPoint };
        return ret;
    }
    private Vector2[] GenerateLineThroughTheEye()
    {
        Vector2 midPoint = theEye.transform.position;

        Vector2 screenEdgePoint = GetRandomEdgePointFromRect(playArea);
        Vector2 edgeToMid = midPoint - screenEdgePoint;

        Vector2 endPoint = edgeToMid * indicatorEndPointOffset;
        Vector2 screenEdgePointOffsetted = screenEdgePoint - MOVE_BACK_OUT_OF_SIGHT_OFFSET * edgeToMid.normalized;

        Debug.DrawLine(screenEdgePointOffsetted, endPoint, Color.red);

        Vector2[] ret = { screenEdgePointOffsetted, endPoint };
        return ret;
    }
    private MovingTarget GetNewMovingTargetFromPool()
    {
        MovingTarget ret = null;

        if (respawnableMovingTargets.Count > 0)
        {
            ret = respawnableMovingTargets[0];
            respawnableMovingTargets.Remove(ret);
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
