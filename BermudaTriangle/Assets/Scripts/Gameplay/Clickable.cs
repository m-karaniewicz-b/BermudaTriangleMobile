using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clickable : MonoBehaviour
{
    private static List<Clickable> clickables;

    public int priority = 0;
    public float maxClickRadius = 5;

    public virtual void OnBeginClick() { }
    public virtual void OnEndClick() { }

    private static void UpdateClickablesList()
    {
        clickables = new List<Clickable>(FindObjectsOfType<Clickable>());
    }

    public static Clickable GetClickableOnPosition(Vector2 pos)
    {
        UpdateClickablesList();

        Clickable retClickable = null;
        float smallestDistance = Mathf.Infinity;
        float highestPriority = 0;

        for (int i = 0; i < clickables.Count; i++)
        {
            float dist = Vector2.Distance(clickables[i].transform.position, pos);

            if (dist < clickables[i].maxClickRadius)
            {
                if (clickables[i].priority > highestPriority)
                {
                    retClickable = clickables[i];
                    highestPriority = clickables[i].priority;
                    smallestDistance = dist;
                }
                else
                {
                    if (clickables[i].priority == highestPriority)
                    {
                        if (dist < smallestDistance)
                        {
                            retClickable = clickables[i];
                            smallestDistance = dist;
                        }
                    }
                }
            }
        }

        return retClickable;
    }

    /*
    private void OnDrawGizmos()
    {
        UnityEditor.Handles.color = Color.gray;
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.back, maxClickRadius);
    }
    */

    /*
    public bool IsInDistance(Vector2 pos)
    {
        return Vector2.Distance(transform.position, pos) < maxClickRadius;
    }
    */
    /*
    public bool IsInClickDistance()
    {
        return Vector2.Distance(transform.position, DragAndDrop.GetTouchPosition()) < maxClickRadius;
    }
    */

}
