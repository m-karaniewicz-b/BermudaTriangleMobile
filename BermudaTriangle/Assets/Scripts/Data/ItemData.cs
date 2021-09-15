using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemData
{
    [Header("Generation Stats")]
    internal bool isGenerated = false;
    internal float itemSeed;
    internal float itemEfficiency;
    internal float itemSize;

    [Header("Description")]
    public string name;
    public string description;
    public Sprite icon;

    [Header("Aquisition Cost")]
    public int costMoney;
    public int costLivesCurrent;
    public int costLifeContainers;

    [Header("Availability Requirements")]
    public int minTotalScore;

    [Header("One-time Effects")]
    public int money;
    public int lifeRestore;

    [Header("Continuous Effects Duration")]
    public bool permanent = true;
    public int lastForLevels;

    [Header("Continuous Effects")]
    public int lifeContainers;
    public int controlPoints;

    public int chargeMaxRadius;
    public int chargeSpeed;
    public int multikillBonusMoney;

    public int pointBonus;

    public int enemySpeed;
    public int enemyHealthMod;


    public ItemData()
    {
        //itemSeed = UnityEngine.Random.value;

        name = $"New Item";
        description = $"New item description.";//$"Seed: {itemSeed}";
        //costMoney = UnityEngine.Random.Range(0, 150);
    }

}
