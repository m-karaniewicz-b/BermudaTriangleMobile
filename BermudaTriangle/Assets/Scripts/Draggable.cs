using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draggable : Clickable
{
    public static Clickable heldObject;

    private void Update()
    {
        if (heldObject == this)
        {
            Vector2 newPos = TouchManager.GetTouchPosition();

            Rect dragArea = GameManager.instance.camArea;

            newPos = new Vector3(
                Mathf.Clamp(newPos.x, dragArea.xMin, dragArea.xMax),
                Mathf.Clamp(newPos.y, dragArea.yMin, dragArea.yMax),
                heldObject.transform.position.z);

            transform.position = newPos;



            /*
            Vector3 minScreenBounds = Camera.main.ScreenToWorldPoint(Vector2.zero);
            Vector3 maxScreenBounds = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));

            transform.position = new Vector3(
                Mathf.Clamp(transform.position.x, minScreenBounds.x, maxScreenBounds.x),
                Mathf.Clamp(transform.position.y, minScreenBounds.y, maxScreenBounds.y),
                heldObject.transform.position.z);
            */
        }
    }

    public override void OnBeginClick()
    {
        base.OnBeginClick();

        AudioManager.instance.Play("PickUp");
        VFXManager.SpawnParticleOneshot(VFXManager.instance.clickEmptyVFX, transform.position);
        heldObject = this;
    }

    public override void OnEndClick()
    {
        base.OnEndClick();

        AudioManager.instance.Play("PutDown");
        VFXManager.SpawnParticleOneshot(VFXManager.instance.dropVFX, transform.position);
        heldObject = null;
    }

}
