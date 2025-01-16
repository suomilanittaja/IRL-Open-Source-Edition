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

    public GameObject player;
    public PlayerController playerControll;
    public Transform playerPos;
    public Raycast RaycastScript;
    public CameraController cameraController;

	void OnTriggerExit (Collider other)
	{
        if (isEntered == false)
        {
            playerControll.canEnter = false;

            canEnter = false;
            player = null;
            playerPos = null;
            playerControll = null;
            RaycastScript = null;
        }
    }

	void OnTriggerEnter (Collider other)
	{
		if (other.CompareTag("Player") && isEntered == false)
		{
            player = other.gameObject;                              //set player object
            playerPos = (Transform)player.GetComponent(typeof(Transform));
            playerControll = (PlayerController)player.GetComponent(typeof(PlayerController));
            RaycastScript = (Raycast)player.GetComponent(typeof(Raycast));

            canEnter = true;
            playerControll.canEnter = true;
        }
	}


	void Update()
	{
        if (Camera == null)
            Camera = GameObject.FindWithTag("VehicleCam");
            cameraController = (CameraController)Camera.GetComponent(typeof(CameraController));

        if (player != null)
        {
            if (canEnter == true && playerControll.canEnter == true && playerControll.isEntered == false && Input.GetKeyDown(KeyCode.Return) && isEntered == false)
            {
                TransferOwnership();
                RaycastScript = GameObject.FindWithTag("Player").GetComponent<Raycast>();
                gameObject.tag = "Vehicle";
                cameraController.Check();
                carControll.enabled = true;
                RaycastScript.enabled = false;
                playerControll.isEntered = true;
                isEntered = true;
                photonView.RPC("Enter", RpcTarget.All);
            }

            if (canEnter == false && playerControll.canEnter == false && playerControll.isEntered == true && Input.GetKeyDown(KeyCode.Return) && isEntered == true && photonView.Owner.IsLocal == true)
            {
                gameObject.tag = "Untagged";
                playerPos.transform.position = exitPoint.transform.position;
                player.gameObject.SetActive(true);
                carControll.enabled = false;
                RaycastScript.enabled = true;
                playerControll.isEntered = false;
                photonView.RPC("Exit", RpcTarget.All);
                isEntered = false;
            }
        }
	}

	void Start ()
	{
		carControll.enabled = false;
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

        IEnumerator Timer()
        {
            yield return new WaitForSeconds(1);
            canEnter = false;
            playerControll.canEnter = false;
        }
    }

    [PunRPC]
    void Exit()
    {
        canEnter = true;
        isEntered = false;
        playerControll.canEnter = true;
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
