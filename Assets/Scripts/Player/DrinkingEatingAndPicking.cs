using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using Photon.Pun;

public class DrinkingEatingAndPicking : MonoBehaviourPunCallbacks
{
    [Header("Drinking tags")]
    [SerializeField] private string selectableTag = "Selectable";
    [SerializeField] private string selectableTag2 = "Selectable";

    [Header("Eating tags")]
    [SerializeField] private string selectableTag3 = "Selectable";

    [Header("Picking tags")]
    [SerializeField] private string selectableTag4 = "Gun";

    [Header("GameObjects for Drinking")]
    public GameObject DrinkText;

    [Header("GameObjects for Eating")]
    public GameObject EatText;

    [Header("GameObjects for picking Gun")]
    public GameObject gunObject;
    public GameObject useText;
    public GameObject remoteGun;

    [Header("Scripts")]
    public PlayerController playerControllerScript;
    public Stats statsScript;
    public Raycast rayScript;
    private PhotonView photonViewScript;

    [Header("Others")]
    public bool usingGun = false;
    private Transform _selection;
    private GameObject Hit;

    private void Update()
    {
        Hit = rayScript.rayHittedObject;

        if (rayScript.rayHittedObject != null)
        {
            if (rayScript.rayHittedObject.CompareTag(selectableTag) && Input.GetKeyDown(KeyCode.F) && rayScript.hitDis <= 5)
            {
                photonViewScript = (PhotonView)Hit.GetComponent(typeof(PhotonView));
                TransferOwnershipDrink();
                statsScript.drunk();
            }

            if (rayScript.rayHittedObject.CompareTag(selectableTag2) && Input.GetKeyDown(KeyCode.F) && rayScript.hitDis <= 5)
            {
                photonViewScript = (PhotonView)Hit.GetComponent(typeof(PhotonView));
                TransferOwnershipDrink();
                statsScript.Drink();
            }

            if (rayScript.rayHittedObject.CompareTag(selectableTag) || rayScript.rayHittedObject.CompareTag(selectableTag2) && rayScript.hitDis <= 5)
            {
                DrinkText.gameObject.SetActive(true);
            }
            else
                DrinkText.gameObject.SetActive(false);
        }


        if (rayScript.rayHittedObject != null)
        {
            if (rayScript.rayHittedObject.CompareTag(selectableTag3) && Input.GetKeyDown(KeyCode.F) && rayScript.hitDis <= 5)
            {
                photonViewScript = (PhotonView)Hit.GetComponent(typeof(PhotonView));
                TransferOwnershipFood();
                statsScript.Eat();
                print("key was pressed");
            }

            if (rayScript.rayHittedObject.CompareTag(selectableTag3) && rayScript.hitDis <= 5)
            {
                EatText.gameObject.SetActive(true);
            }
            else
                EatText.gameObject.SetActive(false);
        }


        if (rayScript.rayHittedObject != null)
        {
            if (rayScript.rayHittedObject.CompareTag(selectableTag4) && Input.GetKeyDown(KeyCode.F) && rayScript.hitDis <= 5)
            {
                photonViewScript = (PhotonView)Hit.GetComponent(typeof(PhotonView));
                print("key was pressed");
                PhotonNetwork.Destroy(Hit);
                playerControllerScript.hasGun = true;
                usingGun = true;
                photonView.RPC("pickUp", RpcTarget.All);
            }

            if (rayScript.rayHittedObject.CompareTag(selectableTag4) && rayScript.hitDis <= 5)
            {
                useText.gameObject.SetActive(true);
            }
            else
                useText.gameObject.SetActive(false);

            if (usingGun == false)
            {
                photonView.RPC("useHand2", RpcTarget.All);
            }

            if (usingGun)
            {
                photonView.RPC("useGun2", RpcTarget.All);
            }
        }
    }

   public void TransferOwnershipDrink()
   {
       if (photonViewScript.Owner.IsLocal == false)
       {
           Debug.Log("Requesting Ownership");
           photonViewScript.RequestOwnership();
           PhotonNetwork.Destroy(Hit);
       }
       else
        PhotonNetwork.Destroy(Hit);
   }

    public void TransferOwnershipFood()
    {
        if (photonViewScript.Owner.IsLocal == false)
        {
            Debug.Log("Requesting Ownership");
            photonViewScript.RequestOwnership();
            PhotonNetwork.Destroy(Hit);
        }
        else
            PhotonNetwork.Destroy(Hit);
    }

    [PunRPC]
    void pickUp()
    {
        gunObject.gameObject.SetActive(true);
        remoteGun.gameObject.SetActive(true);
    }

    [PunRPC]
    void useGun2()
    {
        gunObject.gameObject.SetActive(true);
        remoteGun.gameObject.SetActive(true);
    }

    [PunRPC]
    void useHand2()
    {
        gunObject.gameObject.SetActive(false);
        remoteGun.gameObject.SetActive(false);
    }
}
