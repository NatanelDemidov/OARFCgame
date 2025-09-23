using System.Net;
using UnityEngine;

public class Player : MonoBehaviour
{
    Vector3 dir;
    [SerializeField] int speed = 20;
    Rigidbody rb;
    float mouseX;
    [SerializeField] Vector3 scale;
    Animator animator;
    float mouseY;
    Vector3 rotDir;
    [SerializeField] int rotSpeed;
    [SerializeField] Camera cam;
    bool ladder = false;
    bool ladderClimb = false;
    int walkState = 0;
    [SerializeField] int jumpForce;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    
    void Update()
    {
        if (ladder == true)
        {
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
                animator.SetBool("Climb", false);
                ladder = false;
                rb.useGravity = true;
                walkState = 0;
            }

        }
        if (ladderClimb == false)
        {
            Walk();
            
        }
        RotCam();
        AnimationControl();

    }
    void AnimationControl()
    {
        if (walkState == 0)
        {
            animator.SetBool("Walk", false);
            animator.SetBool("CrouchWalk", false);
            animator.SetBool("Run", false);
        }
        if (walkState == 1)
        {
            animator.SetBool("Walk", true);
        }
        if (walkState == 2)
        {
            animator.SetBool("CrouchWalk", true);
        }
        if (walkState == 3)
        {
            animator.SetBool("Run", true);
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
            transform.localScale = new Vector3(scale.x/2,scale.y/2,scale.z/2);
            speed = 20;
            walkState = 2;
        }
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            transform.Translate(0,25,0);
            transform.localScale = new Vector3(scale.x, scale.y, scale.z);
            speed = 40;
            walkState = 0;
        }
        if (Input.GetKeyDown(KeyCode.LeftShift) && transform.localScale.x != 2.5f)
        {
            speed = 100;
            walkState = 3;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            speed = 40;
            walkState = 0;
        }
        if (Input.GetKeyDown(KeyCode.Space) && transform.localScale.x != 2.5f)
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
            animator.SetBool("Climb", false);
            rb.useGravity = true;
            transform.position = other.transform.position;
        }
        if (other.CompareTag("Griip"))
        {
            //animator.SetTrigger("Grip");
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
    }
}
