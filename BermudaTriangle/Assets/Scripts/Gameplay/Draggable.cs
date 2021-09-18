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

        AudioManager.Instance.Play("PickUp");
        VFXManager.SpawnParticleOneshot(VFXManager.Instance.clickEmptyVFX, transform.position);
    }

    public override void OnEndClick()
    {
        base.OnEndClick();

        AudioManager.Instance.Play("PutDown");
        VFXManager.SpawnParticleOneshot(VFXManager.Instance.dropVFX, transform.position);
    }

}
