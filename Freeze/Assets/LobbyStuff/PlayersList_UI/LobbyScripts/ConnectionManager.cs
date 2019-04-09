using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

/// <summary>
/// Simple Connection Manager.
/// Deals with toggle UI menu and player list based on photon status.
/// </summary>

public class ConnectionManager : MonoBehaviourPunCallbacks
{

	public GameObject MenuUI;
	public GameObject RoomUI;
	bool ConnectionInProgress;
	ClientState _clientStateCache;

	public string PlayerName
	{
		get
		{
			return PhotonNetwork.NickName;
		}
		set
		{
			PhotonNetwork.NickName = value;
		}
	}

	void Start()
	{
        MenuUI.SetActive(true);
		RoomUI.SetActive(false);

	}

	void Update()
	{
		if (_clientStateCache != PhotonNetwork.NetworkClientState)
		{
			_clientStateCache = PhotonNetwork.NetworkClientState;
		}
	}

	public override void OnJoinedLobby()
	{
    Debug.Log("OnJoinedLobby");

    if (ConnectionInProgress) { 

        Debug.Log("Join Random Room");
        RoomOptions roomOptions = new RoomOptions() { IsVisible = true, MaxPlayers = 5 };
        PhotonNetwork.JoinOrCreateRoom("RoomOne", roomOptions, TypedLobby.Default);
        return;
		}

		MenuUI.SetActive(true);
	}
		
	// the following methods are implemented to give you some context. re-implement them as needed.
		
	public virtual void OnFailedToConnectToPhoton(DisconnectCause cause)
	{
		Debug.LogError("Cause: " + cause);
	}


	public void Connect () {
    Debug.Log("Connnect");
		// Unity UI hack to catch TextField submition. 
		// the player name TextField OnEndEdit calls Connect(), but pressing Esc also means ending the edit, so we catch the esc key and don't proceed.
		if( Input.GetKeyDown( KeyCode.Escape ) ) 
		{
			return;
		}

		ConnectionInProgress = true;
		if (PhotonNetwork.InLobby)
		{
			PhotonNetwork.JoinRandomRoom();
		}else{
        Debug.Log("Connect to master call");    
			PhotonNetwork.ConnectUsingSettings();
		}
		MenuUI.SetActive(false);
	}
		
    public override void OnConnectedToMaster()
    {
    Debug.Log("On ConnectedTo MAstert");
    PhotonNetwork.JoinLobby();
    }

	public void LeaveRoom(){
		PhotonNetwork.LeaveRoom();
	}

	public override void OnLeftRoom()
	{
		MenuUI.SetActive(false);
		RoomUI.SetActive(false);
    }

    public override void OnJoinedRoom()
	{
        Debug.Log("Joined Room");
		ConnectionInProgress = false;
		MenuUI.SetActive(false);
		RoomUI.SetActive(true);
	}

}


