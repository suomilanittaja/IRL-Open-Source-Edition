using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Bullet : MonoBehaviourPun {

    public float bulletSpeed = 20;                //set bullet speed

	void Start () 
    {
		if (photonView.IsMine)     //if bullet is mine
		{
            gameObject.tag = "Bullet2";   //change it tag to bullet2 so self damage is not possible
        }
	}

	// Update is called once per frame
	void Update () {
        transform.Translate(0, 0, bulletSpeed * Time.deltaTime);          //make it move
	}

    void OnTriggerEnter(Collider other)
    {
        if (photonView.IsMine) // Only the owner can destroy the bullet
        {
            StartCoroutine(DestroyBullet());        //start timer to destroy bullet when it hit something
        }
    }

    IEnumerator DestroyBullet()
    {
        yield return new WaitForSeconds(0.1f);  // Small delay to allow damage to register
        PhotonNetwork.Destroy(gameObject);      //destroy bullet
    }
}
