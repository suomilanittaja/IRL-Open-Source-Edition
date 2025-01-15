using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject main;
    public GameObject options;
    public PlayerController controll;
    public bool isOpened = false;

    void Start()
    {
      main.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (!isOpened)
            {
                isOpened = true;
                main.SetActive(true);
                controll?.ToggleCursorLock(false);
            }
            else
            {
                isOpened = false;
                main.SetActive(false);
                controll?.ToggleCursorLock(true);
            }
        }
    }

    public void Resume() //resume button
    {
      main.SetActive(false);
      controll.ToggleCursorLock(true);
      isOpened = false;
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
