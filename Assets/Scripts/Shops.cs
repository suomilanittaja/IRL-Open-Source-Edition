using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Shops : MonoBehaviourPunCallbacks
{
    [Header("Shop")]
    [SerializeField] private string selectableTag = "Selectable";
    public GameObject ShopUi;
    public GameObject lemonadePrefab;
    public GameObject macaroniCasserolePrefab;
    public GameObject gunPrefab;
    public GameObject beerPrefab;
    public Transform ShopItemSpawn;

    [Header("VehicleSpawn")]
    [SerializeField] private string selectableTag2 = "Selectable";
    public Transform VehicleSpawn;  //car spawning position
    public GameObject VehicleUi;       //car spawning ui
    public GameObject carPrefab; //car prefab

    [Header("Atm")]
    [SerializeField] private string selectableTag3 = "Selectable";
    public GameObject AtmUi;   //Ui for ATM

    [Header("Scripts")]
    public Money money;
    public PlayerController controll; //player controller
    public Stats stats;
    public Raycast rayScript;

    [Header("Others")]
    private Transform _selection;
    private GameObject Hit;
    private bool isOpen = false;

    // Start is called before the first frame update
    void Start()
    {
        ShopUi.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (controll == null)
            controll = GameObject.FindWithTag("Player").GetComponent<PlayerController>();

        if (ShopUi == null)
        {
            ShopUi = GameObject.FindGameObjectWithTag("ShopUi");
            ShopUi.gameObject.SetActive(false);
        }

        if (VehicleUi == null)
        {
            VehicleUi = GameObject.FindGameObjectWithTag("CarShopUi");
            VehicleUi.SetActive(false);
        }

        if (money == null)
            money = GameObject.FindWithTag("Player").GetComponent<Money>();

        if (rayScript == null)
            rayScript = GameObject.FindWithTag("Player").GetComponent<Raycast>();

        if (stats == null)
            stats = GameObject.FindWithTag("Player").GetComponent<Stats>();

        if (AtmUi == null)  //find AtmUi
        {
            AtmUi = GameObject.FindGameObjectWithTag("AtmUi");
            AtmUi.SetActive(false);
        }

        Hit = rayScript.rayHitted;

        if (rayScript.rayHitted != null)
        {
            if (rayScript.rayHitted.CompareTag(selectableTag) && rayScript.hitDis <= 2)
            {
                ShopUi.gameObject.SetActive(true);
                controll.ToggleCursorLock(false);
                isOpen = true;
            }
            else if (rayScript.rayHitted.CompareTag(selectableTag2) == false && rayScript.rayHitted.CompareTag(selectableTag3) == false && rayScript.rayHitted.CompareTag(selectableTag) == false && isOpen == true)
            {
                ShopUi.gameObject.SetActive(false);
                controll.ToggleCursorLock(true);
                isOpen = false;
            }

            if (rayScript.rayHitted.CompareTag(selectableTag2) && rayScript.hitDis <= 2)
            {
                VehicleUi.gameObject.SetActive(true);
                controll.ToggleCursorLock(false);
                isOpen = true;
            }
            else if (rayScript.rayHitted.CompareTag(selectableTag) == false && rayScript.rayHitted.CompareTag(selectableTag3) == false && rayScript.rayHitted.CompareTag(selectableTag2) == false && isOpen == true)
            {
                VehicleUi.gameObject.SetActive(false);
                controll.ToggleCursorLock(true);
                isOpen = false;
            }

            if (rayScript.rayHitted.CompareTag(selectableTag3) && rayScript.hitDis <= 2)
            {
                AtmUi.gameObject.SetActive(true);
                controll.ToggleCursorLock(false);
                isOpen = true;
            }
            else if (rayScript.rayHitted.CompareTag(selectableTag) == false && rayScript.rayHitted.CompareTag(selectableTag2) == false && rayScript.rayHitted.CompareTag(selectableTag3) == false && isOpen == true)
            {
                AtmUi.gameObject.SetActive(false);
                controll.ToggleCursorLock(true);
                isOpen = false;
            }
        }
    }

    private void InstantiatePrefab(GameObject prefab, int cost)
    {
        if (money.money >= cost)
        {
            money.money -= cost;
            PhotonNetwork.Instantiate(prefab.name, ShopItemSpawn.position, ShopItemSpawn.rotation);
            ShopUi.SetActive(false);
            controll.ToggleCursorLock(true);
        }
    }

    public void lemonade()
    {
        InstantiatePrefab(lemonadePrefab, 10);
    }

    public void macaroniCasserole()
    {
        InstantiatePrefab(macaroniCasserolePrefab, 15);
    }

    public void gun()
    {
        InstantiatePrefab(gunPrefab, 50);
    }

    public void beer()
    {
        InstantiatePrefab(beerPrefab, 20);
    }

    public void Spawn() //spawn car
    {
        controll.ToggleCursorLock(true);
        VehicleUi.SetActive(false);
        PhotonNetwork.Instantiate(carPrefab.name, VehicleSpawn.transform.position, VehicleSpawn.rotation);
    }
}
