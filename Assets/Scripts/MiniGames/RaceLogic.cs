using UnityEngine;

public class RaceLogic : MonoBehaviour
{
    public GameObject startLineObject;
    public GameObject[] waypointsObjects;

    private int currentWaypointIndex = 0;
    public bool gameStarted = false;


    public void ShowNextWaypoint()
    {
        if (currentWaypointIndex < waypointsObjects.Length)
        {
            foreach (GameObject waypoint in waypointsObjects)
            {
                waypoint.SetActive(false); // Hide all waypoints
            }

            waypointsObjects[currentWaypointIndex].SetActive(true); // Show the next waypoint
        }
    }

    void Update()
    {
        if (gameStarted && waypointsObjects.Length > 0)
        {
            GameObject currentWaypoint = waypointsObjects[currentWaypointIndex];

            foreach (GameObject vehicle in GameObject.FindGameObjectsWithTag("Vehicle"))
            {
                // Check if the vehicle is close enough to the current waypoint
                if (Vector3.Distance(vehicle.transform.position, currentWaypoint.transform.position) < 2f)
                {
                    currentWaypointIndex++;

                    // Check if the currentWaypointIndex is still within bounds
                    if (currentWaypointIndex < waypointsObjects.Length)
                    {
                        ShowNextWaypoint(); // Show the next waypoint
                    }
                    else
                    {
                        Debug.Log("All waypoints reached!");
                        Debug.Log(" finished the race!");
                        gameStarted = false;
                        startLineObject.SetActive(true);
                        currentWaypointIndex = 0;
                        foreach (GameObject waypoint in waypointsObjects)
                        {
                            waypoint.SetActive(false); // Hide all waypoints
                        }
                    }
                }
            }
        }
    }
}
