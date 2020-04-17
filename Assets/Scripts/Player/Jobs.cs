using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Jobs : MonoBehaviour
{
    public int job = 10;
    public bool Payed = false;
    public Money money;
    public TextMeshProUGUI text;
    // Start is called before the first frame update
    void Start()
    {
      text.text = "";
    }

    // Update is called once per frame
    void Update()
    {
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
        text.text = "RewardMoney + 20€";
        money.money += 20;
        Payed = true;
        StartCoroutine(WaitTime());
      }
    }
    IEnumerator SupportMoney()
    {
      yield return new WaitForSeconds(15);
      if (job == 0 && Payed == false)
      {
        text.text = "unemployment benefit + 5€";
        money.money += 5;
        Payed = true;
        StartCoroutine(WaitTime());
      }
    }
    IEnumerator WaitTime()
    {
      yield return new WaitForSeconds(10);
      text.text = "";
      yield return new WaitForSeconds(20);
      Payed = false;
    }
}
