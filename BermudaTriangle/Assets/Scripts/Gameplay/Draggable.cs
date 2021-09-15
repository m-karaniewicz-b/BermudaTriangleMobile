using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draggable : Holdable
{
    private void Update()
    {
        if (heldObject == this)
        {
            Vector2 newPos = InputManager.GetCursorPosition();

            Rect dragArea = GameManager.camArea;

            newPos = new Vector3(
                Mathf.Clamp(newPos.x, dragArea.xMin, dragArea.xMax),
                Mathf.Clamp(newPos.y, dragArea.yMin, dragArea.yMax),
                heldObject.transform.position.z);

            transform.position = newPos;
        }
    }

    public override void OnBeginClick()
    {
        base.OnBeginClick();

        AudioManager.instance.Play("PickUp");
        VFXManager.SpawnParticleOneshot(VFXManager.instance.clickEmptyVFX, transform.position);
    }

    public override void OnEndClick()
    {
        base.OnEndClick();

        AudioManager.instance.Play("PutDown");
        VFXManager.SpawnParticleOneshot(VFXManager.instance.dropVFX, transform.position);
    }

}
