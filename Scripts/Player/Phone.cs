using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class Phone : MonoBehaviourPunCallbacks
{
    public GameObject smartPhone;
    public GameObject mainMenu;
    public GameObject jobMenu;
    public bool usingPhone = false;
    public TextMeshProUGUI text;
    public Money jobs;

    [SerializeField] private Material policeMaterial;
    [SerializeField] private Material robberMaterial;
    [SerializeField] private Material pizzaguyMaterial;



    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1) && usingPhone == false)
        {
            smartPhone.gameObject.SetActive(true);
            Cursor.lockState = CursorLockMode.None; //unlock cursor
            Cursor.visible = true; //make mouse visible
            usingPhone = true;
        }
        else if (Input.GetKeyDown(KeyCode.F1) && usingPhone == true)
        {
            smartPhone.gameObject.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked; //lock cursor
            Cursor.visible = false; //disable visible mouse
            usingPhone = false;
        }
    }
    public void Jobs()
    {
      mainMenu.gameObject.SetActive(false);
      jobMenu.gameObject.SetActive(true);
    }

    public void Bum()
    {
      text.text = "Job - Bum";
      jobs.job = 0;
    }

    public void Police()
    {
      text.text = "Job - Police";
      jobs.job = 1;
      if (photonView.IsMine) // Tarkistaa, että tämä on paikallisen pelaajan omistama objektin PhotonView
      {
          // Kutsu RPC:tä, joka vaihtaa materiaalin kaikille
          photonView.RPC("SetMaterialToPolice", RpcTarget.All);
      }
    }

    public void Robber()
    {
      text.text = "Job - Robber";
      jobs.job = 2;
      if (photonView.IsMine) // Tarkistaa, että tämä on paikallisen pelaajan omistama objektin PhotonView
      {
          // Kutsu RPC:tä, joka vaihtaa materiaalin kaikille
          photonView.RPC("SetMaterialToRobber", RpcTarget.All);
      }
    }

    public void PizzaGuy()
    {
        text.text = "Job - PizzaGuy";
        jobs.job = 3;
        if (photonView.IsMine) // Tarkistaa, että tämä on paikallisen pelaajan omistama objektin PhotonView
        {
            // Kutsu RPC:tä, joka vaihtaa materiaalin kaikille
            photonView.RPC("SetMaterialToPizzaguy", RpcTarget.All);
        }
    }

    public void Back()
    {
      mainMenu.gameObject.SetActive(true);
      jobMenu.gameObject.SetActive(false);
    }

    // RPC-metodi, joka synkronoi materiaalin kaikille pelaajille
    [PunRPC]
    public void SetMaterialToPolice()
    {
        // Hae Renderer-komponentti ja vaihda materiaali
        GetComponent<Renderer>().material = policeMaterial;
    }

    // RPC-metodi, joka synkronoi materiaalin kaikille pelaajille
    [PunRPC]
    public void SetMaterialToRobber()
    {
        // Hae Renderer-komponentti ja vaihda materiaali
        GetComponent<Renderer>().material = robberMaterial;
    }

    // RPC-metodi, joka synkronoi materiaalin kaikille pelaajille
    [PunRPC]
    public void SetMaterialToPizzaguy()
    {
        // Hae Renderer-komponentti ja vaihda materiaali
        GetComponent<Renderer>().material = pizzaguyMaterial;
    }
}
