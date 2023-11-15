using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLookAround : MonoBehaviour
{
    // Start when the player moves its mouse around

    public Transform player;
    public float mouseSensitivity = 2f; // Mouse sensitivity
    float cameraVerticalRotation = 0f;

    void Start()
    {
        //
    }

    // Update is called once per frame
    void Update()
    {
        // get the mouse input
        float cameraX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float cameraY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // rotate the camera around the local X Axis
        cameraVerticalRotation -= cameraY;
        cameraVerticalRotation = Mathf.Clamp(cameraVerticalRotation, -90f, 90f);
        transform.localEulerAngles = Vector3.right * cameraVerticalRotation;

        // rotate the camera around the local Y Axis
        player.Rotate(Vector3.up * cameraX);
    }
}
