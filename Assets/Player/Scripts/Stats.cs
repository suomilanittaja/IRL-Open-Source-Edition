using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stats : MonoBehaviour
{
  public PlayerController controll;
  public Slider HealthBar;

  void Update()
  {
    if (controll == null)
    {
      GameObject tempobj = GameObject.FindWithTag("Player");
      controll = (PlayerController)tempobj.GetComponent(typeof(PlayerController));
      HealthBar.maxValue = controll.Health;
    }
    updateUI();
  }

  private void updateUI()
  {
    //Health = Mathf.Clamp(Health, 0, 100f);
    HealthBar.value = controll.Health;
  }
}
