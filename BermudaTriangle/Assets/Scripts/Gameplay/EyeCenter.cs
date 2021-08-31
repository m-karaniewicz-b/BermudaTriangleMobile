using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeCenter : Clickable
{
    private float startingHitRadius = 1;
    internal float hitRadius;

    private List<MovingTarget> targetList = new List<MovingTarget>();

    public LayerMask eyeMask;

    [SerializeField] private CircleCollider2D coll;
    [SerializeField] private ParticleSystem circlingParticles;

    private void Awake()
    {
        SetHitRadius(startingHitRadius);
    }

    public override void OnBeginClick()
    {
        base.OnBeginClick();

        targetList.RemoveAll(item => item == null);
        if (targetList.Count > 0)
        {
            AudioManager.instance.Play("Hit");
            VFXManager.SpawnParticleExplosionOneShot(VFXManager.instance.hitVFX, transform.position, hitRadius);

            for (int i = 0; i < targetList.Count; i++)
            {
                targetList[i].Kill();
            }
        }
        else
        {
            AudioManager.instance.Play("Miss");
            VFXManager.SpawnParticleExplosionOneShot(VFXManager.instance.missVFX, transform.position, hitRadius);

        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        targetList.Add(collision.gameObject.GetComponentInParent<MovingTarget>());

        //Debug.Log($"Target acquired: {collision.gameObject.name} Target count: {targetList.Count}");
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        MovingTarget tar = collision.gameObject.GetComponentInParent<MovingTarget>();
        targetList.RemoveAll(item => item == tar);

        //Debug.Log($"Target lost: {collision.gameObject.name} Target count: {targetList.Count}");
    }

    public void SetHitRadius(float radius)
    {
        hitRadius = radius;
        coll.radius = hitRadius;

        var shape = circlingParticles.shape;
        shape.radius = hitRadius;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.1f);

        Gizmos.DrawSphere(transform.position, hitRadius);
    }
}
