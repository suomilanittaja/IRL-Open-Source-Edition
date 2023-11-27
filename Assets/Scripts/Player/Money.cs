using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Money : MonoBehaviour
{
    [Header("Ints")]
    public int money;
    public int job = 10;

    [Header("GameObjects")]
    public GameObject RobbingUi;
    public GameObject RobbingInfo;

    [Header("Text")]
    public TextMeshProUGUI text;
    public TextMeshProUGUI rewardText;

    public bool Payed = false;

    // Start is called before the first frame update
    void Start()
    {
        money = 100;
        rewardText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        text.text = money.ToString() + "€";

        if (job == 1)
        {
            StartCoroutine(RewardMoney());
        }
        if (job == 0)
        {
            StartCoroutine(SupportMoney());
        }
    }

    IEnumerator RewardMoney()
    {
        yield return new WaitForSeconds(15);
        if (job == 1 && Payed == false)
        {
            rewardText.text = "RewardMoney + 20€";
            money += 20;
            Payed = true;
            StartCoroutine(WaitTime());
        }
    }

    IEnumerator SupportMoney()
    {
        yield return new WaitForSeconds(15);
        if (job == 0 && Payed == false)
        {
            rewardText.text = "unemployment benefit + 5€";
            money += 5;
            Payed = true;
            StartCoroutine(WaitTime());
        }
    }

    IEnumerator WaitTime()
    {
        yield return new WaitForSeconds(10);
        rewardText.text = "";
        yield return new WaitForSeconds(20);
        Payed = false;
    }

    public void ROB()
    {
        RobbingUi.SetActive(false);
        RobbingInfo.SetActive(true);
        StartCoroutine(WaitingTime());
    }

    IEnumerator WaitingTime()
    {
        yield return new WaitForSeconds(5);         //yield on a new YieldInstruction that waits for 5 seconds.
        RobbingInfo.SetActive(false);
        yield return new WaitForSeconds(5);
        money += 500;
    }
}
