using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreadCrumb
{
    private Vector2 pos; public Vector2 GetPos() { return pos; }
    public List<BreadCrumb> neighbors = new List<BreadCrumb>();

    public bool upNeigh;
    public bool downNeigh;
    public bool rightNeigh;
    public bool leftNeigh;

    public BreadCrumb(Vector2 position)
    {
        pos = position;
        upNeigh = false; downNeigh = false; rightNeigh = false; leftNeigh = false;
    }
}
