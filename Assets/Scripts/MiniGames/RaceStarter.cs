using UnityEngine;

public class RaceStarter : MonoBehaviour
{

    public RaceLogic raceLogicScript;

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("trigger");

        if (other.CompareTag("Vehicle") && raceLogicScript.gameStarted == false)
        {
            Debug.Log("Race starting!");
            raceLogicScript.gameStarted = true;
            raceLogicScript.ShowNextWaypoint();
            gameObject.SetActive(false);
        }
    }
}