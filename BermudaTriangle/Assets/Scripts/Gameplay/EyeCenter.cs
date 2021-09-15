using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeCenter : Holdable
{
    public static EyeCenter instance;

    [Header("Settings")]    
    [SerializeField]private Color explosionRadiusVFXColor = Color.white;
    [SerializeField]private float explosionRadiusVFXDuration = 1f;

    private float chargeSpeed = 1f; //1 unit of radius per second
    private float chargeRadiusMaxCurrent = CHARGE_RADIUS_MAX_DEFAULT;

    public const float CHARGE_RADIUS_MAX_DEFAULT = 1.2f;
    public const float CHARGE_RADIUS_MIN = 0.75f;

    private bool isCharging = false;
    private float chargeRadiusCurrent;

    public LayerMask enemyTargetLayer;
    public const string HITBOX_TAG = "Hitbox";

    [Header("References")]
    [SerializeField] private RadiusDisplay ChargeRadiusMaxIndicator;
    [SerializeField] private SpriteRenderer ChargeRadiusCurrentIndicator;
    [SerializeField] private CircleCollider2D attackCollider;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        ChargeRadiusCurrentIndicator.enabled = false;
    }

    private void Start()
    {
        SetChargeRadiusMax(chargeRadiusMaxCurrent);
        ResetCharging();
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
            AudioManager.instance.Play("Hit");
            VFXManager.SpawnParticleExplosionOneShot(VFXManager.instance.hitVFX,
                transform.position, radius, radius, radius, explosionRadiusVFXColor, Color.white);
            VFXManager.instance.SpawnSpriteOneShot(VFXManager.instance.spriteRadialGradientReverse,
                transform.position, radius * 2, explosionRadiusVFXColor, explosionRadiusVFXDuration);

            for (int i = 0; i < targets.Count; i++)
            {
                MovingTarget t = targets[i].GetComponentInParent<MovingTarget>();
                if (t != null) t.Kill();
            }
        }
        else
        {
            AudioManager.instance.Play("Miss");
            VFXManager.SpawnParticleExplosionOneShot(VFXManager.instance.missVFX, transform.position, radius);
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

    private void SetChargeRadiusCurrent(float newRadius)
    {
        chargeRadiusCurrent = newRadius;
        ChargeRadiusCurrentIndicator.transform.localScale =
            new Vector3(chargeRadiusCurrent * 2, chargeRadiusCurrent * 2, 0);

    }

}
