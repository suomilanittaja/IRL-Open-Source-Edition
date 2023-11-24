using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopButtons : MonoBehaviour
{


    public VehicleSpawner vehSpawn;
    public Shop shopSpawn;

    public void Update()
    {
        if (vehSpawn == null)
            vehSpawn = GameObject.FindWithTag("vehSpawner").GetComponent<VehicleSpawner>();
        if (shopSpawn == null)
            shopSpawn = GameObject.FindWithTag("shopSpawner").GetComponent<Shop>();
    }
    public void SpawnVEh()
    {
        vehSpawn.Spawn();
    }

    public void SpawnGun()
    {
        shopSpawn.gun();
    }

    public void SpawnLemonade()
    {
        shopSpawn.lemonade();
    }

    public void SpawnBeer()
    {
        shopSpawn.beer();
    }

    public void SpawnMacaroniCasserole()
    {
        shopSpawn.macaroniCasserole();
    }

}
