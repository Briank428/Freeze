using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
/// <summary>
/// Player list UI manager.
/// This is a good starting point to handle efficient UI management for listing Photon Players.
/// It only updates when required
/// For long lists, be aware that items creation isn't optimal, as for every creation of GameObject, consider using pooling systems or dedicated framework for efficient long lists like https://www.assetstore.unity3d.com/en/#!/content/36378
/// 
/// Note that the typical setup should be that this controller sits on a GameObject that is never disabled, and the UI element can be turned off at will. This is important for efficient UI rendering to keep the minimum amount of UI on screen
/// </summary>
public class PlayerListUIController: MonoBehaviourPunCallbacks
{

	public GameObject UIList;

	public GameObject PlayerItemPrefab;

	Dictionary<int,PlayerItem> _items = new Dictionary<int, PlayerItem>();


	/// <summary>
	/// Updates the UI listing, it creates the necessary items not yet listed, update existing items and remove unused entries
	/// </summary>
	public void UpdateUI()
	{
		List<int> processedIDS = new List<int>();

		// update existing items and add new items
		foreach(Photon.Realtime.Player _player in PhotonNetwork.PlayerList)
		{
			if (_items.ContainsKey(_player.ActorNumber)) // update
			{
				_items[_player.ActorNumber].RefreshData(_player);
				processedIDS.Add(_player.ActorNumber);

			}else{ // create new
				GameObject _item =  (GameObject)Instantiate(PlayerItemPrefab);
				_item.transform.SetParent(UIList.transform);

				PlayerItem _playerItem = _item.GetComponent<PlayerItem>();
				_items.Add(_player.ActorNumber,_playerItem);
				_items[_player.ActorNumber].RefreshData(_player);

				_playerItem.AnimateRevealItem();

				processedIDS.Add(_player.ActorNumber);
			}
		}

		// now deal with items that needs to be deleted.
		// work in reverse so that we can delete entries without worries.
		foreach(var _item in _items.Reverse())
		{
			if (!processedIDS.Contains(_item.Key))
			{
				_items[_item.Key].AnimateRemoveItem();
				_items.Remove(_item.Key);
			}
		}

	}

	/// <summary>
	/// Cleans up list to prevent memory leak.
	/// </summary>
	public void CleanUpList()
	{
		_items = new Dictionary<int, PlayerItem>();
		foreach(Transform child in UIList.transform) 
		{
			Destroy(child.gameObject);
		}
	}

	#region PHOTON CALLBACKS
	public override void OnJoinedRoom()
	{
		CleanUpList();
		UpdateUI();
	}
		
	public override void OnLeftRoom()
	{
		CleanUpList();
	}

	public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
	{
		UpdateUI();
	}
		
	public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
	{
		UpdateUI();
	}

	public override void OnMasterClientSwitched (Photon.Realtime.Player newMasterClient)
	{
		UpdateUI();
	}

	#endregion
}