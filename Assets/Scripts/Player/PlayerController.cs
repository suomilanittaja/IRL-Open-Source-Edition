using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class PlayerController : MonoBehaviourPun, IPunObservable
{

    [Header("Floats")]
    public float Health = 100;
    public float walkSpeed = 5f;  //new
    public float jumpForce = 5f;
    public float sensitivity = 2f;

    [Header("GameObjects")]
    public GameObject bulletPrefab;
    public GameObject Minicam;
    public GameObject Cam;
    public GameObject Remote;
    public GameObject Canvas;
    public GameObject GunUi;

    [Header("Bools")]
    public bool isGrounded;
    public bool hasGun = false;
    public bool isEntered;
    public bool isTabPressed;

    [Header("Others")]
    public Transform shotPos;
    public const string PlayerTag = "Player";
    public DrinkingEatingAndPicking pickUp;
    public Rigidbody rb;
   

    public Manager _manager;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        CheckOwnership();
    }



    private void Update()
    {
        if (photonView.IsMine)
        {
            // Handle player movement
            HandleMovement();

            // Handle player look (mouse input)
            HandleMouseLook();

            // Handle jumping
            HandleJump();

            CheckHealth();

            HandleEntryExit();        
        }
    }


    // used as Observed component in a PhotonView, this only reads/writes the position
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            //stream.SendNext(transform.position);
            //stream.SendNext(transform.rotation);
            stream.SendNext(Health);
        }
        else
        {
            //transform.position = (Vector3)stream.ReceiveNext();
            //transform.rotation = (Quaternion)stream.ReceiveNext();
            Health = (float)stream.ReceiveNext();
        }
    }


    private void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 moveDirection = new Vector3(horizontal, 0f, vertical).normalized;
        Vector3 moveAmount = moveDirection * walkSpeed * Time.deltaTime;
        transform.Translate(moveAmount); // Move the player

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            GunUi.SetActive(true);
            ToggleCursorLock(false);
        }
        else if (Input.GetKeyUp(KeyCode.Tab))
        {
            GunUi.SetActive(false);
            ToggleCursorLock(true);
        }

        if (Input.GetMouseButtonDown(0) && hasGun && pickUp.usingGun && !Cursor.visible)
        {
            Fire();
        }
        Cam.gameObject.SetActive(true);
    }

    private void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;
        transform.Rotate(Vector3.up * mouseX);  // Rotate the player based on mouse input
        float currentRotation = Camera.main.transform.eulerAngles.x; // Rotate the camera (up and down) based on mouse input
        float newRotation = currentRotation - mouseY;
        newRotation = Mathf.Clamp(newRotation, 0f, 90f); // Limit the camera rotation to avoid flipping
        Camera.main.transform.localRotation = Quaternion.Euler(newRotation, 0f, 0f);
    }

    private void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange); // Apply a vertical force for jumping
            isGrounded = false;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        // Check if the player is grounded
        isGrounded = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Bullet")
            ChangeHealth(-10);
    }

    void ChangeHealth(float value)
    {
        Health += value;
    }

    private void Fire()
    {
        PhotonNetwork.Instantiate(bulletPrefab.name, shotPos.transform.position, shotPos.rotation);
    }

    [PunRPC]
    private void EnterExit(bool enter)
    {
        gameObject.SetActive(!enter);
    }


    private void CheckOwnership()
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
            GetComponent<DrinkingEatingAndPicking>().enabled = false;
        }
    }

    private void CheckHealth()
    {
        if (Health <= 0)
        {
            _manager.died = true;
            StartCoroutine(Die());
        }
    }

    public void ToggleCursorLock(bool lockCursor)
    {
        Cursor.lockState = lockCursor ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !lockCursor;
    }

    private void HandleEntryExit()
    {
        if (isEntered)
            photonView.RPC("EnterExit", RpcTarget.All, true);
        else
            photonView.RPC("EnterExit", RpcTarget.All, false);
    }

    public void useGun()
    {
        if (hasGun)
        {
            pickUp.usingGun = true;
        }
    }

    public void useHand()
    {
        pickUp.usingGun = false;
    }

    IEnumerator Die()
    {
        yield return new WaitForSeconds(5);         //yield on a new YieldInstruction that waits for 5 seconds.
        PhotonNetwork.Destroy(gameObject);
    }
}
