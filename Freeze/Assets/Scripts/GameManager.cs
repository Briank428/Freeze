using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region public vars
    public float timeLeft = 320f;
    public GameObject itPrefab;
    public static GameManager Instance;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        StartCoroutine("StartGame");
    }

    public IEnumerator StartGame()
    {
        GameObject temp = PhotonNetwork.Instantiate("Player",Vector3.up,Quaternion.identity);
        if (temp.GetPhotonView().IsMine) temp.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = Color.blue;
        GameObject tempAI = PhotonNetwork.Instantiate("AI",Vector3.up,Quaternion.identity);
        yield return null;
    }
    // Update is called once per frame
    void Update()
    {
        timeLeft -= Time.deltaTime;
        if (timeLeft < 0 || AllRunnersFrozen())
        {
            EndGame();
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
        Debug.Log("end");
        //GameOver
        //Goto Lobby
        //PhotonNetwork.LoadLevel("Lobby");
    }

}
