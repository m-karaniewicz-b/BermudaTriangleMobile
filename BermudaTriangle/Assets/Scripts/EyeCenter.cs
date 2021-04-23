using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeCenter : Clickable
{
    private List<MovingTarget> targetList = new List<MovingTarget>();

    public LayerMask eyeMask;

    public override void OnBeginClick()
    {
        base.OnBeginClick();

        targetList.RemoveAll(item => item == null);
        if (targetList.Count > 0)
        {
            AudioManager.instance.Play("Hit");
            VFXManager.SpawnParticleOneshot(VFXManager.instance.hitVFX, transform.position);

            //List<MovingTarget> trgts = new List<MovingTarget>();
            for (int i = 0; i < targetList.Count; i++)
            {
                targetList[i].DestroySelf();
                GameManager.instance.AddPoints(1);
                //trgts.Add(targetList[i]);
                //Debug.Log($"Destroying: {targetList[i].gameObject}");
            }

            /*
            for (int i = 0; i < trgts.Count; i++)
            {
                trgts[i].Damage();
            }
            */
        }
        else
        {
            AudioManager.instance.Play("Miss");
            VFXManager.SpawnParticleOneshot(VFXManager.instance.missVFX, transform.position);

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

}
