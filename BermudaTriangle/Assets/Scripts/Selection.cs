using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selection : MonoBehaviour
{
    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 touchPos = Camera.main.ScreenToWorldPoint(touch.position);

            /*
            if (heldObject != null)
            {
                UpdateDraggedObjectPosition();

                if (touch.phase == TouchPhase.Ended)
                {
                    EndTouch();
                }
            }
            else
            {
                if (touch.phase == TouchPhase.Began)
                {
                    BeginTouch();
                }
            }
            */
        }
    }


}
