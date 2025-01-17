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

    [Header("Text")]
    public TextMeshProUGUI text;
    public TextMeshProUGUI rewardText;
    public TextMeshProUGUI currentBalanceText;

    public bool Payed = false;
    public bool waypointGiven = false;

    private float yOffset = 100f;

    [Header("Banking")]
    public GameObject robbingUi;
    public GameObject bankingUi;
    public Transform lockSprite; // Assign the lock dial GameObject in Unity
    public float lockRotationSpeed = 100f;
    public float unlockAngle = 180f; // The correct angle to unlock
    private bool isUnlocked = false;
    public bool robbing = false;

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

        if (job == 0)
        {
            StartCoroutine(SupportMoney());
        }

        if (job == 1)
        {
            StartCoroutine(RewardMoney());
        }

        if (job == 3 && waypointGiven == false)
        {
            GameObject[] waypoints = GameObject.FindGameObjectsWithTag("Waypoint"); // Find all objects with Waypoint tag


            if (waypoints.Length > 0)                                               //Chech if there is objects before random choice
            {
                int randomIndex = Random.Range(0, waypoints.Length);                //Choice random waypoint

                GameObject randomWaypoint = waypoints[randomIndex];                 //find random waypoint object

                //Move object +100 y
                Vector3 newPosition = randomWaypoint.transform.position;
                newPosition.y += yOffset;
                randomWaypoint.transform.position = newPosition;

                randomWaypoint.SetActive(true);                                     //set object active

                waypointGiven = true;
            }
            else
            {
                Debug.LogWarning("object could not found 'Waypoint'.");
            }
        }

        if (robbing == true)
        {
            bankingUi.SetActive(false);
            robbingUi.SetActive(true);
            if (!isUnlocked)
            {
                // Rotate the lock dial
                lockSprite.Rotate(0, 0, lockRotationSpeed * Time.deltaTime);

                // Check if the player presses Space at the right time
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    float currentAngle = lockSprite.eulerAngles.z;

                    // Check if the player pressed at the correct angle
                    if (Mathf.Abs(currentAngle - unlockAngle) < 10f) // 10-degree tolerance
                    {
                        Debug.Log("Unlocked!");
                        robbing = false;
                        isUnlocked = true;
                        robbingUi.SetActive(false);
                        isUnlocked = false;
                        money += 500;
                    }
                    else
                    {
                        Debug.Log("Failed! Try again.");
                    }
                }
            }
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

   

    IEnumerator WaitTime()
    {
        yield return new WaitForSeconds(10);
        rewardText.text = "";
        yield return new WaitForSeconds(20);
        Payed = false;
    }

    public void ROB()
    {
        robbing = true;
    }
}
