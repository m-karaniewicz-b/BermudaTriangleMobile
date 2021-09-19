using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class UpgradeManager : Singleton<UpgradeManager>
{
    [Header("Current Upgrade Stats")]
    private int lifeContainers;
    private int controlPoints;

    private int chargeMaxRadius;
    private int chargeSpeed;

    private int multikillBonusMoney;
    private int pointBonus;

    private int enemySpeedMod;
    private int enemyHealthMod;

    private void Awake()
    {
        GameManager.Instance.OnGameSessionStart += Init;
    }

    private void Init()
    {
        lifeContainers = 3;
        controlPoints = 3;

        chargeMaxRadius = 0;
        chargeSpeed = 0;

        multikillBonusMoney = 0;
        pointBonus = 0;

        enemySpeedMod = 0;
        enemyHealthMod = 0;
        UpdateUpgrades();
    }

    public void ApplyItem(ItemData item, bool unapply = false)
    {
        int mult = unapply ? -1 : 1;

        if (!unapply) ApplyOneTimeEffects(item);

        lifeContainers += item.lifeContainers * mult;
        controlPoints += item.controlPoints * mult;

        chargeMaxRadius += item.chargeMaxRadius * mult;
        chargeSpeed += item.chargeSpeed * mult;

        multikillBonusMoney += item.multikillBonusMoney * mult;
        pointBonus += item.pointBonus * mult;

        enemySpeedMod += item.enemySpeed * mult;
        enemyHealthMod += item.enemyHealthMod * mult;

        UpdateUpgrades();
    }

    private void ApplyOneTimeEffects(ItemData item)
    {
        GameManager.Instance.SetLivesCurrent(GameManager.Instance.livesCurrent + item.lifeRestore);
        GameManager.Instance.SetMoney(GameManager.Instance.moneyTotal + item.money);
    }

    private void UpdateUpgrades()
    {
        GameManager.Instance.SetLifeContainers(lifeContainers);

        if (Centroid.Instance.GetControlPointCount() != controlPoints) 
            Centroid.Instance.InitControlPoints(controlPoints);


        float newRadius = EyeCenter.CHARGE_RADIUS_MAX_DEFAULT + Remap(chargeMaxRadius, 20, 0, 5, 0.8f);
        EyeCenter.instance.SetChargeRadiusMax(newRadius);

        float newSpeed = 1 + Remap(chargeSpeed, 20, 0, 10, 1f);
        EyeCenter.instance.SetChargeSpeed(newSpeed);
    }

    private static float Remap(int inputValue, int inputForMax, float outputMin, float outputMax, float curve = 1)
    {
        return Lerp(outputMin, outputMax, Mathf.Pow((float)inputValue / inputForMax, curve));
    }

    private static float Lerp(float firstFloat, float secondFloat, float by)
    {
        return firstFloat * (1 - by) + secondFloat * by;
    }

}
