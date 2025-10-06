using System.Net;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("CameraRotation")]
    float mouseY;
    float mouseX;
    [SerializeField] Camera cam;
    Vector3 rotDir;
    public float rotSpeed;

    [Header("Movement")]
    Vector3 dir;
    [SerializeField] float speed;
    float startSpeed;
    [SerializeField] int jumpForce;
    [SerializeField] Transform legs;
    [SerializeField] float radiusSphere;
    [SerializeField] LayerMask groundMask;


    [Header("Misc")]
    public Rigidbody rb;
    [SerializeField] Vector3 scale;

    [Header("Scripts")]
    [SerializeField] CamControl camControl;
    [SerializeField] PauseMenu pauseMenuControl;

    [Header("Animation")]
    Animator animator;
    int walkState = 0;

    [Header("Bools")]
    bool ladder = false;
    bool ladderClimb = false;
    public bool isGrip = false;
    bool isOnGround = false;
    bool isRunning = false;

    [Header("Canvas")]
    [SerializeField] TMP_Text tutorialText;
    [SerializeField] GameObject tutorialTextObject;

    [Header("Optimazation")]
    [SerializeField] GameObject start;
    [SerializeField] GameObject intro;
    [SerializeField] GameObject entranceAsylum;
    [SerializeField] GameObject asylum;


    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        startSpeed = speed;
    }

    
    void Update()
    {
        if (ladder == true)
        {
            tutorialTextObject.SetActive(true);
            tutorialText.text = "Press W to climb up\nPress S to jump off";
            if (Input.GetKey(KeyCode.W))
            {
                ladderClimb = true;
                animator.SetBool("Climb",true);
                rb.useGravity = false;
                transform.Translate(0, 10 * Time.deltaTime, 0);
                walkState = 0;
            }
            if (Input.GetKeyUp(KeyCode.W))
            {
                walkState = 0;
            }
            if (Input.GetKey(KeyCode.S))
            {
                ladderClimb = false;
                tutorialTextObject.SetActive(false);
                animator.SetBool("Climb", false);
                ladder = false;
                rb.useGravity = true;
                walkState = 0;
            }

        }
        if (ladderClimb == false && isGrip == false)
        {
            Walk();
            camControl.CameraPlaneManagment();
        }
        isOnGround = Physics.CheckSphere(legs.position, radiusSphere, groundMask);
        pauseMenuControl.Options();
        RotCam();
        AnimationControl();
        pauseMenuControl.PauseMenuControl();
    }
    
    void AnimationControl()
    {
        if (walkState == 0)
        {
            animator.SetBool("Walk", false);
            animator.SetBool("CrouchWalk", false);
            animator.SetBool("Run", false);
        }
        if (walkState == 1 && !isGrip)
        {
            animator.SetBool("Walk", true);
        }
        if (walkState == 2 && !isGrip)
        {
            animator.SetBool("CrouchWalk", true);
        }
        if (walkState == 3 && !isGrip)
        {
            animator.SetBool("Run", true);
        }
        if (isGrip)
        {
            animator.SetBool("Walk", false);
            animator.SetBool("CrouchWalk", false);
            animator.SetBool("Run", false);
            tutorialTextObject.SetActive(true);
            tutorialText.text = "Press T to climb up\nPress S to jump off";
            if (Input.GetKey(KeyCode.S))
            {
                isGrip = false;
                tutorialTextObject.SetActive(false);
            }
            if (Input.GetKey(KeyCode.T))
            {
                animator.SetTrigger("Grip");
                tutorialTextObject.SetActive(false);
            }
        }
    }
    void Walk() 
    {
        dir = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
        {
            dir.z = 1;
            walkState = 1;
        }if (Input.GetKeyUp(KeyCode.W))
        {
            walkState = 0;
            speed = startSpeed;
            isRunning = false;
        }
        if (Input.GetKey(KeyCode.S))
        {
            dir.z = -1;
            walkState = 1;
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            walkState = 0;
        }
        if (Input.GetKey(KeyCode.D))
        {
            dir.x = 1;
            walkState = 1;
        }if (Input.GetKeyUp(KeyCode.D))
        {
            walkState = 0;
        }
        if (Input.GetKey(KeyCode.A))
        {
            dir.x = -1;
            walkState = 1;
        }if (Input.GetKeyDown(KeyCode.A))
        {
            walkState = 0;
        }
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if(transform.localScale.x != 2.5)
            {
                transform.localScale = new Vector3(scale.x / 2, scale.y / 2, scale.z / 2);
                speed = startSpeed / 2;
                walkState = 2;
            }
            else if(transform.localScale.x == 2.5)
            {
                transform.Translate(0, 15*Time.deltaTime, 0);
                transform.localScale = new Vector3(scale.x, scale.y, scale.z);
                speed = startSpeed;
                walkState = 0;
            }
            
        }
        if (Input.GetKeyDown(KeyCode.LeftShift) && transform.localScale.x != 2.5f)
        {
            if (isRunning)
            {
                speed = startSpeed;
                isRunning = false;
                walkState = 0;
            }
            else 
            {
                speed = startSpeed * 2.5f;
                isRunning = true;
                walkState = 3;
            }
        }
        if (Input.GetKeyDown(KeyCode.Space) && transform.localScale.x != 2.5f && isOnGround)
        {
            rb.AddForce(Vector3.up * jumpForce,ForceMode.Impulse);
            //animator.SetTrigger("Jump");
        }
        if(rb.linearVelocity.y > 0)
        {
            Physics.gravity = new Vector3(0,-80,0);
            
        }
        else
        {
            Physics.gravity = new Vector3(0, -120, 0);
        }
        dir.Normalize();
        Vector3 velocity = rb.linearVelocity;
        rb.linearVelocity = (transform.forward * dir.z + transform.right * dir.x) * speed + velocity.y * Vector3.up;
    }
    void RotCam()
    {
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");
        rotDir = new Vector3(mouseX, mouseY).normalized;
        transform.Rotate(Vector3.up, rotDir.x * rotSpeed * Time.deltaTime);
        cam.transform.Rotate(Vector3.right, rotDir.y * -rotSpeed * Time.deltaTime);
        float cameraXRot = (cam.transform.localEulerAngles.x) % 360;
        if (cameraXRot > 180)
        {
            cameraXRot -= 360;

        }
        else if (cameraXRot < -180)
        {
            cameraXRot += 360;
        }
        cameraXRot = Mathf.Clamp(cameraXRot, -45, 45);
        cam.transform.localEulerAngles = new Vector3(cameraXRot, 0, 0);
    }
   
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ladder"))
        {
            ladder = true;
        }
        if (other.CompareTag("LadderEnd") && ladderClimb)
        {
            ladderClimb = false;
            ladder = false;
            tutorialTextObject.SetActive(false);
            animator.SetBool("Climb", false);
            rb.useGravity = true;
            transform.position = other.transform.position;
        }
        if (other.CompareTag("Griip"))
        {
            isGrip = true;
            rb.useGravity = false;
            walkState = 0;
        }
        if (other.CompareTag("EnterAsylum"))
        {
            Destroy(start);
            Destroy(intro);
            asylum.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ladder"))
        {
            ladderClimb = false;
            animator.SetBool("Climb", false);
            ladder = false;
            rb.useGravity = true;
        }
        if (other.CompareTag("Griip"))
        {
            isGrip = false;
            rb.useGravity = true;
        }
    }
}
