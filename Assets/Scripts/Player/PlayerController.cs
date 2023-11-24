using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class PlayerController : MonoBehaviourPun, IPunObservable
{

    [Header("Floats")]
    public float Speed = 10f;
    public float Health = 100;
    public float moveSpeed;
    public float sprintSpeedMultiplier = 2f;
    public float jumpHeight = 3f;

    [Header("GameObjects")]
    public GameObject bulletPrefab;
    public GameObject Minicam;
    public GameObject Cam;
    public GameObject Remote;
    public GameObject Canvas;

    [Header("Bools")]
    public bool hasGun = false;
    public bool isEntered;
    public bool tabPressed;
    public bool disableMouselook = false;

    [Header("Others")]
    public Transform shotPos;
    public CharacterController characterController;
    public const string PlayerTag = "Player";
    public PickUp pickUp;
    public PlayerController playerController;

    private PlayerCamera playerCamera;
    public Manager _manager;
    private float _gravity = -10f;
    private float _yAxisVelocity;

    private void Start()
    {       
        playerCamera = Cam.GetComponent<PlayerCamera>();
    }

    public void Lock()
    {
      Cursor.lockState = CursorLockMode.Locked; //lock cursor
      Cursor.visible = false; //disable visible mouse
      disableMouselook = false;
    }

    public void UnLock()
    {
      disableMouselook = true;
      Cursor.lockState = CursorLockMode.None; //unlock cursor
      Cursor.visible = true; //make mouse visible
    }

    public void check()
    {
        if (photonView.IsMine)
        {
            GetComponent<MeshRenderer>().material.color = Color.blue;
            gameObject.tag = "Player";
            Remote.SetActive(false);
            Canvas.SetActive(true);
        }
        else
        {
            GetComponent<MeshRenderer>().material.color = Color.red;
            Cam.SetActive(false);
            Minicam.SetActive(false);
            Remote.SetActive(true);
            Canvas.SetActive(false);
            //GetComponent<CharacterController>().enabled = false;
            GetComponent<PickUp> ().enabled = false;
            //GetComponent<PlayerController>().enabled = false;
        }
    }

    private void Update()
    {
        check();

        if (photonView.IsMine)
        {
            InputMovement();
            if (Input.GetKeyDown(KeyCode.Tab))
            {
              tabPressed = true;
              UnLock();
            }
            else if (Input.GetKeyUp(KeyCode.Tab))
            {
              tabPressed = false;
              Lock();
            }

            if (Input.GetMouseButtonDown(0) && hasGun == true && pickUp.usingGun == true && Cursor.visible == false)
            {
                Fire();
            }
            Cam.gameObject.SetActive(true);

            if (disableMouselook == true)
              playerCamera.enabled = false;
            else
              playerCamera.enabled = true;


            if (Health <= 0)
            {
                _manager.died = true;
                StartCoroutine(Kuolema());
            }

            if (isEntered == true)
            {
                photonView.RPC("enter", RpcTarget.All);
            }

            if (isEntered == false)
            {
                photonView.RPC("exit", RpcTarget.All);
            }
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
            //Vector3 pos = transform.localPosition;
            //stream.Serialize(ref pos);
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
            stream.SendNext(Health);
        }
        else
        {
            //Vector3 pos = Vector3.zero;
            //stream.Serialize(ref pos);  // pos gets filled-in. must be used somewhere
            transform.position = (Vector3)stream.ReceiveNext();
            transform.rotation = (Quaternion)stream.ReceiveNext();
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

  [PunRPC]
  void enter()
  {
    this.gameObject.SetActive(false);
  }

  [PunRPC]
  void exit()
  {
    this.gameObject.SetActive(true);
  }

    IEnumerator Kuolema()
    {
        yield return new WaitForSeconds(5);         //yield on a new YieldInstruction that waits for 5 seconds.
        PhotonNetwork.Destroy(gameObject);
    }
}
