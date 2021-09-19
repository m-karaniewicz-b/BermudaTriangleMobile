using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeCenter : Holdable
{
    public static EyeCenter instance;

    public const float CHARGE_RADIUS_MAX_DEFAULT = 1.2f;
    public const float CHARGE_RADIUS_MIN = 0.75f;
    public const string HITBOX_TAG = "Hitbox";

    private bool isCharging;
    private float chargeSpeed;
    private float chargeRadiusMaxCurrent;
    private float chargeRadiusCurrent;

    [Header("Settings")]    
    [SerializeField] private Color explosionRadiusVFXColor = Color.white;
    [SerializeField] private float explosionRadiusVFXDuration = 1f;
    [SerializeField] private LayerMask enemyTargetLayer;

    [Header("References")]
    [SerializeField] private RadiusDisplay ChargeRadiusMaxIndicator;
    [SerializeField] private SpriteRenderer ChargeRadiusCurrentIndicator;
    [SerializeField] private CircleCollider2D attackCollider;

    protected override void Awake()
    {
        instance = this;
        base.Awake();

        ResetCharging();
        chargeRadiusMaxCurrent = CHARGE_RADIUS_MAX_DEFAULT;

    }

    private void Start()
    {
        SetChargeRadiusMax(chargeRadiusMaxCurrent);

    }

    private void Update()
    {
        if (isCharging)
        {
            if (chargeRadiusCurrent < chargeRadiusMaxCurrent)
                SetChargeRadiusCurrent(chargeRadiusCurrent + Time.deltaTime * chargeSpeed);
        }
    }

    public override void OnBeginClick()
    {
        base.OnBeginClick();

        if (!isCharging)
        {
            StartCharging();
        }
    }

    public override void OnEndClick()
    {
        base.OnEndClick();
        if (isCharging)
        {
            TryAttack(chargeRadiusCurrent);

            ResetCharging();
        }
    }

    public void SetChargeRadiusMax(float newRadius)
    {
        chargeRadiusMaxCurrent = newRadius;
        ChargeRadiusMaxIndicator.SetRadius(newRadius);
    }

    public void SetChargeSpeed(float speed)
    {
        chargeSpeed = speed;
    }

    private void ResetCharging()
    {
        isCharging = false;
        chargeRadiusCurrent = CHARGE_RADIUS_MIN;
        ChargeRadiusCurrentIndicator.enabled = false;
    }

    private void StartCharging()
    {
        isCharging = true;
        ChargeRadiusCurrentIndicator.enabled = true;
    }

    private void TryAttack(float radius)
    {
        List<Collider2D> targets = new List<Collider2D>(
            Physics2D.OverlapCircleAll(transform.position, radius, enemyTargetLayer));

        if (targets.Count > 0)
        {
            AudioManager.Instance.Play("Hit");
            VFXManager.SpawnParticleExplosionOneShot(VFXManager.Instance.hitVFX,
                transform.position, radius, radius, radius, explosionRadiusVFXColor, Color.white);
            VFXManager.Instance.SpawnSpriteOneShot(VFXManager.Instance.spriteRadialGradientReverse,
                transform.position, radius * 2, explosionRadiusVFXColor, explosionRadiusVFXDuration);

            for (int i = 0; i < targets.Count; i++)
            {
                MovingTarget t = targets[i].GetComponentInParent<MovingTarget>();
                if (t != null) t.Kill();
            }
        }
        else
        {
            AudioManager.Instance.Play("Miss");
            VFXManager.SpawnParticleExplosionOneShot(VFXManager.Instance.missVFX, transform.position, radius);
        }
    }

    private void SetChargeRadiusCurrent(float newRadius)
    {
        chargeRadiusCurrent = newRadius;
        ChargeRadiusCurrentIndicator.transform.localScale =
            new Vector3(chargeRadiusCurrent * 2, chargeRadiusCurrent * 2, 0);

    }

}
