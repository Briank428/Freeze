using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.AI;
using System.Linq;

public class AIMove : MonoBehaviourPun
{
    List<GameObject> players;
    // Start is called before the first frame update
    void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Runner").ToList();
        
    }

    // Update is called once per frame
    void Update()
    {

    }
}
