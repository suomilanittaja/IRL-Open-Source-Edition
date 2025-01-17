using UnityEngine;

public class DeliveryPoint : MonoBehaviour
{
    private float yOffset = -100f;
    private Money moneyScript;

    // when something hit waypoint
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))         //check if it is player
        {
            moneyScript = GameObject.FindWithTag("Player").GetComponent<Money>(); //find moneyScript with tag

            // move waypoint -100 in y position
            Vector3 newPosition = transform.position;
            newPosition.y += yOffset;
            transform.position = newPosition;

            moneyScript.money += 500;           //give money
            moneyScript.waypointGiven = false;          //request for new waypoint
        }
    }
}
