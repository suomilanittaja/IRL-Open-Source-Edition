using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime; // Tämä voi olla tarpeen myös, riippuen käyttämästäsi Photon-toiminnallisuudesta.


public class CarController : MonoBehaviourPunCallbacks, IPunObservable
{
    public enum Drivetrain {FWD,
        RWD,
        FourWD};

    private float horizontalInput, verticalInput;
    private float steeringAngle;


    [Header("Car Settings")]
    public Drivetrain drivetrain = Drivetrain.FWD;
    public float maxSteeringAngle = 30;
    public float torque = 1000;
    public float brakeTorque = 1000;
    public Transform CenterOfMass;
    public Rigidbody RigidBody;

    [Header("Wheel Colliders")]
    public WheelCollider frontRightWheel;
    public WheelCollider frontLeftWheel;
    public WheelCollider rearRightWheel;
    public WheelCollider rearLeftWheel;

    [Header("Wheel Models")]
    public Transform frontRightTransform;
    public Transform frontLeftTransform;
    public Transform rearRightTransfrom;
    public Transform rearLeftTransform;

    private void GetInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
    }

    void Start()
    {
      RigidBody.centerOfMass = CenterOfMass.localPosition;
    }
    private void Accelerate()
    {
        switch (drivetrain)
        {
            case Drivetrain.FWD:
                frontLeftWheel.motorTorque = torque * verticalInput;
                frontRightWheel.motorTorque = torque * verticalInput;
                break;
            case Drivetrain.RWD:
                rearLeftWheel.motorTorque = torque * verticalInput;
                rearRightWheel.motorTorque = torque * verticalInput;
                break;
            case Drivetrain.FourWD:
                frontLeftWheel.motorTorque = torque * verticalInput;
                frontRightWheel.motorTorque = torque * verticalInput;
                rearLeftWheel.motorTorque = torque * verticalInput;
                rearRightWheel.motorTorque = torque * verticalInput;
                break;
        }
    }

    private void Steer()
    {
        steeringAngle = maxSteeringAngle * horizontalInput;
        frontLeftWheel.steerAngle = steeringAngle;
        frontRightWheel.steerAngle = steeringAngle;
    }

    private void UpdateWheelPoses()
    {
        UpdateWheelPose(frontLeftWheel, frontLeftTransform);
        UpdateWheelPose(frontRightWheel, frontRightTransform);
        UpdateWheelPose(rearLeftWheel,rearLeftTransform);
        UpdateWheelPose(rearRightWheel, rearRightTransfrom);
    }

    private void UpdateWheelPose(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos = wheelTransform.position;
        Quaternion rot = wheelTransform.rotation;
        wheelCollider.GetWorldPose(out pos,out rot);
        wheelTransform.position = pos;
        wheelTransform.rotation = rot;
    }


    private void FixedUpdate()
    {
        GetInput();
        Steer();
        Accelerate();
        UpdateWheelPoses();
        HandleBraking();
    }

    private void HandleBraking()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            // Pysäytysohjeet
            rearLeftWheel.brakeTorque = brakeTorque;
            rearRightWheel.brakeTorque = brakeTorque;
        }
        else
        {
            // Vapauta jarrut
            rearLeftWheel.brakeTorque = 0;
            rearRightWheel.brakeTorque = 0;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // Lähetetään tiedot
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
            stream.SendNext(RigidBody.linearVelocity);
        }
        else
        {
            // Vastaanotetaan tiedot
            transform.position = (Vector3)stream.ReceiveNext();
            transform.rotation = (Quaternion)stream.ReceiveNext();
            RigidBody.linearVelocity = (Vector3)stream.ReceiveNext();
        }
    }
}
