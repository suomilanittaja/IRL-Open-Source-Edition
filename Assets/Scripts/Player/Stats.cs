using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stats : MonoBehaviour
{
  [Header("Settings")]
  public float healthOverTimer;

  public float Hunger;
	public float hungerOverTime;

	public float Thirst;
	public float thirstOverTime;

	public float Drunk;
	public float drunkOverTime;

  [Header("Sliders")]
	public Slider HungerBar;
	public Slider ThirstBar;
	public Slider DrunkBar;
  public Slider HealthBar;

  private float minAmount = 0.1f;
  private PlayerController controll;

  private void Start()
  {
		HungerBar.maxValue = Hunger;
		ThirstBar.maxValue = Thirst;
  }

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
    Hunger = Mathf.Clamp(Hunger, 0, 100f);
		Thirst = Mathf.Clamp(Thirst, 0, 100f);

    HungerBar.value = Hunger;
		ThirstBar.value = Thirst;
    CalculateValues();
  }
  private void CalculateValues()
  {
    Hunger -= hungerOverTime * Time.deltaTime;
		Thirst -= thirstOverTime * Time.deltaTime;

    if(Hunger <= minAmount || Thirst <= minAmount)
		{
			controll.Health -= healthOverTimer * Time.deltaTime;
		}
  }
  public void Drink()
	{
		Thirst += 20;
	}
}
