using UnityEngine;

public class DeliveryPoint : MonoBehaviour
{
    private float yOffset = -100f;
    private Money money;

    // Kun pelaaja törmää Waypointiin
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Pelaaja törmäsi waypointiin: " + gameObject.name);
            money = GameObject.FindWithTag("Player").GetComponent<Money>(); //etsi scripti
            // Siirrä objekti Y-akselilla 100 yksikköä ylöspäin
            Vector3 newPosition = transform.position;
            newPosition.y += yOffset;
            transform.position = newPosition;

            money.money += 500;
            money.Given = false;
        }
    }
}
