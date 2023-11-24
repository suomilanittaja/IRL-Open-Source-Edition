using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robbery : MonoBehaviour
{
    public GameObject Ui;
    public GameObject Info;
    public Money money;

    public void ROB()
    {
        Ui.SetActive(false);
        Info.SetActive(true);
        StartCoroutine(TiirikointiAika());
    }

    IEnumerator TiirikointiAika()
    {    
        yield return new WaitForSeconds(5);         //yield on a new YieldInstruction that waits for 5 seconds.
        Info.SetActive(false);
        yield return new WaitForSeconds(5);
        money.money += 500;
    }
}
