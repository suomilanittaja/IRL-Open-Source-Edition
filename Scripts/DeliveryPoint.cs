using UnityEngine;

public class DeliveryPoint : MonoBehaviour
{
    private float yOffset = -100f;
    private Money money;

    // Kun pelaaja t�rm�� Waypointiin
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Pelaaja t�rm�si waypointiin: " + gameObject.name);
            money = GameObject.FindWithTag("Player").GetComponent<Money>(); //etsi scripti
            // Siirr� objekti Y-akselilla 100 yksikk�� yl�sp�in
            Vector3 newPosition = transform.position;
            newPosition.y += yOffset;
            transform.position = newPosition;

            money.money += 500;
            money.Given = false;
        }
    }
}
