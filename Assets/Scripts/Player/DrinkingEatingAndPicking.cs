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
    private GameObject Drink;

    [Header("GameObjects for Eating")]
    public GameObject EatText;
    private GameObject Food;

    [Header("GameObjects for picking Gun")]
    private GameObject pickGun;
    public GameObject Gun;
    public GameObject remoteGun;

    [Header("Scripts")]
    public PlayerController controller;
    public Stats stats;
    public Raycast rayScript;
    private PhotonView PhotonView;

    [Header("Others")]
    public bool usingGun = false;
    private Transform _selection;

    private void Update()
    {
        Drink = rayScript.rayHitted;

        if (rayScript.rayHitted != null)
        {
            if (rayScript.rayHitted.CompareTag(selectableTag) && Input.GetKeyDown(KeyCode.F) && rayScript.hitDis <= 5)
            {
                PhotonView = (PhotonView)Drink.GetComponent(typeof(PhotonView));
                TransferOwnershipDrink();
                stats.drunk();
            }

            if (rayScript.rayHitted.CompareTag(selectableTag2) && Input.GetKeyDown(KeyCode.F) && rayScript.hitDis <= 5)
            {
                PhotonView = (PhotonView)Drink.GetComponent(typeof(PhotonView));
                TransferOwnershipDrink();
                stats.Drink();
            }

            if (rayScript.rayHitted.CompareTag(selectableTag) && rayScript.hitDis <= 5)
            {
                DrinkText.gameObject.SetActive(true);
            }
            else
                DrinkText.gameObject.SetActive(false);
        }

        Food = rayScript.rayHitted;

        if (rayScript.rayHitted != null)
        {
            if (rayScript.rayHitted.CompareTag(selectableTag3) && Input.GetKeyDown(KeyCode.F) && rayScript.hitDis <= 5)
            {
                PhotonView = (PhotonView)Food.GetComponent(typeof(PhotonView));
                TransferOwnershipFood();
                stats.Eat();
                print("key was pressed");
            }

            if (rayScript.rayHitted.CompareTag(selectableTag3) && rayScript.hitDis <= 5)
            {
                EatText.gameObject.SetActive(true);
            }
            else
                EatText.gameObject.SetActive(false);
        }

        pickGun = rayScript.rayHitted;

        if (rayScript.rayHitted != null)
        {
            if (rayScript.rayHitted.CompareTag(selectableTag4) && Input.GetKeyDown(KeyCode.F) && rayScript.hitDis <= 5)
            {
                PhotonView = (PhotonView)Gun.GetComponent(typeof(PhotonView));
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

            if (usingGun)
            {
                photonView.RPC("useGun2", RpcTarget.All);
            }
        }
    }

   public void TransferOwnershipDrink()
   {
       if (PhotonView.Owner.IsLocal == false)
       {
           Debug.Log("Requesting Ownership");
           PhotonView.RequestOwnership();
           PhotonNetwork.Destroy(Drink);
       }
       else
        PhotonNetwork.Destroy(Drink);
   }

    public void TransferOwnershipFood()
    {
        if (PhotonView.Owner.IsLocal == false)
        {
            Debug.Log("Requesting Ownership");
            PhotonView.RequestOwnership();
            PhotonNetwork.Destroy(Food);
        }
        else
            PhotonNetwork.Destroy(Food);
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