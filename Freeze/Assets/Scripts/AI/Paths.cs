using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paths
{
    private float distance;
    private GameObject player;
    public Paths(float d, GameObject go)
    {
        distance = d;
        player = go;
    }
    public float GetDistance() { return distance; }
    public GameObject GetPlayer() { return player; }
}
