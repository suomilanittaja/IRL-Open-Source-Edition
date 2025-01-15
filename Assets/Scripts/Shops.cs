using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Shops : MonoBehaviourPunCallbacks
{
    [Header("Shop")]
    [SerializeField] private string selectableTag = "Selectable";
    public GameObject shopUi;
    public GameObject lemonadePrefab;
    public GameObject macaroniCasserolePrefab;
    public GameObject gunPrefab;
    public GameObject beerPrefab;
    public Transform shopItemSpawn;

    [Header("VehicleSpawn")]
    [SerializeField] private string selectableTag2 = "Selectable";
    public Transform vehicleSpawn;           //car spawning position
    public GameObject vehicleUi;             //car spawning ui
    public GameObject carPrefab;             //car prefab


    [Header("Atm")]
    [SerializeField] private string selectableTag3 = "Selectable";
    public GameObject atmUi;                 //Ui for ATM

    [Header("Scripts")]
    public Money moneyScript;
    public PlayerController playerControllerScript; //player controller
    public Stats statsScript;
    public Raycast rayScript;
    public GameSetupController GameSetupControllerScript;

    [Header("Others")]
    private Transform _selection;
    private GameObject Hit;                 //gameobject what is hitted by raycast
    public bool isOpen = false;             //is it open
    private bool canOpen = true;            //can i open it

    // Start is called before the first frame update
    void Start()
    {
        shopUi.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameSetupControllerScript.PlayerSpawned == true)
        {
            if (playerControllerScript == null)                                       //if controll empty
                playerControllerScript = GameObject.FindWithTag("Player").GetComponent<PlayerController>();  //find it with tag


            if (shopUi == null)                                         //if shopui empty
            {
                shopUi = GameObject.FindGameObjectWithTag("ShopUi");    //find it with tag
                shopUi.gameObject.SetActive(false);                     //and set it false
            }


            if (vehicleUi == null)                                      //if VehicleUi empty
            {
                vehicleUi = GameObject.FindGameObjectWithTag("CarShopUi");                      //find it with tag
                vehicleUi.SetActive(false);                             //and set it false
            }


            if (moneyScript == null)                                          //if money empty 
                moneyScript = GameObject.FindWithTag("Player").GetComponent<Money>();                 //find it with tag


            if (rayScript == null)                                      //if raySript emty
                rayScript = GameObject.FindWithTag("Player").GetComponent<Raycast>();           //find it with tag


            if (statsScript == null)                                          //if stats empty
                statsScript = GameObject.FindWithTag("Player").GetComponent<Stats>();                 //find it with tag


            if (atmUi == null)                                          //if AtmUi empty
            {
                atmUi = GameObject.FindGameObjectWithTag("AtmUi");      //find it with tag
                atmUi.SetActive(false);                                 //and set it false
            }


            Hit = rayScript.rayHitted;                                  //what it hitted

            if (rayScript.rayHitted != null)                            //if it has hitted something
            {
                if (rayScript.rayHitted.CompareTag(selectableTag) && rayScript.hitDis <= 2 && canOpen == true)  //if it hitted this tag from close distance and can open ui 
                {
                    shopUi.gameObject.SetActive(true);                  //open ui
                    playerControllerScript.ToggleCursorLock(false);                   //free the mouse
                    isOpen = true;                                      //shop has opened
                }


                if (rayScript.rayHitted.CompareTag(selectableTag2) && rayScript.hitDis <= 2 && canOpen == true)
                {
                    vehicleUi.gameObject.SetActive(true);
                    playerControllerScript.ToggleCursorLock(false);
                    isOpen = true;
                }
                

                if (rayScript.rayHitted.CompareTag(selectableTag3) && rayScript.hitDis <= 2 && canOpen == true)
                {
                    atmUi.gameObject.SetActive(true);
                    playerControllerScript.ToggleCursorLock(false);
                    isOpen = true;
                }

                if (rayScript.rayHitted.CompareTag(selectableTag) == false && rayScript.rayHitted.CompareTag(selectableTag2) == false && rayScript.rayHitted.CompareTag(selectableTag3) == false && isOpen == true)
                {
                    atmUi.gameObject.SetActive(false);
                    vehicleUi.gameObject.SetActive(false);
                    shopUi.gameObject.SetActive(false);
                    playerControllerScript.ToggleCursorLock(true);
                    isOpen = false;
                    canOpen = false;
                    StartCoroutine(Wait());
                }
            }
        }
    }

    private void InstantiatePrefab(GameObject prefab, int cost)
    {
        if (moneyScript.money >= cost)
        {
            moneyScript.money -= cost;
            PhotonNetwork.Instantiate(prefab.name, shopItemSpawn.position, shopItemSpawn.rotation);
            shopUi.SetActive(false);
            playerControllerScript.ToggleCursorLock(true);
        }
    }

    public void SpawnLemonade()
    {
        InstantiatePrefab(lemonadePrefab, 10);
        StartCoroutine(Wait());
        canOpen = false;
    }

    public void SpawnMacaroniCasserole()
    {
        InstantiatePrefab(macaroniCasserolePrefab, 15);
        StartCoroutine(Wait());
        canOpen = false;
    }

    public void SpawnGun()
    {
        InstantiatePrefab(gunPrefab, 50);
        StartCoroutine(Wait());
        canOpen = false;
    }

    public void SpawnBeer()
    {
        InstantiatePrefab(beerPrefab, 20);
        StartCoroutine(Wait());
        canOpen = false;
    }

    public void SpawnCar() //spawn car
    {
        playerControllerScript.ToggleCursorLock(true);
        vehicleUi.SetActive(false);
        StartCoroutine(Wait());
        canOpen = false;
        PhotonNetwork.Instantiate(carPrefab.name, vehicleSpawn.transform.position, vehicleSpawn.rotation);
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(3);
        canOpen = true;
    }
}

