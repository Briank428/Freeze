using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviourPunCallbacks
{
    float timeLeft;
    const float TIME_INIT = 10f;
    bool countDown;
    public Text text;
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        timeLeft = TIME_INIT;
        countDown = false;
        text.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.IsMasterClient && countDown)
        {
            timeLeft -= Time.deltaTime;
            text.text = "Time Left: " + ((int)timeLeft).ToString();
            if (timeLeft <= 0)
            {
                Debug.Log("New Scene");
                countDown = false;
                PhotonNetwork.CurrentRoom.IsOpen = false;
                PhotonNetwork.LoadLevel(4);
            }
        }
        else if(countDown) text.text = "Time Left: " + ((int)timeLeft).ToString();
        else { timeLeft = TIME_INIT;}
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        if (PhotonNetwork.PlayerList.Length >= 2) countDown = true;
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        if (PhotonNetwork.PlayerList.Length < 2) { countDown = false; text.text = "Waiting For Players ..."; }
    }

    public override void OnJoinedRoom()
    {
        text.text = "Waiting For Players ...";
    }
    public override void OnLeftRoom()
    {
        text.text = "";
    }
}
