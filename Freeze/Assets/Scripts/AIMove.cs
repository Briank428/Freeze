using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.AI;

public class AIMove : MonoBehaviourPun
{
    private NavMeshAgent agent;
    private GameObject currentPlayer;
    public static List<GameObject> players;
    private List<Paths> distances;
    // Start is called before the first frame update
    void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();
        currentPlayer = players[0];
        agent.SetDestination(currentPlayer.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        Paths temp = new Paths(agent.remainingDistance, currentPlayer);
        foreach (GameObject player in players)
        {
            agent.SetDestination(player.transform.position);
            if (agent.remainingDistance < temp.GetDistance()) {
                currentPlayer = player;  temp = new Paths(agent.remainingDistance, player);  }
        }
    }
}
