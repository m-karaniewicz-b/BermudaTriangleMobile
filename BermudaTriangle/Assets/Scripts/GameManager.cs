using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int score = 0;
    public int hiScore = 0;

    internal bool pauseState;

    internal Rect playArea;
    internal Rect camArea;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI hiScoreText;

    public GameObject PauseMenuParent;

    public MovingTarget movingTargetPrefab;

    private float midPointMaxOffsetX = 0;
    private float midPointMaxOffsetY = 5f;
    private float baseMovingTargetSpeed = 3f;
    private float targetStartDelay = 2f;
    private float spawnCooldown = 5f;
    private int simultaneousSpawnCount = 1;
    private float lastSpawnTime = -Mathf.Infinity;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else Destroy(gameObject);

        float cameraHeight = Camera.main.orthographicSize * 2;
        float cameraWidth = Camera.main.orthographicSize * 2 * Camera.main.aspect;

        Vector2 camAreaSize = new Vector2(cameraWidth, cameraHeight);
        camArea = new Rect((Vector2)Camera.main.transform.position - camAreaSize/2, camAreaSize);

        Vector2 playAreaSize = new Vector2(7f, 16f);
        Vector2 playAreaCenterPos = Vector2.zero;

        playArea = new Rect(playAreaCenterPos - playAreaSize/2, playAreaSize);

        UpdateScoreDisplay();

        PauseMenuParent.SetActive(false);
    }

    private void Start()
    {
        AudioManager.instance.Play("Chain1");
    }

    private void Update()
    {
        UpdateScoreDisplay();

        if (Time.time - lastSpawnTime > spawnCooldown / (1 + score / 10f))
        {
            for (int i = 0; i < simultaneousSpawnCount; i++)
            {
                CreateNewMovingTarget();
            }
            lastSpawnTime = Time.time;
        }
    }
    public void SetPause(bool pause)
    {
        TouchManager.FlushTouchInput();

        pauseState = pause;

        PauseMenuParent.SetActive(pause);

        if (pause)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }
    public void RestartGame()
    {
        Debug.Log("Game Restarted UWU");
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        throw new System.NotImplementedException();
    }
    public void AddPoints(int amountAdded)
    {
        if (amountAdded > 0)
        {
            score += amountAdded;

            if (score > hiScore)
            {
                hiScore = score;
            }
        }
        else if (amountAdded < 0)
        {
            score = Mathf.Clamp(score + amountAdded, 0, int.MaxValue);

        }

        UpdateScoreDisplay();
    }
    private void UpdateScoreDisplay()
    {
        scoreText.text = score.ToString();
        hiScoreText.text = hiScore.ToString();
    }
    private void CreateNewMovingTarget()
    {
        Vector2[] linePos = GenerateTargetLine();
        Vector2 start = linePos[0];
        Vector2 end = linePos[1];

        GetNewMovingTarget().Init(start, end, targetStartDelay / (1 + score / 10f), 
            baseMovingTargetSpeed * (1 + score / 10f));
    }
    private Vector2[] GenerateTargetLine()
    {
        float startOffset = 2;

        Vector2 originPoint = playArea.center;

        Vector2 midPoint = new Vector2(
            originPoint.x + Random.Range(-1f, 1f) * midPointMaxOffsetX, 
            originPoint.y + Random.Range(-1f, 1f) * midPointMaxOffsetY);

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
    private MovingTarget GetNewMovingTarget()
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

        if (Random.value > 0.5f)
        {
            edgePoint.x = Random.Range(-rect.width / 2, rect.width / 2);
            edgePoint.y = ((Random.value > 0.5f) ? rect.height : -rect.height) / 2;
        }
        else
        {
            edgePoint.x = ((Random.value > 0.5f) ? rect.width : -rect.width) / 2;
            edgePoint.y = Random.Range(-rect.height / 2, rect.height / 2);
        }

        return edgePoint;
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(playArea.center,playArea.size);
    }

    /*
    public static Vector2[] GetLineSegmentAndRectIntersectionPoint(Vector2 line, Rect rect)
    {
        Vector2 intersectionPoint1 = Vector2.zero;
        Vector2 intersectionPoint2 = Vector2.zero;




        Vector2[] ret = { intersectionPoint1, intersectionPoint2 };
        return ret;
    }
    */


}
