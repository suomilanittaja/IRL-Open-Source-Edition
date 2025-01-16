using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Bullet : MonoBehaviourPun {
    public float Speed = 10;

	// Use this for initialization
	void Start () {
		if (photonView.IsMine)
		{
            gameObject.tag = "Bullet2";
        }
	}

	// Update is called once per frame
	void Update () {
        transform.Translate(0, 0, Speed * Time.deltaTime);

	}

    void OnTriggerEnter(Collider other)
    {
        if (photonView.IsMine) // Only the owner can destroy the bullet
        {
            StartCoroutine(DestroyBullet());
        }
    }

    IEnumerator DestroyBullet()
    {
        yield return new WaitForSeconds(0.1f);  // Small delay to allow damage to register
        PhotonNetwork.Destroy(gameObject);
    }
}
