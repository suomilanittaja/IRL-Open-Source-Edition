using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class EnterandExit : MonoBehaviourPunCallbacks
{
    public CarController carControllerScript;
	public Transform exitPointPosition;
	public GameObject cameraObject;
    public PhotonView photonViewScript;

    public bool canEnter;
    public bool isEntered;

    public GameObject playerObject;
    public PlayerController playerControllerScript;
    public Transform playerPosition;
    public Raycast raycastScript;
    public CameraController cameraControllerScript;

	void OnTriggerExit (Collider other)
	{
        if (isEntered == false)
        {
            playerControllerScript.canEnter = false;

            canEnter = false;
            playerObject = null;
            playerPosition = null;
            playerControllerScript = null;
            raycastScript = null;
        }
    }

	void OnTriggerEnter (Collider other)
	{
		if (other.CompareTag("Player") && isEntered == false)
		{
            playerObject = other.gameObject;                              //set player object
            playerPosition = (Transform)playerObject.GetComponent(typeof(Transform));
            playerControllerScript = (PlayerController)playerObject.GetComponent(typeof(PlayerController));
            raycastScript = (Raycast)playerObject.GetComponent(typeof(Raycast));

            canEnter = true;
            playerControllerScript.canEnter = true;
        }
	}


	void Update()
	{
        if (cameraObject == null)
            cameraObject = GameObject.FindWithTag("VehicleCam");
            cameraControllerScript = (CameraController)cameraObject.GetComponent(typeof(CameraController));

        if (playerObject != null)
        {
            if (canEnter == true && playerControllerScript.canEnter == true && playerControllerScript.isEntered == false && Input.GetKeyDown(KeyCode.Return) && isEntered == false)
            {
                TransferOwnership();
                raycastScript = GameObject.FindWithTag("Player").GetComponent<Raycast>();
                gameObject.tag = "Vehicle";
                cameraControllerScript.Check();
                carControllerScript.enabled = true;
                raycastScript.enabled = false;
                playerControllerScript.isEntered = true;
                isEntered = true;
                photonView.RPC("Enter", RpcTarget.All);
            }

            if (canEnter == false && playerControllerScript.canEnter == false && playerControllerScript.isEntered == true && Input.GetKeyDown(KeyCode.Return) && isEntered == true && photonView.Owner.IsLocal == true)
            {
                gameObject.tag = "Untagged";
                playerPosition.transform.position = exitPointPosition.transform.position;
                playerObject.gameObject.SetActive(true);
                carControllerScript.enabled = false;
                raycastScript.enabled = true;
                playerControllerScript.isEntered = false;
                photonView.RPC("Exit", RpcTarget.All);
                isEntered = false;
            }
        }
	}

	void Start ()
	{
		carControllerScript.enabled = false;
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
            playerControllerScript.canEnter = false;
        }
    }

    [PunRPC]
    void Exit()
    {
        canEnter = true;
        isEntered = false;
        playerControllerScript.canEnter = true;
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
