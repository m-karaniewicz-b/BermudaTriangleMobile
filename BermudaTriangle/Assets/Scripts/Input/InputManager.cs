using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    private void Update()
    {
        if(GameManager.pauseState != true)
        {
            if(Application.isEditor)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    BeginClick();
                }
                else if(Input.GetMouseButtonUp(0))
                {
                    EndClick();
                }
            }
            else
            {
                if (Input.touchCount > 0)
                {
                    Touch touch = Input.GetTouch(0);

                    if (touch.phase == TouchPhase.Began)
                    {
                        BeginClick();
                    }

                    if (touch.phase == TouchPhase.Ended)
                    {
                        EndClick();
                    }
                }
            }
        }
    }

    private void BeginClick()
    {
        Vector2 clickPosition = GetCursorPosition();

        Clickable cl = Clickable.GetClickableOnPosition(clickPosition);

        if (cl == null)
        {
            AudioManager.Instance.Play("ClickEmpty");
            VFXManager.SpawnParticleOneshot(VFXManager.Instance.clickEmptyVFX, clickPosition);
            return;
        }

        cl.OnBeginClick();
    }

    private static void EndClick()
    {
        if(Holdable.heldObject != null) Holdable.heldObject.OnEndClick();
    }

    public static Vector2 GetCursorPosition()
    {
        if(Application.isEditor)
        {
            return Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        else
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
    }

    public static void FlushInput()
    {
        EndClick();
    }
}
