using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Bullet : MonoBehaviourPun {
    public float Speed = 10;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
        transform.Translate(0, 0, Speed * Time.deltaTime);

	}

  void OnCollisionEnter()
  {
    Destroy(gameObject);
    StartCoroutine(Timer());
  }

  IEnumerator Timer()
  {
   yield return new WaitForSeconds(1);
   PhotonNetwork.Destroy(gameObject);
  }
}
