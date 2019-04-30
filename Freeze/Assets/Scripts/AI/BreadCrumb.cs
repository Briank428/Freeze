using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreadCrumb
{
    private Vector2 pos; public Vector2 GetPos() { return pos; }

    public BreadCrumb(Vector2 position)
    {
        pos = position;
    }
}
