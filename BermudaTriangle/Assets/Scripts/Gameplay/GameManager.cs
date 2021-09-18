using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : Singleton<GameManager>
{
    private const int LIFE_CONTAINER_MAXIMUM = 8;
    private const int LEVEL_COMPLETE_THRESHOLD = 1;

    public static int livesCurrent;
    public static int lifeContainers;
    private bool lifeSystemIsActive;

    public static int moneyTotal;
    public static int scoreCurrentLevel;
    public static int scoreTotal;

    public static bool pauseState;
    public static bool levelLostFlag = false;
    public static bool levelFinishFlag = false;
    public static bool waitForUIContinue = false;

    public static Rect playArea;
    public static Rect camArea;

    public static Action OnGameSessionStart;
    public static Action OnLevelStart;
    public static Action OnLevelEndComplete;
    public static Action OnLevelEndLost;
    public static Action OnUpgradeMenuStart;
    public static Action OnUpgradeMenuTransitionOut;
    public static Action OnUpgradeMenuEnd;
    public static Action OnGameOverMenuStart;
    public static Action OnGameOverMenuTransitionOut;
    public static Action<int> OnMoneyModified;
    public static Action<bool> OnLifeSystemIsActiveModified;
    public static Action<int> OnLivesCurrentModified;
    public static Action<int> OnLifeContainersModified;

    private void Awake()
    {

        QualitySettings.vSyncCount = 0;
        //Application.targetFrameRate = 30;

        CalculatePlayArea();

    }

    private void Start()
    {
        AudioManager.Instance.Play("Chain1");

        GameSessionInit();

        OnLevelStart?.Invoke();
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
        SetLifeContainers(3);
        SetLivesCurrent(3);
    }

    private IEnumerator TryLevelCompleteSequence()
    {
        float transitionDuration = TransitionUI.TRANSITION_DURATION;

        //Debug.Log($"Trying to end the level. Time: {Time.time}");
        levelFinishFlag = true;

        //Wait for a short while and check if player dies
        yield return new WaitForSeconds(1f);

        //If so, abort level complete
        if (levelLostFlag)
        {
            levelFinishFlag = false;
            yield return null;
        }

        //Otherwise start transitioning to upgrade menu
        OnLevelEndComplete?.Invoke();
        yield return new WaitForSecondsRealtime(transitionDuration / 2);

        //Turn upgrade menu on and end the level mid-transition
        SetPauseState(true);
        OnUpgradeMenuStart?.Invoke();

        //Wait the second half of the transition
        yield return new WaitForSecondsRealtime(transitionDuration / 2);

        //Wait for upgrade menu continue;
        waitForUIContinue = true;
        while (waitForUIContinue) yield return null;

        //Start transitioning to new level
        OnUpgradeMenuTransitionOut?.Invoke();
        scoreCurrentLevel = 0;
        yield return new WaitForSecondsRealtime(transitionDuration / 2);

        //Hide upgrade menu, load level
        SetPauseState(false);
        OnUpgradeMenuEnd?.Invoke();

        //Wait the second half of the transition and start level
        yield return new WaitForSecondsRealtime(transitionDuration / 2);
        OnLevelStart?.Invoke();

        levelFinishFlag = false;

        yield return null;
    }

    private IEnumerator GameOverSequence()
    {
        float transitionDuration = TransitionUI.TRANSITION_DURATION;

        //Wait for a short while to show player death
        yield return new WaitForSeconds(1f);

        //Transition to game over menu
        OnLevelEndLost?.Invoke();
        yield return new WaitForSecondsRealtime(transitionDuration / 2);

        //Turn game over menu on
        OnGameOverMenuStart?.Invoke();
        SetPauseState(true);

        //Wait the second half of the transition
        yield return new WaitForSecondsRealtime(transitionDuration / 2);

        //Wait for game over menu continue;
        waitForUIContinue = true;
        while (waitForUIContinue) yield return null;

        //Start transitioning to new game
        OnGameOverMenuTransitionOut?.Invoke();
        yield return new WaitForSecondsRealtime(transitionDuration / 2);

        //Hide game over menu, load new game
        SetPauseState(false);
        GameSessionInit();

        //Wait the second half of the transition and start level
        yield return new WaitForSecondsRealtime(transitionDuration / 2);
        OnLevelStart?.Invoke();

        yield return null;
    }

    private void LevelLost()
    {
        levelLostFlag = true;
        StartCoroutine(GameOverSequence());
    }

    public void UIContinue()
    {
        waitForUIContinue = false;
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

    public void SetMoney(int newAmount, bool grantPoints = true)
    {
        int diff = newAmount - moneyTotal;
        moneyTotal = newAmount;
        if (diff > 0 && grantPoints)
        {
            scoreTotal += diff;
            scoreCurrentLevel += diff;

            if (scoreCurrentLevel >= LEVEL_COMPLETE_THRESHOLD && diff > 0 && pauseState == false && !levelFinishFlag)
                StartCoroutine(TryLevelCompleteSequence());

        }
        else moneyTotal = Mathf.Clamp(moneyTotal, 0, int.MaxValue);

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
            else if (livesCurrent > lifeContainers) livesCurrent = lifeContainers;

            OnLivesCurrentModified?.Invoke(livesCurrent);
        }
    }

    public void SetLifeContainers(int newCount)
    {
        if (lifeSystemIsActive)
        {
            lifeContainers = newCount;
            lifeContainers = Mathf.Clamp(lifeContainers, 1, LIFE_CONTAINER_MAXIMUM);

            if (lifeContainers < livesCurrent) SetLivesCurrent(lifeContainers);
            OnLifeContainersModified?.Invoke(lifeContainers);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(playArea.center, playArea.size);
    }


}
