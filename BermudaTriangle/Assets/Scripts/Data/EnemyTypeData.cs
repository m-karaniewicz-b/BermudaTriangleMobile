using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New enemy type", menuName = "Data/EnemyTypeData")]
public class EnemyTypeData : ScriptableObject
{
    [Header("Base Stats")]
    public float movementSpeed = 3f;
    public bool indicatorOn = true;
    public EntityTrajectory trajectory = EntityTrajectory.random;

    [Header("Kill")]
    public bool canBeKilled = true;
    public int lifeModOnKill = 0;
    public int pointModOnKill = 1;

    [Header("Escape")]
    public int lifeModOnEscape = 0;
    public int pointModOnEscape = 0;

    [Header("Eye Collision")]
    public bool collideWithEye = false;
    public bool destroyOnEyeCollision = false;
    public int lifeModOnEyeCollision = 0;
    public int pointModOnEyeCollision = 0;

    [Header("Visual")]
    public Sprite sprite;
    public Color color1 = Color.white;
    public Color color2 = Color.white;//unused

    [Header("Optional")]
    public GameObject prefab;//unused
}
public enum EntityTrajectory
{
    random,
    throughTheEye,
    //throughClosestControlPoint,
    //throughFurthestControlPoint,
}

[System.Serializable]
public class SpawnGroup
{
    [Header("Requirements")]
    public int weight = 100;
    public int minLevelScore = 0;
    public int maxLevelScore = 100;

    [Header("Spawning")]
    public EnemyTypeData enemyType;
    public int instanceCount = 1;

    [Header("Timing")]
    public float globalSpawnCooldown = 3;
    //public float spawnDelay = 0;
}