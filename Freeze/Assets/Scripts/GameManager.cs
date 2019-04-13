using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class GameManager : MonoBehaviour
{
    public float timeLeft = 320f;
    public static GameManager Instance;
    public List<Vector2Int> spawnCells;
    private Vector2Int playerLoc;
    public GameObject player;
    private bool started;

    // Start is called before the first frame update
    
    void Start()    
    {
        Instance = this;
        started = false;
        StartCoroutine("StartGame");
    }

    public IEnumerator StartGame()
    {
        yield return new WaitForSeconds(5);
        Random rnd = new Random();
        playerLoc = spawnCells[rnd.Next(spawnCells.Count)];
        Vector2Int aiLoc = spawnCells[rnd.Next(spawnCells.Count)];

        player = PhotonNetwork.Instantiate("Player",new Vector3(playerLoc.x,0,playerLoc.y),Quaternion.identity) as GameObject;
        if (player.GetPhotonView().IsMine || !PhotonNetwork.IsConnected) player.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = Color.blue;
        if (PhotonNetwork.IsMasterClient) { Debug.Log("Master Client");  GameObject tempAI = PhotonNetwork.Instantiate("AI",new Vector3(aiLoc.x,0,aiLoc.y) , Quaternion.identity); }
        Debug.Log("Player and AI instantiated");
        started = true;
        yield return null;
    }
    // Update is called once per frame
    void Update()
    {
        timeLeft -= Time.deltaTime;
        if (timeLeft < 0 || AllRunnersFrozen())
        {
            //EndGame();
        }
    }
     bool AllRunnersFrozen()
    {
        GameObject[] runners = GameObject.FindGameObjectsWithTag("Runner");
        foreach (GameObject runner in runners)
        {
            if (runner.GetComponent<OnTag>().IsFrozen) return false;
        }
        return true;
    }

    void EndGame()
    {
        //Debug.Log("end");
        //GameOver
        //Goto Lobby
        //PhotonNetwork.LoadLevel("Lobby");
    }

}
