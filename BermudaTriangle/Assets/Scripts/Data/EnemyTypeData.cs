using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New enemy type", menuName = "Data/EnemyTypeData")]
public class EnemyTypeData : ScriptableObject
{
    [Header("Base Stats")]
    public float movementSpeed = 3f;
    public bool targetingEye = false;
    public bool indicatorOn = true;

    public int lifeModOnKill = 0;
    public int lifeModOnEscape = 0;
    public int lifeModOnEyeCollision = 0;
    public int pointModOnKill = 1;
    public int pointModOnEscape = 0;
    public int pointModOnEyeCollision = 0;

    [Header("Visual")]
    public Sprite sprite;
    public Color color1 = Color.white;
    public Color color2 = Color.white;

    [Header("Optional")]
    public GameObject prefab;

}
