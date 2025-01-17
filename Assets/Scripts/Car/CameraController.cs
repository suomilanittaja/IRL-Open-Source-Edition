using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform targetPosition;
    public Vector3 offset;
    public float speed = 10;
    public float roatationSpeed = 20;

    public void LookAt()
    {
        Vector3 direction = targetPosition.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, roatationSpeed * Time.deltaTime);
    }

    public void MoveTo()
    {
        Vector3 position = targetPosition.position + targetPosition.forward * offset.z
                                           + targetPosition.right * offset.x
                                           + targetPosition.up * offset.y;
        transform.position = Vector3.Lerp(transform.position, position, speed * Time.deltaTime);
    }

    public void FixedUpdate()
    {
        LookAt();
        MoveTo();
    }

    public void Check()
    {
        targetPosition = GameObject.FindWithTag("Vehicle").GetComponent<Transform>();
    }
}
