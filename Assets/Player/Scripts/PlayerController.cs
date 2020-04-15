using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class PlayerController : MonoBehaviourPun {


    public float Speed = 10f;
    public float Health = 100;
    public Transform shotPos;
    public GameObject bulletPrefab;
    public Manager _manager;
    public CharacterController characterController;

    public float moveSpeed;
    public float sprintSpeedMultiplier = 2f;
    public float jumpHeight = 3f;
    private float _gravity = -10f;
    private float _yAxisVelocity;
    public GameObject Cam;
    public const string PlayerTag = "Player";

    private void Start()
    {
        if (photonView.IsMine)
        {
            GetComponent<MeshRenderer>().material.color = Color.blue;
            gameObject.tag = "Player";
        }
        else
        {
            GetComponent<MeshRenderer>().material.color = Color.red;
        }

        _manager = GameObject.FindWithTag("Manager").GetComponent<Manager>();
    }

    private void Update()
    {

        if (photonView.IsMine)
        {
            InputMovement();
            if (Input.GetMouseButtonDown(0))
            {
                Fire();
            }
            Cam.gameObject.SetActive(true);
        }

        if (Health <= 0)
        {
          _manager.died = true;
          PhotonNetwork.Destroy(gameObject);
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Bullet")
        ChangeHealth(-10);
    }

    void ChangeHealth(float value)
    {
        Health += value;
    }

    // used as Observed component in a PhotonView, this only reads/writes the position
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            Vector3 pos = transform.localPosition;
            stream.Serialize(ref pos);
            stream.SendNext(Health);
        }
        else
        {
            Vector3 pos = Vector3.zero;
            stream.Serialize(ref pos);  // pos gets filled-in. must be used somewhere
            Health = (float)stream.ReceiveNext();
        }
    }

    void Fire()
    {
        PhotonNetwork.Instantiate(bulletPrefab.name, shotPos.transform.position, shotPos.rotation);
    }

    void InputMovement()
    {

      float horizontal = Input.GetAxis("Horizontal");
      float vertical = Input.GetAxis("Vertical");

      if (Input.GetKey(KeyCode.LeftShift))
      vertical *= sprintSpeedMultiplier;

      Vector3 movement = horizontal * moveSpeed * Time.deltaTime * transform.right +
                         vertical * moveSpeed * Time.deltaTime * transform.forward;

      if (characterController.isGrounded)
          _yAxisVelocity = -0.5f;


      if (Input.GetKeyDown(KeyCode.Space))
      {
            _yAxisVelocity = Mathf.Sqrt(jumpHeight * -2f * _gravity);
      }

      _yAxisVelocity += _gravity * Time.deltaTime;
      movement.y = _yAxisVelocity * Time.deltaTime;

      characterController.Move(movement);
  }
}
