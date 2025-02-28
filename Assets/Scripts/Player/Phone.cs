﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class Phone : MonoBehaviourPunCallbacks
{
    public GameObject smartPhoneObject;
    public GameObject mainMenu;
    public GameObject jobMenu;
    public bool usingPhone = false;
    public TextMeshProUGUI currentJobText;
    public Money moneyScript;
    public PlayerController playerControllerScript;

    [SerializeField] private Material policeMaterial;
    [SerializeField] private Material robberMaterial;
    [SerializeField] private Material pizzaguyMaterial;
    [SerializeField] private Material bumMaterial;



    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1) && usingPhone == false)
        {
            smartPhoneObject.gameObject.SetActive(true);
            playerControllerScript.ToggleCursorLock(false);
            usingPhone = true;
        }
        else if (Input.GetKeyDown(KeyCode.F1) && usingPhone == true)
        {
            smartPhoneObject.gameObject.SetActive(false);
            playerControllerScript.ToggleCursorLock(true);
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
        currentJobText.text = "Job - Bum";
        moneyScript.job = 0;

        if (photonView.IsMine) // Tarkistaa, että tämä on paikallisen pelaajan omistama objektin PhotonView
        {
            // Kutsu RPC:tä, joka vaihtaa materiaalin kaikille
            photonView.RPC("SetMaterialToBum", RpcTarget.All);
        }
    }

    public void Police()
    {
        currentJobText.text = "Job - Police";
        moneyScript.job = 1;

        if (photonView.IsMine) // Tarkistaa, että tämä on paikallisen pelaajan omistama objektin PhotonView
        {
            // Kutsu RPC:tä, joka vaihtaa materiaalin kaikille
            photonView.RPC("SetMaterialToPolice", RpcTarget.All);
        }
    }

    public void Robber()
    {
        currentJobText.text = "Job - Robber";
        moneyScript.job = 2;

        if (photonView.IsMine) // Tarkistaa, että tämä on paikallisen pelaajan omistama objektin PhotonView
        {
            // Kutsu RPC:tä, joka vaihtaa materiaalin kaikille
            photonView.RPC("SetMaterialToRobber", RpcTarget.All);
        }
    }

    public void PizzaGuy()
    {
        currentJobText.text = "Job - PizzaGuy";
        moneyScript.job = 3;

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
    public void SetMaterialToBum()
    {
        Debug.Log("PunRPCCall");
        // Hae Renderer-komponentti ja vaihda materiaali
        GetComponent<Renderer>().material = bumMaterial;
    }

    // RPC-metodi, joka synkronoi materiaalin kaikille pelaajille
    [PunRPC]
    public void SetMaterialToPolice()
    {
        Debug.Log("PunRPCCall");
        // Hae Renderer-komponentti ja vaihda materiaali
        GetComponent<Renderer>().material = policeMaterial;
    }

    // RPC-metodi, joka synkronoi materiaalin kaikille pelaajille
    [PunRPC]
    public void SetMaterialToRobber()
    {
        Debug.Log("PunRPCCall");
        // Hae Renderer-komponentti ja vaihda materiaali
        GetComponent<Renderer>().material = robberMaterial;
    }

    // RPC-metodi, joka synkronoi materiaalin kaikille pelaajille
    [PunRPC]
    public void SetMaterialToPizzaguy()
    {
        Debug.Log("PunRPCCall");
        // Hae Renderer-komponentti ja vaihda materiaali
        GetComponent<Renderer>().material = pizzaguyMaterial;
    }
}
