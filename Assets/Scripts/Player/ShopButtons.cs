using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopButtons : MonoBehaviour
{
    public Shops shopsScript;

    public void Update()
    {
        if (shopsScript == null)
            shopsScript = GameObject.FindWithTag("Manager").GetComponent<Shops>();
    }

    public void SpawnVEh()
    {
        shopsScript.SpawnCar();
    }

    public void SpawnGun()
    {
        shopsScript.SpawnGun();
    }

    public void SpawnLemonade()
    {
        shopsScript.SpawnLemonade();
    }

    public void SpawnBeer()
    {
        shopsScript.SpawnBeer();
    }

    public void SpawnMacaroniCasserole()
    {
        shopsScript.SpawnMacaroniCasserole();
    }

}
