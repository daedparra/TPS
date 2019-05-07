using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class NetDataSyncer : MonoBehaviourPun, IPunObservable
{

	[SerializeField, Range(0.01f, 1f)]
    private float _SmoothFactor = 1f;

	private Vector3 _NetPos;
	private Rigidbody _RB;

	private void Awake()
	{
		_RB = GetComponent<Rigidbody>();
	}

    [PunRPC]
	private void RPC_SetTeam(int team)
	{
		GetComponent<AUnit>().SetTeam(team);
	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if(stream.IsWriting) //Sending info
		{
			stream.SendNext(transform.position);
			stream.SendNext(transform.rotation);
			stream.SendNext(_RB.velocity);
		}
		else //Receiving info
		{
			_NetPos = (Vector3)stream.ReceiveNext();
			transform.rotation =  (Quaternion)stream.ReceiveNext();
			_RB.velocity = (Vector3)stream.ReceiveNext();
		}
	}

	private void Update()
	{
		if(photonView.IsMine)
		{
			return;
		}

		transform.position = Vector3.Lerp(transform.position, _NetPos, _SmoothFactor);
	}

}
