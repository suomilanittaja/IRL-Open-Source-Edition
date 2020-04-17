using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject main;
    public GameObject options;
    private PlayerController controll;
    private bool Opened = false;

    void Start()
    {
      main.SetActive(false);
    }

    void Update()
    {
      if (controll == null)
         controll = GameObject.FindWithTag("Player").GetComponent<PlayerController>();

      if (Input.GetKeyDown(KeyCode.Escape) && Opened == false)
      {
        Opened = true;
        main.SetActive(true);
        controll.UnLock();
      }
      else if (Input.GetKeyDown(KeyCode.Escape) && Opened == true)
      {
        Opened = false;
        main.SetActive(false);
        controll.Lock();
      }
    }

    public void Resume()
    {
      main.SetActive(false);
      controll.Lock();
      Opened = false;
    }

    public void SetQuality (int qualityIndex)
    {
      QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullscreen (bool isFullscreen)
    {
      Screen.fullScreen = isFullscreen;
    }

    public void Options()
    {
      main.SetActive(false);
      options.SetActive(true);
    }

    public void Back()
    {
      main.SetActive(true);
      options.SetActive(false);
    }

    public void Quit()
    {
      Application.Quit();
    }
}
