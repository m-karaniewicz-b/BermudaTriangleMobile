using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    public GameObject heldObject;
    public LayerMask dragMask;

    private EyeCenter eye;


    private void Awake()
    {
        eye = FindObjectOfType<EyeCenter>();

    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

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
        }
    }

    private void BeginTouch()
    {
        if (!eye.IsInClickDistance())
        {
            //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, dragMask);

            RaycastHit2D hit = Physics2D.Raycast(GetTouchPosition(), Camera.main.transform.forward, Mathf.Infinity, dragMask);

            if (hit.collider != null)
            {
                AudioManager.instance.Play("PickUp");
                VFXManager.SpawnParticleOneshot(VFXManager.instance.clickEmptyVFX, GetTouchPosition());
                //Debug.Log(hit.collider.gameObject.name);
                heldObject = hit.collider.gameObject;
            }
            else
            {
                AudioManager.instance.Play("ClickEmpty");
                VFXManager.SpawnParticleOneshot(VFXManager.instance.clickEmptyVFX, GetTouchPosition());
            }
        }
    }

    private void EndTouch()
    {
        AudioManager.instance.Play("PutDown");
        VFXManager.SpawnParticleOneshot(VFXManager.instance.dropVFX, GetTouchPosition());
        heldObject = null;

    }

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

    public static Vector2 GetTouchPosition()
    {
        if(Input.touchCount>0)
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
