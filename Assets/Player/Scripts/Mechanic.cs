using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mechanic : MonoBehaviour
{
    //public RCC_UIDashboardDisplay Rcc;
    public Jobs mechanic;
    // Start is called before the first frame update
    void Start()
    {
      //Rcc.displayType = RCC_UIDashboardDisplay.DisplayType.Off;

    }

    // Update is called once per frame
    
    /*void Update()
    {
      if (Input.GetKeyDown(KeyCode.F2) && mechanic.job == 2 && Rcc.displayType == RCC_UIDashboardDisplay.DisplayType.Full)
      {
        Rcc.displayType = RCC_UIDashboardDisplay.DisplayType.Customization;
        Cursor.lockState = CursorLockMode.None; //unlock cursor
        Cursor.visible = true; //make mouse visible
      }
      else if (Input.GetKeyDown(KeyCode.F2) && Rcc.displayType == RCC_UIDashboardDisplay.DisplayType.Customization)
      {
        Rcc.displayType = RCC_UIDashboardDisplay.DisplayType.Full;
        Cursor.lockState = CursorLockMode.Locked; //lock cursor
        Cursor.visible = false; //disable visible mouse
      }
    }*/
}
