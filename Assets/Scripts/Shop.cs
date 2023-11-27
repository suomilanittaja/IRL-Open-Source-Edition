using UnityEngine;
using System.Collections;
using Photon.Pun;

public class Shop : MonoBehaviourPunCallbacks
{

   public Money money;
   public GameObject Ui;
   public GameObject lemonadePrefab;
   public GameObject macaroniCasserolePrefab;
   public GameObject gunPrefab;
   public GameObject beerPrefab;
   public Transform Spawn;
   public PlayerController controll;


   void Start()
   {
     Ui.gameObject.SetActive(false);
   }

   void OnTriggerEnter (Collider other)
   {
     if (other.gameObject.CompareTag("Player"))
     {
       Ui.gameObject.SetActive(true);
       controll.ToggleCursorLock(false);
     }
   }

   void OnTriggerExit (Collider other)
   {
     Ui.gameObject.SetActive(false);
     controll.ToggleCursorLock(true);
   }

    private void InstantiatePrefab(GameObject prefab, int cost)
    {
        if (money.money >= cost)
        {
            money.money -= cost;
            PhotonNetwork.Instantiate(prefab.name, Spawn.position, Spawn.rotation);
            Ui.SetActive(false);
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

    void FixedUpdate()
   {
     if (controll == null)
        controll = GameObject.FindWithTag("Player").GetComponent<PlayerController>();

     if (Ui == null)
        {
            Ui = GameObject.FindGameObjectWithTag("ShopUi");
            Ui.gameObject.SetActive(false);
        }

     if (money == null)
        money = GameObject.FindWithTag("Player").GetComponent<Money>();
    }
}
