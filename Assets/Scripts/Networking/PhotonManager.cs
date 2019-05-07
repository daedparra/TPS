using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PhotonManager : MonoBehaviourPunCallbacks
{

	[SerializeField]
	private string _UserName = "DUDE";

	[SerializeField]
	private int _TeamNumber = 0;


	private void Start()
	{
		PhotonNetwork.NickName = _UserName;
		PhotonNetwork.SendRate = 50;
		PhotonNetwork.SerializationRate = 25;

		Debug.LogFormat("Connected? {0}", PhotonNetwork.ConnectUsingSettings());
	}

	public override void OnConnectedToMaster()
	{
		print("OnConnectedToMaster");
		RoomOptions roomOptions = new RoomOptions()
		{
			MaxPlayers = 20,
			IsVisible = true,
			IsOpen = true
		};
		PhotonNetwork.JoinOrCreateRoom("PG14", roomOptions, TypedLobby.Default);
	}


	public override void OnJoinedRoom()
	{
		Debug.LogFormat("OnJoinedRoom: {0}", PhotonNetwork.CurrentRoom.Name);
		var clone = PhotonNetwork.Instantiate("Net_NightShade", transform.position, transform.rotation);
		PhotonView.Get(clone).RPC("RPC_SetTeam", RpcTarget.AllBufferedViaServer, _TeamNumber);
	}

}
