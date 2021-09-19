using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Holdable : Clickable
{
    public static Clickable heldObject;

    protected virtual void Awake()
    {
        heldObject = null;
    }

    public override void OnBeginClick()
    {
        base.OnBeginClick();

        heldObject = this;
    }

    public override void OnEndClick()
    {
        base.OnEndClick();

        heldObject = null;
    }

}
