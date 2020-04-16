using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class EnterandExit : MonoBehaviourPunCallbacks
{
  public CarController carControll;
	public Transform exitPoint;
	public GameObject Camera;
  public PhotonView photonview;

  public bool canEnter;
  public bool isEntered;

  private GameObject player;
  private PlayerController playerControll;
  private Transform playerPos;
  private Raycast RaycastScript;
  private GameObject manager;

	void OnTriggerExit (Collider other)
	{
		canEnter = false;
	}

	void OnTriggerEnter (Collider Hit)
	{
		if (Hit.gameObject.tag == "Player" && isEntered == false)
		{
				canEnter = true;
		}
	}
	void Update()
	{
		if (canEnter == true && Input.GetKeyDown(KeyCode.Return))
		{
      TransferOwnership();
      carControll.enabled = true;
      RaycastScript.enabled = false;
      Camera.gameObject.SetActive(true);
      playerControll.isEntered = true;
      photonView.RPC("Enter", RpcTarget.All);
		}

		if (canEnter == false && Input.GetKeyDown(KeyCode.Return))
		{
      playerPos.transform.position = exitPoint.transform.position;
      player.gameObject.SetActive(true);
      carControll.enabled = false;
      RaycastScript.enabled = true;
      Camera.gameObject.SetActive(false);
      playerControll.isEntered = false;
      photonView.RPC("Exit", RpcTarget.All);
		}

	}

	void Start ()
	{
		Camera.gameObject.SetActive(false);
		carControll.enabled = false;
    if (player == null)
            player = GameObject.FindWithTag("Player");
            playerPos = (Transform)player.GetComponent(typeof(Transform));
            playerControll = (PlayerController)player.GetComponent(typeof(PlayerController));
            RaycastScript = GameObject.FindWithTag("Manager").GetComponent<Raycast>();
	}

  IEnumerator Timer()
	{
		yield return new WaitForSeconds(1);
		canEnter = false;
	}

  [PunRPC]
  void Enter()
  {
    isEntered = true;
    StartCoroutine(Timer());
  }

  [PunRPC]
  void Exit()
  {
    canEnter = false;
    isEntered = false;
  }

  void TransferOwnership()
  {
      if (photonView.Owner.IsLocal == false)
      {
          Debug.Log("Requesting Ownership");
          photonView.RequestOwnership();
      }
  }
}
