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
    public PlayerController controll;

    public GameObject PPP_Drunk;
	public GameObject PPP_Player;

    private void Start()
    {
        HungerBar.maxValue = Hunger;
		ThirstBar.maxValue = Thirst;
    }

    void Update()
    {
        HealthBar.value = controll.Health;
        updateUI();
    }

    private void updateUI()
    {
        HealthBar.value = controll.Health;
        Hunger = Mathf.Clamp(Hunger, 0, 100f);
	    Thirst = Mathf.Clamp(Thirst, 0, 100f);
        HungerBar.value = Hunger;
	    ThirstBar.value = Thirst;
        Drunk = Mathf.Clamp(Drunk, 0, 100f);
        DrunkBar.value = Drunk;
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

        if(Drunk <= 0)
        {
            PPP_Drunk.gameObject.SetActive(false);
            PPP_Player.gameObject.SetActive(true);
        }
        else
        {
            if(Drunk >= 0)
            {
                PPP_Player.gameObject.SetActive(false);
                PPP_Drunk.gameObject.SetActive(true);
            }
        Drunk -= drunkOverTime * Time.deltaTime;
        }
    }

    public void Drink()
	{
		Thirst += 20;
	}

    public void drunk()
	{
		Thirst += 10;
        Drunk += 20;
	}

    public void Eat()
    {
        Hunger += 20;
    }
}
