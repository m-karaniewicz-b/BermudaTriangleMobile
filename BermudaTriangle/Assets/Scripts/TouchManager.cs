using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchManager : MonoBehaviour
{
    private void Update()
    {
        if(GameManager.instance.pauseState != true)
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                {
                    BeginSingleTouch();
                }

                if (touch.phase == TouchPhase.Ended)
                {
                    EndTouch();
                }
            }
        }
    }

    private void BeginSingleTouch()
    {
        Clickable cl = Clickable.GetClickableOnPosition(GetTouchPosition());

        if (cl == null)
        {
            AudioManager.instance.Play("ClickEmpty");
            VFXManager.SpawnParticleOneshot(VFXManager.instance.clickEmptyVFX, GetTouchPosition());
            return;
        }

        cl.OnBeginClick();
    }

    private static void EndTouch()
    {
        if(Draggable.heldObject!=null) Draggable.heldObject.OnEndClick();
    }

    public static Vector2 GetTouchPosition()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 touchPos = Camera.main.ScreenToWorldPoint(touch.position);

            return touchPos;
        }
        else
        {
            Debug.LogError("Check for touchCount before calling this!");
            return Vector2.zero;
        }
    }

    public static void FlushTouchInput()
    {
        EndTouch();
    }

    /*
    private void UpdateDraggedObjectPosition()
    {
        heldObject.transform.position = GetTouchPosition();

        Vector3 minScreenBounds = Camera.main.ScreenToWorldPoint(Vector2.zero);
        Vector3 maxScreenBounds = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));

        heldObject.transform.position = new Vector3(
            Mathf.Clamp(heldObject.transform.position.x, minScreenBounds.x, maxScreenBounds.x),
            Mathf.Clamp(heldObject.transform.position.y, minScreenBounds.y, maxScreenBounds.y),
            heldObject.transform.position.z);
    }
    */
}
