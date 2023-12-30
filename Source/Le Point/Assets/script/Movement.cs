using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
// These videos take long to make so I hope this helps you out and if you want to help me out you can by leaving a like and subscribe, thanks!
 
public class Movement : MonoBehaviour
{
    [SerializeField] Transform playerCamera;
    [SerializeField][Range(0.0f, 0.5f)] float mouseSmoothTime = 0.03f;
    [SerializeField] bool cursorLock = true;
    [SerializeField] float mouseSensitivity = 3.5f;
    [SerializeField] float Speed = 6.0f;
    [SerializeField][Range(0.0f, 0.5f)] float moveSmoothTime = 0.3f;
    [SerializeField] float gravity = -30f;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask ground;  
 
    public float jumpHeight = 6f;
    float velocityY;
    bool isGrounded;
 
    float cameraCap;
    Vector2 currentMouseDelta;
    Vector2 currentMouseDeltaVelocity;
    
    CharacterController controller;
    Vector2 currentDir;
    Vector2 currentDirVelocity;
    Vector3 velocity;

    bool checksMovement = false; // Variable to track if movement is enabled

    void Start()
    {
        controller = GetComponent<CharacterController>();
 
        if (cursorLock)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = true;
        }
    }
 
    void Update()
    {
        UpdateMouse();

        // Check the player's score and enable/disable movement accordingly
        if (ScoreManager.scoreCount >= 25 && ScoreManager.scoreCount < 30)
        {
            checksMovement = true;
        }
        else if (ScoreManager.scoreCount >= 30 && ScoreManager.scoreCount < 50)
        {
            // Handle the movement for score between 30 and 50
            checksMovement = true;
        }
        else if (ScoreManager.scoreCount >= 50 && ScoreManager.scoreCount < 55)
        {
            // Handle the movement for score between 50 and 55
            checksMovement = true;
        }
        // Add more conditions for other score ranges as needed
        else if (ScoreManager.scoreCount >= 55 && ScoreManager.scoreCount < 75)
        {
            // Handle the movement for score between 55 and 75
            checksMovement = true;
        }
        else if (ScoreManager.scoreCount >= 75 && ScoreManager.scoreCount < 80)
        {
            // Handle the movement for score between 75 and 80
            checksMovement = true;
        }
        else if (ScoreManager.scoreCount >= 80 && ScoreManager.scoreCount < 99)
        {
            // Handle the movement for score between 80 and 99
            checksMovement = true;
        }
        else
        {
            // Disable movement for any other cases
            checksMovement = false;
        }

        if (checksMovement)
        {
            UpdateMove();
        }
    }
 
    void UpdateMouse()
    {
        Vector2 targetMouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
 
        currentMouseDelta = Vector2.SmoothDamp(currentMouseDelta, targetMouseDelta, ref currentMouseDeltaVelocity, mouseSmoothTime);
 
        cameraCap -= currentMouseDelta.y * mouseSensitivity;
 
        cameraCap = Mathf.Clamp(cameraCap, -90.0f, 90.0f);
 
        playerCamera.localEulerAngles = Vector3.right * cameraCap;
 
        transform.Rotate(Vector3.up * currentMouseDelta.x * mouseSensitivity);
    }
 
    void UpdateMove()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.2f, ground);

        // Only proceed with movement if isMovementEnabled is true
        if (checksMovement)
        {
            Vector2 targetDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            targetDir.Normalize();

            currentDir = Vector2.SmoothDamp(currentDir, targetDir, ref currentDirVelocity, moveSmoothTime);

            velocityY += gravity * 2f * Time.deltaTime;

            Vector3 velocity = (transform.forward * currentDir.y + transform.right * currentDir.x) * Speed + Vector3.up * velocityY;

            controller.Move(velocity * Time.deltaTime);

            if (isGrounded && Input.GetButtonDown("Jump"))
            {
                velocityY = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }

            if (isGrounded! && controller.velocity.y < -1f)
            {
                velocityY = -8f;
            }
        }
    }
}

//video https://www.youtube.com/watch?v=yl2Tv72tV7U

// Movement check sourse: ChatGPT