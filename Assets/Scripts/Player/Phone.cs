using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Phone : MonoBehaviour
{
    public GameObject smartPhone;
    public GameObject mainMenu;
    public GameObject jobMenu;
    public bool usingPhone = false;
    public TextMeshProUGUI text;
    public Jobs jobs;
    // Start is called before the first frame update
    void Start()
    {

    }

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
    }
    public void Mechanic()
    {
      text.text = "Job - Mechanic";
      jobs.job = 2;
    }
    public void Back()
    {
      mainMenu.gameObject.SetActive(true);
      jobMenu.gameObject.SetActive(false);
    }
}
