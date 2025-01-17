using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerController : MonoBehaviourPun, IPunObservable
{
    [Header("Floats")]
    public float Health = 100;
    public float walkSpeed = 5f;
    public float jumpForce = 5f;
    public float sensitivity = 2f;
    public float interpolationSpeed = 10f;

    [Header("GameObjects")]
    public GameObject bulletObject;
    public GameObject miniCamObject;
    public GameObject camObject;
    public GameObject remoteObject;
    public GameObject canvasObject;
    public GameObject gunUi;

    [Header("Bools")]
    public bool isGrounded;
    public bool hasGun = false;
    public bool isEntered;
    public bool canEnter;
    public bool isTabPressed;
    public bool Cursorlock = true;

    [Header("Others")]
    public Transform shotPos;
    public DrinkingEatingAndPicking drinkEatAndPickScript;
    public Rigidbody rigidbodyScript;
    public GameObject driveText;

    public Manager managerScript;

    private Vector3 targetPosition;
    private Quaternion targetRotation;
    private Vector3 networkVelocity;
    private Vector3 networkAngularVelocity;
    private float lastNetworkUpdateTime;

    private void Start()
    {
        rigidbodyScript = GetComponent<Rigidbody>();
        CheckOwnership();
        targetPosition = transform.position;
        targetRotation = transform.rotation;
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            // Handle player movement
            HandleMovement();
            HandleJump();
            CheckHealth();
            HandleEntryExit();

            if (Cursorlock)
            {
                HandleMouseLook();
            }
        }
        else
        {
            SmoothMovement();
        }
    }

    // Photon serialization to sync position, rotation, velocity, and health across clients
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
            stream.SendNext(rigidbodyScript.linearVelocity);
            stream.SendNext(rigidbodyScript.angularVelocity);
            stream.SendNext(Health);
        }
        else
        {
            targetPosition = (Vector3)stream.ReceiveNext();
            targetRotation = (Quaternion)stream.ReceiveNext();
            networkVelocity = (Vector3)stream.ReceiveNext();
            networkAngularVelocity = (Vector3)stream.ReceiveNext();
            Health = (float)stream.ReceiveNext();

            lastNetworkUpdateTime = Time.time;
        }
    }

    private void SmoothMovement()
    {
        float timeSinceUpdate = Time.time - lastNetworkUpdateTime;
        Vector3 extrapolatedPosition = targetPosition + networkVelocity * timeSinceUpdate;
        Quaternion extrapolatedRotation = targetRotation * Quaternion.Euler(networkAngularVelocity * Mathf.Rad2Deg * timeSinceUpdate);

        transform.position = Vector3.Lerp(transform.position, extrapolatedPosition, Time.deltaTime * interpolationSpeed);
        transform.rotation = Quaternion.Slerp(transform.rotation, extrapolatedRotation, Time.deltaTime * interpolationSpeed);
    }

    private void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 moveDirection = new Vector3(horizontal, 0f, vertical).normalized;
        Vector3 moveAmount = moveDirection * walkSpeed * Time.deltaTime;
        transform.Translate(moveAmount);

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            gunUi.SetActive(true);
            ToggleCursorLock(false);
        }
        else if (Input.GetKeyUp(KeyCode.Tab))
        {
            gunUi.SetActive(false);
            ToggleCursorLock(true);
        }

        if (Input.GetMouseButtonDown(0) && hasGun && drinkEatAndPickScript.usingGun && !Cursor.visible)
        {
            Fire();
        }
        camObject.gameObject.SetActive(true);

        if (canEnter == true)
            driveText.gameObject.SetActive(true);
        else
            driveText.gameObject.SetActive(false);
    }

    private void HandleMouseLook()
    {
        if (camObject != null)
        {
            float mouseX = Input.GetAxis("Mouse X") * sensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * sensitivity;
            transform.Rotate(Vector3.up * mouseX);
            float currentRotation = Camera.main.transform.eulerAngles.x;
            float newRotation = currentRotation - mouseY;
            newRotation = Mathf.Clamp(newRotation, 0f, 180f);
            Camera.main.transform.localRotation = Quaternion.Euler(newRotation, 0f, 0f);
        }
    }

    private void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rigidbodyScript.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
            isGrounded = false;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        isGrounded = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("enter!"); // Log when player is hit
        if (other.CompareTag("Bullet"))
        {
            Debug.Log("Player hit by bullet!"); // Log when player is hit
            ChangeHealth(-10);
        }
    }

    void ChangeHealth(float value)
    {
        Health += value;
    }

    private void Fire()
    {
        PhotonNetwork.Instantiate(bulletObject.name, shotPos.position, shotPos.rotation);
    }

    private void CheckOwnership()
    {
        if (photonView.IsMine)
        {
            gameObject.tag = "Player";
            remoteObject.SetActive(false);
            canvasObject.SetActive(true);
        }
        else
        {
            camObject.SetActive(false);
            miniCamObject.SetActive(false);
            remoteObject.SetActive(true);
            canvasObject.SetActive(false);
            GetComponent<DrinkingEatingAndPicking>().enabled = false;
        }
    }

    private void HandleEntryExit()
    {
            if (isEntered)
                photonView.RPC("EnterExit", RpcTarget.All, true);
            if (isEntered == false && canEnter == true)
                photonView.RPC("EnterExit", RpcTarget.All, false);
    }

    private void CheckHealth()
    {
        if (Health <= 0)
        {
            managerScript.died = true;
            StartCoroutine(Die());
        }
    }

    public void ToggleCursorLock(bool lockCursor)
    {
        Cursorlock = lockCursor; // Ensure this is updated first
        Cursor.lockState = lockCursor ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !lockCursor;
    }

    public void useGun()
    {
        if (hasGun)
        {
            drinkEatAndPickScript.usingGun = true;
        }
    }

    public void useHand()
    {
        drinkEatAndPickScript.usingGun = false;
    }

    IEnumerator Die()
    {
        yield return new WaitForSeconds(2);
        PhotonNetwork.Destroy(gameObject);
    }

    [PunRPC]
    private void EnterExit(bool enter)
    {
        gameObject.SetActive(!enter);
    }
}




