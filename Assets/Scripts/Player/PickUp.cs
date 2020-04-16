using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PickUp : MonoBehaviourPunCallbacks
{
	[Header("GameObjects")]
	public GameObject pickGun;
	public GameObject Gun;

	[Header("Scripts")]
	public PlayerController controller;
	private Raycast rayScript;

	[Header("Others")]
	[SerializeField] private string selectableTag = "Gun";
	private Transform _selection;

	void Start()
	{
    rayScript = GameObject.FindWithTag("Manager").GetComponent<Raycast>();
	}

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
		       photonView.RPC("pickUp", RpcTarget.All);
		   }
     }
   }

   [PunRPC]
   void pickUp()
   {
     Gun.gameObject.SetActive(true);
   }
}
