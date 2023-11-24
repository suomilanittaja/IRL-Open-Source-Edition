using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PickUp : MonoBehaviourPunCallbacks
{
	[Header("GameObjects")]
	public GameObject pickGun;
	public GameObject Gun;
	public GameObject remoteGun;

	[Header("Scripts")]
	public PlayerController controller;
	public Raycast rayScript;

	[Header("Others")]
	[SerializeField] private string selectableTag = "Gun";
	private Transform _selection;
	public bool usingGun = false;



	private void Update()
   {
     if (photonView.IsMine)
     {
       pickGun = rayScript.rayHitted;
		   if (rayScript.rayHitted.CompareTag(selectableTag) && Input.GetKeyDown(KeyCode.F) && rayScript.hitDis <= 5)
		   {
		       print("key was pressed");
           PhotonNetwork.Destroy(pickGun);
           controller.hasGun = true;
					 usingGun = true;
		       photonView.RPC("pickUp", RpcTarget.All);
		   }
			 if (usingGun == false)
			 {
				 photonView.RPC("useHand2", RpcTarget.All);
			 }

			 if (usingGun == true)
			 {
				 photonView.RPC("useGun2", RpcTarget.All);
			 }
     }
   }

   [PunRPC]
   void pickUp()
   {
     Gun.gameObject.SetActive(true);
		 remoteGun.gameObject.SetActive(true);
   }

	 [PunRPC]
	 void useGun2()
	 {
		 Gun.gameObject.SetActive(true);
		 remoteGun.gameObject.SetActive(true);
	 }

	 [PunRPC]
	 void useHand2()
	 {
		 Gun.gameObject.SetActive(false);
		 remoteGun.gameObject.SetActive(false);
	 }
}
