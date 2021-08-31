using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public static int livesCurrent;
    public static int livesMax;
    private bool livesIsActive;
    private const int LIFE_CONTAINER_MAXIMUM = 8;
    private const int LEVEL_COMPLETE_THRESHOLD = 10;

    public static int moneyTotal;
    public static int scoreCurrentLevel;
    public static int scoreTotal;

    public static bool pauseState;

    public static Rect playArea;
    public static Rect camArea;

    public static Action OnGameSessionStart;
    public static Action OnLevelStart;
    public static Action OnLevelComplete;
    public static Action OnLevelLost;
    public static Action<int> OnMoneyModified;
    public static Action<bool> OnLivesIsActiveModified;
    public static Action<int> OnLivesCurrentModified;
    public static Action<int> OnLivesMaxModified;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else Destroy(gameObject);

        CalculatePlayArea();
    }

    private void Start()
    {
        AudioManager.instance.Play("Chain1");

        GameSessionInit();
    }

    private void GameSessionInit()
    {
        moneyTotal = 0;
        scoreTotal = 0;
        scoreCurrentLevel = 0;
        livesIsActive = false;

        OnGameSessionStart?.Invoke();

        SetLivesActive(true);
        SetLivesMax(3);
        SetLivesCurrent(3);
    }

    private void LevelStart()
    {
        scoreCurrentLevel = 0;



        OnLevelStart?.Invoke();
    }

    private void LevelComplete()
    {
        SetPauseState(true);



        OnLevelComplete?.Invoke();

        LevelStart();
    }

    private void LevelLost()
    {
        OnLevelLost?.Invoke();
        GameOver();
    }

    private void CalculatePlayArea()
    {
        float cameraHeight = Camera.main.orthographicSize * 2;
        float cameraWidth = Camera.main.orthographicSize * 2 * Camera.main.aspect;

        Vector2 camAreaSize = new Vector2(cameraWidth, cameraHeight);
        camArea = new Rect((Vector2)Camera.main.transform.position - camAreaSize / 2, camAreaSize);

        Vector2 playAreaSize = new Vector2(7f, 16f);
        Vector2 playAreaCenterPos = Vector2.zero;

        playArea = new Rect(playAreaCenterPos - playAreaSize / 2, playAreaSize);

    }

    public void SetPauseState(bool pause)
    {
        InputManager.FlushInput();

        pauseState = pause;

        Time.timeScale = pause ? 0 : 1;
    }

    public void SetMoney(int newAmount)
    {
        moneyTotal = newAmount;
        int diff = newAmount - moneyTotal;
        if (diff > 0) scoreTotal += diff;
        if (diff > 0) scoreCurrentLevel += diff;
        else moneyTotal = Mathf.Clamp(moneyTotal, 0, int.MaxValue);

        if(scoreCurrentLevel >= LEVEL_COMPLETE_THRESHOLD) LevelComplete();

        OnMoneyModified?.Invoke(moneyTotal);
    }

    public void SetLivesActive(bool active)
    {
        livesIsActive = active;

        OnLivesIsActiveModified?.Invoke(active);
    }

    public void SetLivesCurrent(int newCount)
    {
        if(livesIsActive)
        {
            livesCurrent = newCount;
            if (livesCurrent <= 0) LevelLost();
            else if (livesCurrent > livesMax) livesCurrent = livesMax;

            OnLivesCurrentModified?.Invoke(livesCurrent);
        }
    }

    public void SetLivesMax(int newCount)
    {
        if (livesIsActive)
        {
            livesMax = newCount;
            livesMax = Mathf.Clamp(livesMax, 1, LIFE_CONTAINER_MAXIMUM);

            if (livesMax < livesCurrent) SetLivesCurrent(livesMax);
            OnLivesMaxModified?.Invoke(livesMax);
        }
    }

    private void GameOver()
    {
        Debug.Log("Game lost");

        GameSessionInit();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(playArea.center, playArea.size);
    }


}
