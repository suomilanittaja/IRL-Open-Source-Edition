using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopButtons : MonoBehaviour
{
    public Shops shop;

    public void Update()
    {
        if (shop == null)
            shop = GameObject.FindWithTag("Manager").GetComponent<Shops>();
    }

    public void SpawnVEh()
    {
        shop.Spawn();
    }

    public void SpawnGun()
    {
        shop.gun();
    }

    public void SpawnLemonade()
    {
        shop.lemonade();
    }

    public void SpawnBeer()
    {
        shop.beer();
    }

    public void SpawnMacaroniCasserole()
    {
        shop.macaroniCasserole();
    }

}
