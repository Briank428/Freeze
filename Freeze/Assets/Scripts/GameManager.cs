using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class GameManager : MonoBehaviour
{
    public float timeLeft = 60f * 2f;
    public static GameManager Instance;
    public List<Vector2Int> spawnCells;
    private Vector2Int playerLoc;
    public GameObject player;
    bool begin;

    // Start is called before the first frame update
    void Start()    
    {
        Instance = this;
        begin = false;
        PhotonNetwork.AutomaticallySyncScene = true;
        StartCoroutine("StartGame");
    }

    public IEnumerator StartGame()
    {
        yield return new WaitForSeconds(3);
        Random rnd = new Random();
        playerLoc = spawnCells[rnd.Next(spawnCells.Count)];
        Vector2Int aiLoc = spawnCells[rnd.Next(spawnCells.Count)];

        player = PhotonNetwork.Instantiate("Player",new Vector2(playerLoc.x + 0.5f,playerLoc.y + 0.5f),Quaternion.identity) as GameObject;
        if (player.GetPhotonView().IsMine) player.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = Color.blue;
        if (PhotonNetwork.IsMasterClient) { GameObject tempAI = PhotonNetwork.Instantiate("AI",new Vector2(aiLoc.x + 0.5f,aiLoc.y + 0.5f) , Quaternion.identity); }
        Debug.Log("Player and AI instantiated");
        begin = true;
        yield return null;
    }
    // Update is called once per frame
    void Update()
    {
        if (begin && PhotonNetwork.IsMasterClient)
        {
            timeLeft -= Time.deltaTime;
            if (timeLeft < 0 || AllRunnersFrozen())
            {
                EndGame();
            }
        }
    }
     bool AllRunnersFrozen()
    {
        GameObject[] runners = GameObject.FindGameObjectsWithTag("Runner");
        foreach (GameObject runner in runners)
        {
            if (!runner.GetComponent<OnTag>().IsFrozen) return false;
        }
        Debug.Log("All Frozen");
        return true;
    }

    void EndGame()
    {
        Debug.Log("end");
        PhotonNetwork.LoadLevel("Lobby");
    }

}
