using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager instance;

    [Header("Current Upgrade Stats")]
    private int lifeContainers = 3;
    private int controlPoints = 3;

    private int chargeMaxRadius = 0;
    private int chargeSpeed = 0;

    private int multikillBonusMoney = 0;
    private int pointBonus = 0;

    private int enemySpeedMod = 0;
    private int enemyHealthMod = 0;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
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
        GameManager.instance.SetLivesCurrent(GameManager.livesCurrent + item.lifeRestore);
        GameManager.instance.SetMoney(GameManager.moneyTotal + item.money);
    }

    private void UpdateUpgrades()
    {
        GameManager.instance.SetLifeContainers(lifeContainers);

        if (Centroid.instance.GetControlPointCount() != controlPoints) 
            Centroid.instance.InitControlPoints(controlPoints);


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


    //[Button]
    //private void CalculationDebug(int upgrade, float lowerEnd, float higherEnd, float curve)
    //{
    //    //float valueSoftcap = (baseN * fraction * upgrade) / (1 + fraction * upgrade);

    //    float val = Mathf.Pow(upgrade * lowerEnd / higherEnd, curve);
    //    float radius = Lerp(0, 5, val);

    //    Debug.Log($"s = {radius}");
    //}

}
