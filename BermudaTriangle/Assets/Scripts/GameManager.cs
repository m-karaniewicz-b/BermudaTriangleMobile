using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private const int LIFE_CONTAINER_MAXIMUM = 8;
    private const int LEVEL_COMPLETE_THRESHOLD = 3;

    public static int livesCurrent;
    public static int livesMax;
    private bool lifeSystemIsActive;

    public static int moneyTotal;
    public static int scoreCurrentLevel;
    public static int scoreTotal;

    public static bool pauseState;
    public static bool levelLostFlag;

    public static Rect playArea;
    public static Rect camArea;

    public static Action OnGameSessionStart;
    public static Action OnLevelStart;
    public static Action OnLevelComplete;
    public static Action OnLevelEnd;
    public static Action OnLevelLost;
    public static Action OnExitMenu;
    public static Action<int> OnMoneyModified;
    public static Action<bool> OnLifeSystemIsActiveModified;
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

        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 30;
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
        lifeSystemIsActive = false;
        levelLostFlag = false;

        OnGameSessionStart?.Invoke();

        SetLifeSystemActive(true);
        SetLivesMax(3);
        SetLivesCurrent(3);
    }

    private void TryLevelComplete()
    {
        StartCoroutine(TryLevelCompleteCoroutine());
    }
    private IEnumerator TryLevelCompleteCoroutine()
    {
        yield return new WaitForSeconds(1f);

        if (levelLostFlag) yield return null;
        OnLevelComplete?.Invoke();

        float transitionDuration = TransitionUI.TRANSITION_DURATION;
        yield return new WaitForSecondsRealtime(transitionDuration / 2);

        SetPauseState(true);
        OnLevelEnd?.Invoke();
        //yield return new WaitForSecondsRealtime(transitionDuration / 2);

        yield return null;
    }

    private void LevelLost()
    {
        levelLostFlag = true;
        SetPauseState(true);
        OnLevelLost?.Invoke();
    }

    public void ProceedToNewSession()
    {
        SetPauseState(false);
        GameSessionInit();
    }

    public void ProceedToNextLevel()
    {
        StartCoroutine(ProceedToNextLevelCoroutine());
    }
    private IEnumerator ProceedToNextLevelCoroutine()
    {
        OnExitMenu?.Invoke();

        float transitionDuration = TransitionUI.TRANSITION_DURATION;
        yield return new WaitForSecondsRealtime(transitionDuration / 2);

        scoreCurrentLevel = 0;
        SetPauseState(false);

        OnLevelStart?.Invoke();
        //yield return new WaitForSecondsRealtime(transitionDuration / 2);

        yield return null;
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
        int diff = newAmount - moneyTotal;
        moneyTotal = newAmount;
        if (diff > 0)
        {
            scoreTotal += diff;
            scoreCurrentLevel += diff;
        }
        else moneyTotal = Mathf.Clamp(moneyTotal, 0, int.MaxValue);

        if (scoreCurrentLevel >= LEVEL_COMPLETE_THRESHOLD) TryLevelComplete();

        OnMoneyModified?.Invoke(moneyTotal);
    }

    public void SetLifeSystemActive(bool active)
    {
        lifeSystemIsActive = active;

        OnLifeSystemIsActiveModified?.Invoke(active);
    }

    public void SetLivesCurrent(int newCount)
    {
        if (lifeSystemIsActive)
        {
            livesCurrent = newCount;
            if (livesCurrent <= 0) LevelLost();
            else if (livesCurrent > livesMax) livesCurrent = livesMax;

            OnLivesCurrentModified?.Invoke(livesCurrent);
        }
    }

    public void SetLivesMax(int newCount)
    {
        if (lifeSystemIsActive)
        {
            livesMax = newCount;
            livesMax = Mathf.Clamp(livesMax, 1, LIFE_CONTAINER_MAXIMUM);

            if (livesMax < livesCurrent) SetLivesCurrent(livesMax);
            OnLivesMaxModified?.Invoke(livesMax);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(playArea.center, playArea.size);
    }


}
