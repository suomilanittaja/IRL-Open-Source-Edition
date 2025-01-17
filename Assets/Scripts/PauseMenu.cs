using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject optionsMenu;
    public PlayerController playercontrollerScript;
    public bool isOpened = false;

    void Start()
    {
      mainMenu.SetActive(false);    //hide mainmenu
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))         //if esc pressed
        {
            if (!isOpened)                          //if mainmenu not open
            {
                OpenMenu();                         //open mainmenu
            }
            else                                    //if  mainmenu already open
            {
                CloseMenu();                        //close mainmenu
            }
        }
    }

    public void OpenMenu()                          //open mainmenu
    {
        isOpened = true;
        mainMenu.SetActive(true);                   //active mainmenu 
        playercontrollerScript.ToggleCursorLock(false); //unlock cursor
    }

    public void CloseMenu()                         //close mainmenu
    {
        isOpened = false;
        mainMenu.SetActive(false);                  //disable mainmenu
        playercontrollerScript.ToggleCursorLock(true);  //lock cursor
    }

    public void Resume() //resume button
    {
      mainMenu.SetActive(false);
      playercontrollerScript.ToggleCursorLock(true);
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
      mainMenu.SetActive(false);
      optionsMenu.SetActive(true);
    }

    public void Back()
    {
      mainMenu.SetActive(true);
      optionsMenu.SetActive(false);
    }

    public void Quit()
    {
      Application.Quit();
    }
}
